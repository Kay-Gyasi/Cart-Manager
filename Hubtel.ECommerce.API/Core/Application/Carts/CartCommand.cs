using FluentValidation;
using System.Collections.Generic;

namespace Hubtel.ECommerce.API.Core.Application.Carts
{
    public class CartCommand
    {
        public List<CartEntryCommand> Items { get; set; }
    }

    public class CartEntryCommand
    {
        public int ItemId { get; set; }
        public int? Quantity { get; set; }
    }

    public class CartCommandValidator : AbstractValidator<CartCommand>
    {
        public CartCommandValidator()
        {
            RuleFor(x => x.Items)
                .NotNull();
            RuleForEach(x => x.Items)
                .Must(x => x.Quantity != null)
                .Must(x => x.ItemId > 0);
        }
    }
}