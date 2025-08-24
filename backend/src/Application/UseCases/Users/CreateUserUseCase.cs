using GerenciamentoDeTarefas.src.Application.DTOs;
using GerenciamentoDeTarefas.src.Application.Interfaces;

namespace GerenciamentoDeTarefas.src.Application.UseCases.Users
{
    public class CreateUserUseCase
    {
        private readonly IUserService _userService;
        public CreateUserUseCase(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<UserDto> ExecuteAsync(CreateUserRequest request)
        {
            return await _userService.CreateUserAsync(request);
        }
    }
}
