using System.Security.Cryptography;
using System.Text;

namespace CentroEventos.Aplicacion.Helpers;

public static class HashHelper
{
    private const int SaltSize = 16; // 128 bit 
    private const int KeySize = 32; // 256 bit
    private const int Iterations = 100000; // NIST recommends at least 10k
    private static readonly HashAlgorithmName _algorithm = HashAlgorithmName.SHA256;

    public static string CalcularHash(string textoPlano)
    {
        byte[] salt = RandomNumberGenerator.GetBytes(SaltSize);
        byte[] hash = Rfc2898DeriveBytes.Pbkdf2(
            Encoding.UTF8.GetBytes(textoPlano),
            salt,
            Iterations,
            _algorithm,
            KeySize
        );

        return $"{Convert.ToHexString(salt)}.{Convert.ToHexString(hash)}";
    }

    public static bool VerificarPassword(string password, string hashAlmacenado)
    {
        var parts = hashAlmacenado.Split('.');
        if (parts.Length != 2) return false;

        var salt = Convert.FromHexString(parts[0]);
        var hash = Convert.FromHexString(parts[1]);

        var hashInput = Rfc2898DeriveBytes.Pbkdf2(
            Encoding.UTF8.GetBytes(password),
            salt,
            Iterations,
            _algorithm,
            KeySize
        );

        return CryptographicOperations.FixedTimeEquals(hash, hashInput);
    }
}