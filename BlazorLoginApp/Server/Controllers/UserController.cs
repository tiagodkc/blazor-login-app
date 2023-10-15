using BlazorLoginApp.Shared;
using Microsoft.AspNetCore.Mvc;

namespace BlazorLoginApp.Server.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class UserController : Controller
	{
		[HttpPost]
		[Route("login")]
		public async Task<IActionResult> Login([FromBody] UserDTO request)
		{
			SessionDTO sessionDTO = new SessionDTO();

			if (!string.IsNullOrWhiteSpace(request.Name) &&
				request.Name.ToLower() == "admin" &&
				!string.IsNullOrWhiteSpace(request.Password) &&
				request.Password == "123")
			{
				sessionDTO.Name = request.Name;
				sessionDTO.Role = "Administrator";
			}
			else
			{
				sessionDTO.Name = "employee";
				sessionDTO.Role = "Employee";
			}

			return StatusCode(StatusCodes.Status200OK, sessionDTO);
		}
	}
}
