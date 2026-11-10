using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JWT_Authentication_with_API_Blazor.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        [HttpGet("data")]
        public IActionResult GetDashboardData()
        {
            // This is a protected endpoint that requires authentication
            return Ok(new { message = "This is protected dashboard data." });
        }
    }
}
