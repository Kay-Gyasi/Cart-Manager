using Hubtel.ECommerce.API.Core.Application.Jwt;
using Hubtel.ECommerce.API.Core.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Hubtel.ECommerce.API.Core.Application.Users
{
    public class UserProcessor
    {
        private readonly ITokenProvider _tokenProvider;
        private readonly UserManager<User> _userManager;
        private readonly ILogger<UserProcessor> _logger;

        public UserProcessor(ITokenProvider tokenProvider, UserManager<User> userManager, ILogger<UserProcessor> logger)
        {
            _tokenProvider = tokenProvider;
            _userManager = userManager;
            _logger = logger;
        }

        public async Task<AuthToken?> LoginAsync(LoginCommand command)
        {
            var user = await _userManager.FindByEmailAsync(command.Email);

            if (user is null || !await _userManager.CheckPasswordAsync(user, command.Password))
            {
                _logger.LogError("Invalid login attempt");
                return null;
            }

            return await Task.Run(() => _tokenProvider.GenerateToken(user));
        }

        public async Task<int?> CreateUser(UserCommand command)
        {
            var user = User.Create(command.Email, command.PhoneNumber)
                            .WithName(command.UserName ?? string.Empty);

            var result = await _userManager.CreateAsync(user, command.Password);
            if (result.Errors.Any())
            {
                _logger.LogError("{Errors}", result.Errors.ToString());
                return null;
            }

            return user.Id;
        }
    }
}
