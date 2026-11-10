using Microsoft.EntityFrameworkCore;
using SampleApp.Application.Interfaces;
using SampleApp.Domain.Entities;
using SampleApp.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace SampleApp.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;
        public UserRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<User?> GetUserByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }
    }
}
