using Hubtel.ECommerce.API.Core.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using System.Linq;

namespace Hubtel.ECommerce.API.Core.Application.Jwt
{
    public class TokenProvider : ITokenProvider
    {
        private readonly string _issuer;
        private readonly SigningCredentials _jwtSigningCredentials;
        private readonly Claim[] _audiences;

        public TokenProvider(IConfiguration configuration, UserManager<User> userManager)
        {
            var bearerSection = configuration.GetSection("jwt");

            _issuer = bearerSection["ValidIssuer"] ?? throw new InvalidOperationException("Issuer is not specified");
            var signingKey = bearerSection["SigningKeys:0:Value"] ?? throw new InvalidOperationException("Signing key is not specified");

            var signingKeyBytes = Encoding.UTF8.GetBytes(signingKey);

            _jwtSigningCredentials = new SigningCredentials(new SymmetricSecurityKey(signingKeyBytes),
                                            SecurityAlgorithms.HmacSha256Signature);

            _audiences = bearerSection.GetSection("ValidAudiences").GetChildren()
                                        .Where(s => !string.IsNullOrEmpty(s.Value))
                                        .Select(s => new Claim(JwtRegisteredClaimNames.Aud, s.Value!))
                                        .ToArray();
        }


        public AuthToken GenerateToken(User user)
        {
            var identity = new ClaimsIdentity(JwtBearerDefaults.AuthenticationScheme);

            identity.AddClaims(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.MobilePhone, user.PhoneNumber ?? ""),
            });

            var id = Guid.NewGuid().ToString().GetHashCode().ToString("x", CultureInfo.InvariantCulture);
            identity.AddClaim(new Claim(JwtRegisteredClaimNames.Jti, id));

            identity.AddClaims(_audiences);

            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.CreateJwtSecurityToken(
                _issuer,
                audience: null,
                identity,
                notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow.AddMinutes(30),
                issuedAt: DateTime.UtcNow,
                _jwtSigningCredentials);

            return new AuthToken(handler.WriteToken(jwtToken));
        }
    }

    public interface ITokenProvider
    {
        AuthToken GenerateToken(User user);
    }

    public class AuthToken
    {
        public AuthToken(string token)
        {
            Token = token;
        }
        public string Token { get; set; }
    };
}
