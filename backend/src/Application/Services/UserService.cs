using GerenciamentoDeTarefas.src.Application.DTOs;
using GerenciamentoDeTarefas.src.Application.Interfaces;
using GerenciamentoDeTarefas.src.Domain.Entities;
using GerenciamentoDeTarefas.src.Domain.Interfaces;

namespace GerenciamentoDeTarefas.src.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepo;
        private readonly Interfaces.IPasswordHasher _passwordHasher;
        public UserService(IUserRepository userRepo, Interfaces.IPasswordHasher passwordHasher)
        {
            _userRepo = userRepo;
            _passwordHasher = passwordHasher;
        }

        public async Task<UserDto> CreateUserAsync(CreateUserRequest request)
        {
            // Validações  
            if (await _userRepo.ExistsByEmailAsync(request.Email))
                throw new CannotUnloadAppDomainException("E-mail já cadastrado.");

            var user = new User
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Email = request.Email,
                PasswordHash = _passwordHasher.Hash(request.Password),
                Role = Enum.Parse<UserRole>(request.Role)
            };
            await _userRepo.AddAsync(user);

            return new UserDto
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Role = user.Role.ToString()
            };
        }

        public async Task<UserDto> EditUserAsync(Guid userId, EditUserRequest request)
        {
            var user = await _userRepo.GetByIdAsync(userId);
            if (user == null)
                throw new Exception("Usuário não encontrado.");

            // Only update email if provided and different from current
            if (!string.IsNullOrWhiteSpace(request.Email) && request.Email != user.Email)
            {
                if (await _userRepo.ExistsByEmailAsync(request.Email))
                    throw new Exception("E-mail já cadastrado.");
                user.Email = request.Email;
            }

            // Only update name if provided
            if (!string.IsNullOrWhiteSpace(request.Name))
                user.Name = request.Name;

            // Only update password if provided
            if (!string.IsNullOrWhiteSpace(request.Password))
                user.PasswordHash = _passwordHasher.Hash(request.Password);

            // Only update role if provided
            if (!string.IsNullOrWhiteSpace(request.Role))
                user.Role = Enum.Parse<UserRole>(request.Role);

            await _userRepo.UpdateAsync(user);

            return new UserDto
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Role = user.Role.ToString()
            };
        }

        public async Task<UserDto?> GetUserByEmailAsync(string email)
        {
            var user = await _userRepo.GetByEmailAsync(email)
                ?? throw new CannotUnloadAppDomainException("E-mail não encontrado.");
            
            return new UserDto
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Role = user.Role.ToString()
            };            
        }

        public async Task<List<UserDto>> ListUsersAsync()
        {
            var users = await _userRepo.GetAllAsync();

            return users.Select(users => new UserDto
            {
                Id = users.Id,
                Name = users.Name,
                Email = users.Email,
                Role = users.Role.ToString()
            }).ToList();            
        }

    }
}
