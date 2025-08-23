using GerenciamentoDeTarefas.src.Application.DTOs;
using GerenciamentoDeTarefas.src.Application.Interfaces;

namespace GerenciamentoDeTarefas.src.Application.UseCases.Users
{
    public class EditUserUseCase
    {
        private readonly IUserService _userService;
        public EditUserUseCase(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<UserDto> ExecuteAsync(Guid userId, EditUserRequest request)
        {
            return await _userService.EditUserAsync(userId, request);
        }
    }
}
