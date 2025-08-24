using GerenciamentoDeTarefas.src.Application.Interfaces;
using GerenciamentoDeTarefas.src.Domain.Entities;
using DomainTaskStatus = GerenciamentoDeTarefas.src.Domain.Entities.TaskStatus;

namespace GerenciamentoDeTarefas.src.Infra.Data
{
    public static class DatabaseSeeder
    {
        public static async Task SeedAsync(AppDbContext context, IPasswordHasher passwordHasher)
        {
            // Check if database already has data
            if (context.Users.Any())
            {
                return;
            }

            // Create admin user
            var adminId = Guid.NewGuid();
            var adminUser = new User
            {
                Id = adminId,
                Name = "Administrator",
                Email = "admin@taskmanager.com",
                PasswordHash = passwordHasher.Hash("Admin123!"),
                Role = UserRole.Admin
            };

            // Create regular users
            var user1Id = Guid.NewGuid();
            var user1 = new User
            {
                Id = user1Id,
                Name = "João Silva",
                Email = "joao.silva@taskmanager.com",
                PasswordHash = passwordHasher.Hash("User123!"),
                Role = UserRole.User
            };

            var user2Id = Guid.NewGuid();
            var user2 = new User
            {
                Id = user2Id,
                Name = "Maria Santos",
                Email = "maria.santos@taskmanager.com",
                PasswordHash = passwordHasher.Hash("User123!"),
                Role = UserRole.User
            };

            var user3Id = Guid.NewGuid();
            var user3 = new User
            {
                Id = user3Id,
                Name = "Pedro Oliveira",
                Email = "pedro.oliveira@taskmanager.com",
                PasswordHash = passwordHasher.Hash("User123!"),
                Role = UserRole.User
            };

            // Add users to context
            context.Users.AddRange(adminUser, user1, user2, user3);

            // Create sample tasks
            var tasks = new List<TaskItem>
            {
                new TaskItem
                {
                    Id = Guid.NewGuid(),
                    Title = "Implementar autenticação JWT",
                    Description = "Desenvolver sistema de autenticação usando JWT tokens para a API",
                    ResponsibleId = user1Id,
                    Status = DomainTaskStatus.Completed,
                    CreatedAt = DateTime.UtcNow.AddDays(-10)
                },
                new TaskItem
                {
                    Id = Guid.NewGuid(),
                    Title = "Criar documentação da API",
                    Description = "Documentar todos os endpoints da API usando Swagger/OpenAPI",
                    ResponsibleId = user2Id,
                    Status = DomainTaskStatus.InProgress,
                    CreatedAt = DateTime.UtcNow.AddDays(-8)
                },
                new TaskItem
                {
                    Id = Guid.NewGuid(),
                    Title = "Implementar testes unitários",
                    Description = "Criar testes unitários para todos os serviços e controladores",
                    ResponsibleId = user1Id,
                    Status = DomainTaskStatus.Pending,
                    CreatedAt = DateTime.UtcNow.AddDays(-6)
                },
                new TaskItem
                {
                    Id = Guid.NewGuid(),
                    Title = "Configurar CI/CD",
                    Description = "Configurar pipeline de integração e deploy contínuo",
                    ResponsibleId = user3Id,
                    Status = DomainTaskStatus.Pending,
                    CreatedAt = DateTime.UtcNow.AddDays(-5)
                },
                new TaskItem
                {
                    Id = Guid.NewGuid(),
                    Title = "Implementar validações de entrada",
                    Description = "Adicionar validações para todos os DTOs de entrada da API",
                    ResponsibleId = user2Id,
                    Status = DomainTaskStatus.InProgress,
                    CreatedAt = DateTime.UtcNow.AddDays(-4)
                },
                new TaskItem
                {
                    Id = Guid.NewGuid(),
                    Title = "Otimizar queries do banco",
                    Description = "Analisar e otimizar as consultas mais frequentes do sistema",
                    ResponsibleId = user1Id,
                    Status = DomainTaskStatus.Pending,
                    CreatedAt = DateTime.UtcNow.AddDays(-3)
                },
                new TaskItem
                {
                    Id = Guid.NewGuid(),
                    Title = "Implementar logging",
                    Description = "Adicionar sistema de logs estruturado usando Serilog",
                    ResponsibleId = user3Id,
                    Status = DomainTaskStatus.Completed,
                    CreatedAt = DateTime.UtcNow.AddDays(-2)
                },
                new TaskItem
                {
                    Id = Guid.NewGuid(),
                    Title = "Configurar monitoramento",
                    Description = "Implementar monitoramento de performance e health checks",
                    ResponsibleId = user2Id,
                    Status = DomainTaskStatus.Pending,
                    CreatedAt = DateTime.UtcNow.AddDays(-1)
                }
            };

            context.Tasks.AddRange(tasks);

            await context.SaveChangesAsync();
        }
    }
}