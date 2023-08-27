using System;

namespace Hubtel.ECommerce.API.Core.Application.Users
{
    public class UserCommand
    {
        public string? UserName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string? Password { get; set; }
    }
}
