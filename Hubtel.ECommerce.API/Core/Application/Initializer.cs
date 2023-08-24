using Hubtel.ECommerce.API.Core.Domain.Entities;
using Hubtel.ECommerce.API.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System;
using System.Text.Json;

namespace Hubtel.ECommerce.API.Core.Application
{
    public class Initializer
    {
        private readonly AppDbContext _dbContext;
        private readonly ILogger<Initializer> _logger;

        public Initializer(AppDbContext dbContext, ILogger<Initializer> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task Initialize()
        {
            try
            {
                await AddItems();
                await AddUsers();
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError("Error: {Error}", JsonSerializer.Serialize(ex));
                throw;
            }
        }

        private async Task AddItems()
        {
            var isInitialized = await _dbContext.Items.AnyAsync();
            if (isInitialized) return;

            var seedData = new Item[]
            {
                Item.Create("Item 1", 120, 3.5m),
                Item.Create("Item 2", 50, 10m),
                Item.Create("Item 3", 70, 49.9m),
                Item.Create("Item 4", 15, 99.9m),
            };
            foreach (var item in seedData)
            {
                await _dbContext.Items.AddAsync(item);
            }
        }

        private async Task AddUsers()
        {
            var isInitialized = await _dbContext.Users.AnyAsync();
            if (isInitialized) return;

            var seedData = new User[]
            {
                User.Create("Kay Gyasi", "0557833216"),
                User.Create("Hubtel", "0557511677")
            };
            foreach (var user in seedData)
            {
                await _dbContext.Users.AddAsync(user);
            }
        }
    }
}
