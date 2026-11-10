using System;
using System.Collections.Generic;
using System.Text;

namespace SampleApp.Domain.Entities
{
    public class User
    {
        public int Id { get; private set; }
        public string Email { get; private set; } = string.Empty;
        public string PasswordHash { get; private set; } = string.Empty;
        public string Role { get; private set; } = string.Empty;

        public User(string email, string passwordHash, string role)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Email is required.");

            if (string.IsNullOrWhiteSpace(passwordHash))
                throw new ArgumentException("Password hash is required.");

            if (string.IsNullOrWhiteSpace(role))
                throw new ArgumentException("Role is required.");

            Email = email;
            PasswordHash = passwordHash;
            Role = role;
        }

        public void UpdateEmail(string newEmail)
        {
            if (string.IsNullOrWhiteSpace(newEmail))
                throw new ArgumentException("Email is required.");

            Email = newEmail.Trim();
        }

        public void UpdatePassword(string newPasswordHash)
        {
            if (string.IsNullOrWhiteSpace(newPasswordHash))
                throw new ArgumentException("Password hash is required.");
            PasswordHash = newPasswordHash;
        }
       
    }
}
