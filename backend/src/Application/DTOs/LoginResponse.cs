namespace GerenciamentoDeTarefas.src.Application.DTOs
{
    public class LoginResponse
    {
        public string AccessToken { get; set; } = null!;
        public string RefreshToken { get; set; } = null!;
        public UserDto User { get; set; } = null!;
    }
}
