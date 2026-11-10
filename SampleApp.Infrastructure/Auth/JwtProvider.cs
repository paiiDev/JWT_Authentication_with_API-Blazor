using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SampleApp.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SampleApp.Infrastructure.Auth
{
    public class JwtProvider : IJwtProvider
    {
        private readonly IConfiguration _config;
        public JwtProvider(IConfiguration config)
        {
            _config = config;
        }

        public string GenerateToken(int userId, string email, string role)
        {
            var jwtSettingsSection = _config.GetSection("JwtSettings");
            var secretKey = jwtSettingsSection["SecretKey"]!;

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
               new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
               new Claim(JwtRegisteredClaimNames.Email, email),
                new Claim(ClaimTypes.Role, role),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                issuer: jwtSettingsSection["Issuer"],
                audience: jwtSettingsSection["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(double.Parse(jwtSettingsSection["ExpirationMinutes"]!)),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
