using System;

namespace Hubtel.ECommerce.API.Core.Application.Exceptions
{
    public class InvalidLoginException : Exception
    {
        public InvalidLoginException() : base("Invalid login")
        {
            
        }
    }
}
