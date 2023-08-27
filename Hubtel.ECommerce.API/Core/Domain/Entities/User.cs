using Microsoft.AspNetCore.Identity;
using System;

namespace Hubtel.ECommerce.API.Core.Domain.Entities
{
    public sealed class User : IdentityUser<int>
    {
        private User() {}
        private User(string email, string phone)
        {
            Email = email;
            PhoneNumber = phone;
        }

        public int? CartId { get; private set; }
        public Cart? Cart { get; private set; }

        public static User Create(string email, string phone) 
            => new User(email, phone);

        public User WithName(string userName)
        {
            UserName = userName;
            return this;
        }
    }
}
