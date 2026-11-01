using AmbientZero.Data.Interfaces;
using AmbientZero.Service.Constants;
using AmbientZero.Service.Interfaces;
using AmbientZero.Service.Models.Options;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AmbientZero.Service.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly Secrets _secrets;

        public AuthService(IOptions<Secrets> secrets,
            IUserRepository userRepository)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository)); 
            _secrets = secrets.Value ?? throw new ArgumentNullException(nameof(secrets));
        }

        public string? AuthUser(string username, string password)
        {
            PasswordHasher<string> passwordHasher = new PasswordHasher<string>();

            var user = _userRepository.GetUserByUsername(username);
            if (user is not null)
            {
                var passwordVerification = passwordHasher.VerifyHashedPassword(username, user.Password, password);

                if (passwordVerification == PasswordVerificationResult.Success || passwordVerification == PasswordVerificationResult.SuccessRehashNeeded)
                {
                    var claims = new List<Claim> { new Claim("UserId", user.Id.ToString()) };
                    var jwtToken = new JwtSecurityToken(claims: claims, notBefore: DateTime.UtcNow, expires: DateTime.UtcNow.AddHours(2),
                        signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secrets.TokenEncryptionKey ?? string.Empty)), SecurityAlgorithms.HmacSha256Signature));
                    return new JwtSecurityTokenHandler().WriteToken(jwtToken);
                }
                else
                {
                    return null;
                }
            }

            return null;

        }
    }
}
