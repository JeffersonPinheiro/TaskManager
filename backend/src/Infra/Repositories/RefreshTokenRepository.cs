using GerenciamentoDeTarefas.src.Domain.Entities;
using GerenciamentoDeTarefas.src.Domain.Interfaces;
using GerenciamentoDeTarefas.src.Infra.Data;
using Microsoft.EntityFrameworkCore;

namespace GerenciamentoDeTarefas.src.Infra.Repositories
{
    public class RefreshTokenRepository : IRefreshTokenRepository
    {
        private readonly AppDbContext _context;
        public RefreshTokenRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task SaveOrUpdateAsync(Guid userId, string refreshToken)
        {
            var existing = await _context.RefreshTokens
                .FirstOrDefaultAsync(rt => rt.UserId == userId);

            if (existing != null)
            {
                existing.Token = refreshToken;
                existing.UpdatedAt = DateTime.UtcNow;
                _context.RefreshTokens.Update(existing);
            }
            else
            {
                _context.RefreshTokens.Add(new RefreshToken
                {
                    UserId = userId,
                    Token = refreshToken,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                });
            }
            await _context.SaveChangesAsync();
        }

        public async Task<string?> GetByUserIdAsync(Guid userId)
        {
            var token = await _context.RefreshTokens
                .AsNoTracking()
                .FirstOrDefaultAsync(rt => rt.UserId == userId);
            return token?.Token;
        }

        public async Task DeleteAsync(Guid userId)
        {
            var token = await _context.RefreshTokens
                .FirstOrDefaultAsync(rt => rt.UserId == userId);
            if (token != null)
            {
                _context.RefreshTokens.Remove(token);
                await _context.SaveChangesAsync();
            }
        }
    }
}
