using InstagramClone.Models;
using InstagramClone.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace InstagramClone.Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly JwtConfig _jwtSettings;
        public UserService(ApplicationDbContext dbContext, IOptions<JwtConfig> jwtSettings)
        {
            _dbContext = dbContext;
            _jwtSettings = jwtSettings.Value;
        }
       
        public User Register(string username, string password, string nickname)
        {
            var existingUser = _dbContext.Users.FirstOrDefault(u => u.Username == username);
            if (existingUser != null)
            {
                throw new ApplicationException("El usuario ya existe.");
            }
            if (password.Length < 6)
            {
                throw new ApplicationException("Por lo menos deben haber 6 caracteters.");
            }

            var hasher = new PasswordHasher<User>();
            var hashedPassword = hasher.HashPassword(null, password);

            var user = new User { Username = username, Password = hashedPassword, NickName = nickname  };
            _dbContext.Users.Add(user);
            _dbContext.SaveChanges();

            return user;
        }

        public string Authenticate(string username, string password)
        {
            var user = _dbContext.Users.SingleOrDefault(u => u.Username == username);

            if (user != null)
            {
                var hasher = new PasswordHasher<User>();
                var result = hasher.VerifyHashedPassword(null, user.Password, password);
                if (result == PasswordVerificationResult.Success)
                {
                    var token = GenerateJwtToken(user);
                    return token;
                }
            }
            throw new ApplicationException("Fallo auth.");
        }

        public string GenerateJwtToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString()),
                    new Claim(ClaimTypes.NameIdentifier, user.Username),
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public int? GetUserIdFromClaims(IEnumerable<Claim> claims)
        {
            var userIdClaim = claims.FirstOrDefault(c => c.Type == ClaimTypes.Name);
            if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int userId))
            {
                return userId;
            }

            return null;
        }

        public User GetUserById(int userId)
        {
            var user = _dbContext.Users.FirstOrDefault(u => u.Id == userId);
            return user;
        }
    }
}
