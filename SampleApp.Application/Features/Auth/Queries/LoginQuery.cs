using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace SampleApp.Application.Features.Auth.Queries
{
    public class LoginQuery : IRequest<string>
    {
        public string Email { get; set; }
        public string Password { get; set; }

        public LoginQuery(string email, string password)
        {
            Email = email;
            Password = password;
        }
    }
}
