using SampleApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SampleApp.Application.Interfaces
{
    public interface IUserRepository
    {
        Task<User> GetUserByEmailAsync(string email);
    }
}
