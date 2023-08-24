namespace Hubtel.ECommerce.API.Core.Domain.Entities
{
    public class User : Entity
    {
        private User(string name, string phoneNumber)
        {
            Name = name;
            PhoneNumber = phoneNumber;
        }

        private User(int id, string name, string phoneNumber)
        {
            Id = id;
            Name = name;
            PhoneNumber = phoneNumber;
        }

        public string Name { get; private set; }
        public string PhoneNumber { get; private set; }
        public int? CartId { get; private set; }
        public Cart? Cart { get; private set; }

        public static User Create(string name, string phoneNumber)
        {
            return new User(name, phoneNumber);
        }

        public static User Create(int id, string name, string phoneNumber)
        {
            return new User(id, name, phoneNumber);
        }
    }
}
