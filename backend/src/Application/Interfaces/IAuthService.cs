using GerenciamentoDeTarefas.src.Application.DTOs;

namespace GerenciamentoDeTarefas.src.Application.Interfaces
{
    public interface IAuthService
    {
        Task<LoginResponse?> LoginAsync(GerenciamentoDeTarefas.src.Application.DTOs.LoginRequest request);
        Task<LoginResponse?> RefreshTokenAsync(RefreshTokenRequest request);
    }
}
