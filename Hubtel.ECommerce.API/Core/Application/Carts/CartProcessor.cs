using Hubtel.ECommerce.API.Core.Domain.Entities;
using Hubtel.ECommerce.API.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace Hubtel.ECommerce.API.Core.Application.Carts
{
    public class CartProcessor
    {
        private readonly ILogger<CartProcessor> _logger;
        private readonly AppDbContext _dbContext;
        private IQueryable<Cart> Carts => _dbContext.Carts
                                            .Include(x => x.User)
                                            .Include(x => x.CartEntries)
                                            .ThenInclude(x => x.Item);

        public CartProcessor(ILogger<CartProcessor> logger,
            AppDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        public async Task<int?> AddToCart(CartCommand command)
        {
            try
            {
                var cartEntries = command.CartEntries.Select(x => CartEntry.Create(x.ItemId, x.Quantity ?? 0));
                var cart = Carts.SingleOrDefault(x => x.UserId == command.UserId);
                if (cart != null)
                {
                    cart.AddToCart(cartEntries);
                    await Task.Run(() => _dbContext.Carts.Update(cart));
                    await _dbContext.SaveChangesAsync();
                    return cart.Id;
                }

                cart ??= Cart.Create(command.UserId);
                cart.AddToCart(cartEntries);
                await _dbContext.Carts.AddAsync(cart);
                await _dbContext.SaveChangesAsync();
                return cart.Id;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error: {Error}", JsonSerializer.Serialize(ex));
                throw;
            }
        }

        public async Task RemoveFromCart(int itemId, int userId)
        {
            try
            {
                var cart = Carts.SingleOrDefault(x => x.UserId == userId);
                if (cart == null)
                {
                    cart = Cart.Create(userId);
                    await _dbContext.SaveChangesAsync();
                    return;
                }

                cart.RemoveFromCart(itemId);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError("Error: {Error}", JsonSerializer.Serialize(ex));
                throw;
            }
        }

        public async Task<CartDto?> GetCart(int userId, string? filter)
        {
            try
            {
                var cart = await Task.Run(() => Carts.SingleOrDefault(x => x.UserId == userId));
                if (cart == null) return new CartDto { UserId = userId, Items = new List<CartEntryDto>() };

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
                throw;
            }
        }
    }
}
