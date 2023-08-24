using FluentValidation;
using Hubtel.ECommerce.API.Core.Application.Carts;
using System.Collections.Generic;

namespace Hubtel.ECommerce.API.Core.Application.Carts
{
    public class CartCommand
    {
        public int UserId { get; set; }
        public List<CartEntryCommand> CartEntries { get; set; }
    }

    public class CartEntryCommand
    {
        public int ItemId { get; set; }
        public int? Quantity { get; set; }
    }
}

public class CartCommandValidator : AbstractValidator<CartCommand>
{
    public CartCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotNull()
            .NotEmpty()
            .NotEqual(0);
        RuleFor(x => x.CartEntries)
            .NotNull();
        RuleForEach(x => x.CartEntries)
            .Must(x => x.Quantity != null)
            .Must(x => x.ItemId > 0);
    }
}
