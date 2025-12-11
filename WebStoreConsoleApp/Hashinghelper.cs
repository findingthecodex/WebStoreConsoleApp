using System.Security.Cryptography;

namespace WebStoreConsoleApp;

public class Hashinghelper
{
    public static string Generatesalt(int size = 16)
    {
        var saltBytes = RandomNumberGenerator.GetBytes(size);
        return Convert.ToBase64String(saltBytes);
    }
    
    /// <summary>
    ///  Generates a cryptographically secure random salt.
    /// </summary>
    /// <param name="value">Entered value</param>
    /// <param name="base64Salt">The slt</param>
    /// <param name="interations">Amount of runs/param>
    /// <param name="hasLength">Length of hash</param>
    /// <returns></returns>
    public static string HasWithSalt(string value, string base64Salt, int interations = 100_000, int hasLength = 32)
    {
        var saltBytes = Convert.FromBase64String(base64Salt);
        using var pbkdf2 = new Rfc2898DeriveBytes(
            password: value,
            salt: saltBytes,
            iterations: interations,
            hashAlgorithm: HashAlgorithmName.SHA256);

        var hash = pbkdf2.GetBytes(hasLength);
        return Convert.ToBase64String(hash);
    }

    /// <summary>
    /// Verifies a string against a hash and salt
    /// </summary>
    /// <param name="value">String to verify</param>
    /// <param name="base64Salt">The salt used to create the hash</param>
    /// <param name="expectedBase64Hash">Hash as Bae64 string</param>
    /// <returns>If computed hash matches it will return with true otherwise false</returns>
    public static bool verify(string value, string base64Salt, string expectedBase64Hash)
    {
        var computedHash = HasWithSalt(value, base64Salt);
        return CryptographicOperations.FixedTimeEquals(
            Convert.FromBase64String(expectedBase64Hash),
            Convert.FromBase64String(computedHash));
    }
}