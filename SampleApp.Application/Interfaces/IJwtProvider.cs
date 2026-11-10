using System;
using System.Collections.Generic;
using System.Text;

namespace SampleApp.Application.Interfaces
{
    public interface IJwtProvider
    {
         string GenerateToken(int userId, string email, string role);
    }
}
