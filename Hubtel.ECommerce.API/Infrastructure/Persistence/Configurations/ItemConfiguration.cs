﻿using Hubtel.ECommerce.API.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hubtel.ECommerce.API.Infrastructure.Persistence.Configurations
{
    public class ItemConfiguration : DatabaseConfiguration<Item, int>
    {
    }
}
