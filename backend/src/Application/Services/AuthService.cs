using GerenciamentoDeTarefas.src.Application.DTOs;
using GerenciamentoDeTarefas.src.Application.Interfaces;
using GerenciamentoDeTarefas.src.Domain.Entities;
using GerenciamentoDeTarefas.src.Domain.Interfaces;
using Microsoft.AspNet.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace GerenciamentoDeTarefas.src.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly Interfaces.IPasswordHasher _passwordHasher;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IConfiguration _config;

        public AuthService(
            IUserRepository userRepository,
            Interfaces.IPasswordHasher passwordHasher,
            IRefreshTokenRepository refreshTokenRepository,
            IConfiguration config)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _refreshTokenRepository = refreshTokenRepository;
            _config = config;
        }

        public async Task<LoginResponse?> LoginAsync(GerenciamentoDeTarefas.src.Application.DTOs.LoginRequest request)
        {
            var user = await _userRepository.GetByEmailAsync(request.Email);
            if (user == null || !_passwordHasher.Verify(user.PasswordHash, request.Password))
                return null;

            var token = GenerateJwtToken(user);
            var refreshToken = GenerateRefreshToken();

            await _refreshTokenRepository.SaveOrUpdateAsync(user.Id, refreshToken);

            return new LoginResponse
            {
                AccessToken = token,
                RefreshToken = refreshToken,
                User = new UserDto
                {
                    Id = user.Id,
                    Name = user.Name,
                    Email = user.Email,
                    Role = user.Role.ToString()
                }
            };
        }

        public async Task<LoginResponse?> RefreshTokenAsync(RefreshTokenRequest request)
        {
            // 1. Valida o access token expirado, mas obtém o UserId/Claims
            var principal = GetPrincipalFromExpiredToken(request.AccessToken);
            if (principal == null)
                return null;

            var userIdString = principal.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userIdString == null || !Guid.TryParse(userIdString, out var userId))
                return null;

            // 2. Confirma se refresh token está correto e válido no banco
            var storedRefreshToken = await _refreshTokenRepository.GetByUserIdAsync(userId);
            if (storedRefreshToken == null || storedRefreshToken != request.RefreshToken)
                return null;

            // 3. Gera novo token e novo refresh token
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
                return null;

            var newAccessToken = GenerateJwtToken(user);
            var newRefreshToken = GenerateRefreshToken();
            await _refreshTokenRepository.SaveOrUpdateAsync(userId, newRefreshToken);

            return new LoginResponse
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken,
                User = new UserDto
                {
                    Id = user.Id,
                    Name = user.Name,
                    Email = user.Email,
                    Role = user.Role.ToString()
                }
            };
        }

        private string GenerateJwtToken(User user)
        {
            var key = Encoding.UTF8.GetBytes(_config["Jwt:SecretKey"]!);
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role.ToString())
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(30),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Issuer = _config["Jwt:Issuer"],
                Audience = _config["Jwt:Audience"]
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        private string GenerateRefreshToken()
        {
            var randomBytes = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomBytes);
            return Convert.ToBase64String(randomBytes);
        }

        private ClaimsPrincipal? GetPrincipalFromExpiredToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = true,
                ValidateIssuer = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:SecretKey"]!)),
                ValidateLifetime = false, // Ignora expiração
                ValidIssuer = _config["Jwt:Issuer"],
                ValidAudience = _config["Jwt:Audience"]
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var securityToken);
                if (securityToken is JwtSecurityToken jwtSecurityToken &&
                    jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                {
                    return principal;
                }
            }
            catch
            {
                return null;
            }
            return null;
        }
    }
}
