using GerenciamentoDeTarefas.src.Application.DTOs;
using GerenciamentoDeTarefas.src.Application.UseCases.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GerenciamentoDeTarefas.src.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class UsersController : ControllerBase
    {
        private readonly CreateUserUseCase _createUser;
        private readonly EditUserUseCase _editUser;
        private readonly ListUsersUseCase _listUsers;

        public UsersController(
            CreateUserUseCase createUser,
            EditUserUseCase editUser,
            ListUsersUseCase listUsers
        )
        {
            _createUser = createUser;
            _editUser = editUser;
            _listUsers = listUsers;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateUserRequest request)
        {
            var user = await _createUser.ExecuteAsync(request);
            return CreatedAtAction(nameof(Get), new { id = user.Id }, user);
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> Edit(Guid id, [FromBody] EditUserRequest request)
        {
            var user = await _editUser.ExecuteAsync(id, request);
            return Ok(user);
        }

        [HttpGet]
        public async Task<IActionResult> List()
        {
            var users = await _listUsers.ExecuteAsync();
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var users = await _listUsers.ExecuteAsync();
            return NotFound();
        }
    }
}
