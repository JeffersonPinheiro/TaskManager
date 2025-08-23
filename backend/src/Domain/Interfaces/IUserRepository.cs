using GerenciamentoDeTarefas.src.Domain.Entities;

namespace GerenciamentoDeTarefas.src.Domain.Interfaces
{
    public interface IUserRepository
    {
        Task AddAsync(User user);
        Task<User?> GetByIdAsync(Guid id);
        Task<User?> GetByEmailAsync(string email);
        Task<bool> ExistsByEmailAsync(string email);
        Task<List<User>> GetAllAsync();
        Task UpdateAsync(User user);
        Task DeleteAsync(Guid id);
    }
}
