using Hubtel.ECommerce.API.Core.Domain.Entities;
using Hubtel.ECommerce.API.Infrastructure.Persistence;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using System.Threading.Tasks;
using System;

namespace Hubtel.ECommerce.API.Core.Application.Items
{
    public class ItemProcessor
    {
        private readonly AppDbContext _dbContext;
        private readonly ILogger<ItemProcessor> _logger;

        public ItemProcessor(AppDbContext dbContext, ILogger<ItemProcessor> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<int> AddAsync(ItemCommand command)
        {
            var isNew = command.Id == default;
            Item item;
            
            if (isNew)
            {
                item = Item.Create(command.Name, command.QuantityAvailable, command.UnitPrice);
                await UpsertItemToDb(item);
                return item.Id;
            }

            item = await _dbContext.Items.FindAsync(command.Id);
            item.WithName(command.Name)
                .HasQuantityAvailable(command.QuantityAvailable)
                .HasUnitPrice(command.UnitPrice);
            await UpsertItemToDb(item, isNew: false);
            return item.Id; 
        }

        public async Task<ItemDto?> GetAsync(int itemId)
        {
            var item = await _dbContext.Items.FindAsync(itemId);
            if (item == null) return null;
            return (ItemDto)item;
        }

        private async Task UpsertItemToDb(Item item, bool isNew = true)
        {
            try
            {
                if (isNew) await _dbContext.Items.AddAsync(item);
                else await Task.Run(() => _dbContext.Items.Update(item));
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError("Error: {Error}", JsonSerializer.Serialize(ex));
                throw;
            }
        }
    }
}
