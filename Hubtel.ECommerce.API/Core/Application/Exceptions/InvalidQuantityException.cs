using System;

namespace Hubtel.ECommerce.API.Core.Application.Exceptions
{
    public class InvalidQuantityException : Exception
    {
        public InvalidQuantityException(string item) : base($"Quantity of {item} specified is invalid")
        {
            
        }
    }
}
