using GerenciamentoDeTarefas.src.Application.DTOs;

namespace GerenciamentoDeTarefas.src.Application.Interfaces
{
    public interface IUserService
    {
        Task<UserDto> CreateUserAsync(CreateUserRequest request);
        Task<UserDto> EditUserAsync(Guid userId, EditUserRequest request);
        Task<List<UserDto>> ListUsersAsync();
        Task<UserDto?> GetUserByEmailAsync(string email);
    }
}
