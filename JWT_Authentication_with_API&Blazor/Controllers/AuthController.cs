using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SampleApp.Application.Features.Auth.Queries;
using SampleApp.Shared.DTOs.Auth;

namespace JWT_Authentication_with_API_Blazor.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ISender _sender;

        public AuthController(ISender sender)
        {
            _sender = sender;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto request)
        {
            var query = new LoginQuery(request.Email, request.Password);
            var token = await _sender.Send(query);
            if(token is null)
            {
                return Unauthorized(new {message = "Invalid email or password." });
            }
            return Ok(new { Token = token });
        }
    }
}
