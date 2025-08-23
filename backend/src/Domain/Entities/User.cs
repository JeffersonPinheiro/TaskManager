namespace GerenciamentoDeTarefas.src.Domain.Entities
{
    public class User
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string PasswordHash { get; set; } = null!;
        public UserRole Role { get; set; }
    }

    public enum UserRole
    {
        Admin,
        User
    }
}
