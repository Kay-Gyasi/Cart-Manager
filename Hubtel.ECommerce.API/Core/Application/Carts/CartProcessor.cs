using Hubtel.ECommerce.API.Core.Domain.Entities;
using Hubtel.ECommerce.API.Infrastructure.Persistence;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace Hubtel.ECommerce.API.Core.Application.Carts
{
    public class CartProcessor
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<CartProcessor> _logger;
        private readonly AppDbContext _dbContext;

        // TODO: Input validation
        // TODO: Exception handling
        // TODO: Auto migrations
        public CartProcessor(IHttpContextAccessor httpContextAccessor, ILogger<CartProcessor> logger,
            AppDbContext dbContext)
        {
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
            _dbContext = dbContext;
        }

        public async Task<int?> AddToCart(CartCommand command)
        {
            try
            {
                var userId = GetUserId();
                if (userId == null)
                {
                    _logger.LogError("User Id is null");
                    return null;
                }

                var cartEntries = command.CartEntries.Select(x => CartEntry.Create(x.ItemId, x.Quantity));
                var cart = _dbContext.Carts.SingleOrDefault(x => x.UserId == userId);
                if (cart != null)
                {
                    cart.AddToCart(cartEntries);
                    await Task.Run(() => _dbContext.Carts.Update(cart));
                    await _dbContext.SaveChangesAsync();
                    return cart.Id;
                }

                cart ??= Cart.Create(userId ?? 0);
                cart.AddToCart(cartEntries);
                await _dbContext.AddAsync(cart);
                await _dbContext.SaveChangesAsync();
                return cart.Id;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error: {Error}", JsonSerializer.Serialize(ex));
                throw;
            }
        }

        public async Task<int?> RemoveFromCart(int itemId)
        {
            var userId = GetUserId();
            if (userId == null)
            {
                _logger.LogError("User Id is null");
                return null;
            }

            var cart = _dbContext.Carts.SingleOrDefault(x => x.UserId == userId);
            if (cart == null)
            {
                cart = Cart.Create(userId ?? 0);
                await _dbContext.SaveChangesAsync();
                return cart.Id;
            }

            cart.RemoveFromCart(itemId);
            await _dbContext.SaveChangesAsync();
            return cart.Id;
        }

        public async Task<CartDto?> GetCart()
        {
            var userId = GetUserId();
            if (userId == null)
            {
                _logger.LogError("User Id is null");
                return null;
            }

            return new CartDto();
        }

        private int? GetUserId()
        {
            var userId = _httpContextAccessor.HttpContext.User.FindFirst(JwtRegisteredClaimNames.NameId)?.Value;
            return Convert.ToInt32(userId);
        }
    }
}
