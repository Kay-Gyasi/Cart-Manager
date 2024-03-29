﻿using Hubtel.ECommerce.API.Core.Application.Exceptions;
using Hubtel.ECommerce.API.Core.Domain.Entities;
using Hubtel.ECommerce.API.Infrastructure.Persistence;
using LinqKit;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OneOf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;

namespace Hubtel.ECommerce.API.Core.Application.Carts
{
    public class CartProcessor
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<CartProcessor> _logger;
        private readonly AppDbContext _dbContext;
        private IQueryable<Cart> Carts => _dbContext.Carts
                                            .Include(x => x.User)
                                            .Include(x => x.CartEntries)
                                            .ThenInclude(x => x.Item);

        public CartProcessor(IHttpContextAccessor httpContextAccessor, ILogger<CartProcessor> logger,
            AppDbContext dbContext)
        {
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
            _dbContext = dbContext;
        }

        public async Task<OneOf<int, Exception>> AddToCart(CartCommand command)
        {
            try
            {
                var cartEntries = command.Items.Select(x => CartEntry.Create(x.ItemId, x.Quantity ?? 0));
                var cart = Carts.SingleOrDefault(x => x.UserId == GetUserId());
                if (cart != null)
                {
                    return await UpsertCartItem(cart, cartEntries, isUpdate: true);
                }

                cart = Cart.Create(GetUserId());
                return await UpsertCartItem(cart, cartEntries);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error: {Error}", JsonSerializer.Serialize(ex));
                return ex;
            }
        }

        public async Task<OneOf<bool, Exception>> RemoveFromCart(int itemId)
        {
            try
            {
                var cart = Carts.SingleOrDefault(x => x.UserId == GetUserId());
                if (cart == null)
                {
                    cart = Cart.Create(GetUserId());
                    return await _dbContext.SaveChangesAsync() > 0;
                }

                cart.RemoveFromCart(itemId);
                return await _dbContext.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error: {Error}", JsonSerializer.Serialize(ex));
                return ex;
            }
        }

        public async Task<OneOf<CartItemsDto, Exception>> GetAllCartItems(Filter filter)
        {
            try
            {
                var cartItems = new List<CartEntry>();
                var cartEntryFilter = PredicateBuilder.New<CartEntry>(x => true);
                var cartFilter = PredicateBuilder.New<Cart>(x => true);

                BuildFilters(filter, cartEntryFilter, cartFilter);

                await Carts.Where(cartFilter).ForEachAsync(x =>
                {
                    cartItems.AddRange(x.CartEntries.Where(cartEntryFilter));
                });

                return (CartItemsDto) cartItems;
            }
            catch(Exception ex)
            {
                _logger.LogError("Error: {Error}", JsonSerializer.Serialize(ex));
                return ex;
            }
        }

        public async Task<OneOf<CartDto, Exception>> GetCart(string? filter)
        {
            try
            {
                var cart = await Task.Run(() => Carts.SingleOrDefault(x => x.UserId == GetUserId()));
                if (cart == null) return new CartDto { UserId = GetUserId(), Items = new List<CartEntryDto>() };

                var output = (CartDto)cart;
                if (filter != null)
                {
                    output.Items = output.Items.Where(x =>
                                                x.ItemName.Contains(filter, StringComparison.OrdinalIgnoreCase)
                                                || x.Quantity.ToString() == filter).ToList();
                }

                foreach (var item in output.Items)
                {
                    var price = await Task.Run(() => (_dbContext.Items.SingleOrDefault(x => x.Id == item.ItemId))?.UnitPrice);
                    if (price == null) continue;
                    item.UnitPrice = price;
                }

                return output;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error: {Error}", JsonSerializer.Serialize(ex));
                return ex;
            }
        }

        private void BuildFilters(Filter filter, ExpressionStarter<CartEntry> cartEntryFilter, 
            ExpressionStarter<Cart> cartFilter)
        {
            if (!string.IsNullOrWhiteSpace(filter.PhoneNumber))
            {
                cartFilter.And(a => a.User.PhoneNumber == filter.PhoneNumber);
            }
            
            if (!string.IsNullOrWhiteSpace(filter.Time))
            {
                var isTime = DateTime.TryParse(filter.Time, out var time);
                if (isTime) cartEntryFilter.And(x => x.Audit!.UpdatedAt > time);
            }
            
            if (!string.IsNullOrWhiteSpace(filter.Quantity))
            {
                var isNumber = int.TryParse(filter.Quantity, out var quantity);
                if (isNumber) cartEntryFilter.And(x => x.Quantity == quantity);
            }

            if (!string.IsNullOrWhiteSpace(filter.ItemName))
            {
                cartEntryFilter.And(x => x.ItemName.Contains(filter.ItemName, StringComparison.OrdinalIgnoreCase));
            }
        }

        public class Filter
        {
            public Filter(string? phoneNumber, string? time, string? quantity, string? itemName)
            {
                PhoneNumber = phoneNumber;
                Time = time;
                Quantity = quantity;
                ItemName = itemName;
            }

            public string? PhoneNumber { get; set; }
            public string? Time { get; set; }
            public string? Quantity { get; set; }
            public string? ItemName { get; set; }
        }

        private int GetUserId()
        {
            var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty;
            return Convert.ToInt32(userId);
        }

        private async Task<OneOf<int, Exception>> UpsertCartItem(Cart cart, IEnumerable<CartEntry> cartEntries, bool isUpdate = false)
        {
            cart.AddToCart(cartEntries);

            foreach (var item in cart.CartEntries)
            {
                var itemInDb = await _dbContext.Items.SingleOrDefaultAsync(x => x.Id == item.ItemId);
                var numberOfItemsInStock = itemInDb.QuantityAvailable;
                if (item.Quantity > numberOfItemsInStock) return new InvalidQuantityException(item.ItemName);
            }

            if (isUpdate) await Task.Run(() => _dbContext.Carts.Update(cart));
            else await _dbContext.Carts.AddAsync(cart);
            await _dbContext.SaveChangesAsync();
            return cart.Id;
        }
    }
}
