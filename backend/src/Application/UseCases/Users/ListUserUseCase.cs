using GerenciamentoDeTarefas.src.Application.DTOs;
using GerenciamentoDeTarefas.src.Application.Interfaces;

namespace GerenciamentoDeTarefas.src.Application.UseCases.Users
{
    public class ListUsersUseCase
    {
        private readonly IUserService _userService;
        public ListUsersUseCase(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<List<UserDto>> ExecuteAsync()
        {
            return await _userService.ListUsersAsync();
        }
    }
}
