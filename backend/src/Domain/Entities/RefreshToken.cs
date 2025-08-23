namespace GerenciamentoDeTarefas.src.Domain.Entities
{
    public class RefreshToken
    {
        public Guid UserId { get; set; }
        public string Token { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public User User { get; set; } = null!;
    }
}
