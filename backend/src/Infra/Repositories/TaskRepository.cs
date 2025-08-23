using GerenciamentoDeTarefas.src.Application.DTOs;
using GerenciamentoDeTarefas.src.Domain.Entities;
using GerenciamentoDeTarefas.src.Domain.Interfaces;
using GerenciamentoDeTarefas.src.Infra.Data;
using Microsoft.EntityFrameworkCore;
using TaskStatus = GerenciamentoDeTarefas.src.Domain.Entities.TaskStatus;

namespace GerenciamentoDeTarefas.src.Infra.Repositories
{
    public class TaskRepository : ITaskRepository
    {
        private readonly AppDbContext _context;
        public TaskRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(TaskItem task)
        {
            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();
        }

        public async Task<TaskItem?> GetByIdAsync(Guid id)
        {
            return await _context.Tasks
                .Include(t => t.Responsible)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<List<TaskItem>> GetAllAsync()
        {
            return await _context.Tasks
                .Include(t => t.Responsible)
                .OrderByDescending(t => t.CreatedAt)
                .ToListAsync();
        }

        public async Task<List<TaskItem>> GetFilteredAsync(TaskFilter filter)
        {
            var query = _context.Tasks
                .Include(t => t.Responsible)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(filter.Title))
            {
                query = query.Where(t => t.Title.ToLower().Contains(filter.Title.ToLower()));
            }

            if (filter.ResponsibleId.HasValue)
            {
                query = query.Where(t => t.ResponsibleId == filter.ResponsibleId.Value);
            }

            if (!string.IsNullOrWhiteSpace(filter.Status))
            {
                if (Enum.TryParse<TaskStatus>(filter.Status, true, out var status))
                {
                    query = query.Where(t => t.Status == status);
                }
            }

            if (filter.CreatedFrom.HasValue)
            {
                query = query.Where(t => t.CreatedAt >= filter.CreatedFrom.Value);
            }

            if (filter.CreatedTo.HasValue)
            {
                var createdToEndOfDay = filter.CreatedTo.Value.Date.AddDays(1).AddTicks(-1);
                query = query.Where(t => t.CreatedAt <= createdToEndOfDay);
            }

            if (filter.PageSize.HasValue && filter.PageSize.Value > 0)
            {
                var page = filter.Page ?? 1;
                var skip = (page - 1) * filter.PageSize.Value;
                query = query.Skip(skip).Take(filter.PageSize.Value);
            }

            return await query
                .OrderByDescending(t => t.CreatedAt)
                .ToListAsync();
        }

        public async Task UpdateAsync(TaskItem task)
        {
            _context.Tasks.Update(task);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var task = await GetByIdAsync(id);
            if (task != null)
            {
                _context.Tasks.Remove(task);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<int> CountByStatusAsync(TaskStatus status)
        {
            return await _context.Tasks.CountAsync(t => t.Status == status);
        }

        public async Task<List<TaskItem>> GetLastCreatedAsync(int count)
        {
            return await _context.Tasks
                .Include(t => t.Responsible)
                .OrderByDescending(t => t.CreatedAt)
                .Take(count)
                .ToListAsync();
        }
    }
}
