using GerenciamentoDeTarefas.src.Application.DTOs;
using GerenciamentoDeTarefas.src.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GerenciamentoDeTarefas.src.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] GerenciamentoDeTarefas.src.Application.DTOs.LoginRequest request)
        {
            var result = await _authService.LoginAsync(request);
            if (result == null)
                return Unauthorized(new { message = "Credenciais inválidas" });
            return Ok(result);
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody] RefreshTokenRequest request)
        {
            var result = await _authService.RefreshTokenAsync(request);
            if (result == null)
                return Unauthorized(new { message = "Token inválido" });
            return Ok(result);
        }
    }
}
