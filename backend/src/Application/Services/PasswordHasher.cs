using GerenciamentoDeTarefas.src.Application.Interfaces;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;

namespace GerenciamentoDeTarefas.src.Application.Services
{
    public class PasswordHasher : IPasswordHasher
    {
        private const int SaltSize = 16; // 128 bits
        private const int KeySize = 32;  // 256 bits
        private const int Iterations = 10000;

        public string Hash(string password)
        {
            // Gera salt aleatório
            byte[] salt = new byte[SaltSize];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            // Gera o hash usando PBKDF2
            byte[] key = KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: Iterations,
                numBytesRequested: KeySize);

            // Retorna hash no formato: {iterations}.{salt}.{key}
            return $"{Iterations}.{Convert.ToBase64String(salt)}.{Convert.ToBase64String(key)}";
        }

        public bool Verify(string hash, string password)
        {
            var parts = hash.Split('.');
            if (parts.Length != 3)
                return false;

            int iterations = int.Parse(parts[0]);
            byte[] salt = Convert.FromBase64String(parts[1]);
            byte[] key = Convert.FromBase64String(parts[2]);

            byte[] attemptKey = KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: iterations,
                numBytesRequested: key.Length);

            return CryptographicOperations.FixedTimeEquals(key, attemptKey);
        }
    }
}
