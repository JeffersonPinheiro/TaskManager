namespace GerenciamentoDeTarefas.src.Domain.Interfaces
{
    public interface IRefreshTokenRepository
    {
        Task SaveOrUpdateAsync(Guid userId, string refreshToken);
        Task<string?> GetByUserIdAsync(Guid userId);
        Task DeleteAsync(Guid userId);
    }
}
