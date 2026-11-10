using MediatR;
using SampleApp.Application.Interfaces;
using SampleApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SampleApp.Application.Features.Auth.Queries
{
    public class LoginQueryHandler : IRequestHandler<LoginQuery, string>
    {
        private readonly IJwtProvider _jwtProvider;
        private readonly IUserRepository _userRepository;

        public LoginQueryHandler(IJwtProvider jwtProvider, IUserRepository userRepository)
        {
            _jwtProvider = jwtProvider;
            _userRepository = userRepository;
        }

        public async Task<string?> Handle(LoginQuery request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetUserByEmailAsync(request.Email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            {
                return null;
            }

            var token =  _jwtProvider.GenerateToken(user.Id, user.Email, user.Role);
            return token;
        }
    }
}
