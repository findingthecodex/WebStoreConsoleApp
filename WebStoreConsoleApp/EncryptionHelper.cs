namespace WebStoreConsoleApp;

/// <summary>
///  Helps encrypt and decrypt data
/// </summary>
public static class EncryptionHelper
{
    private const byte key = 0x42;

    /// <summary>
    /// Encrypts data
    /// </summary>
    public static string Encrypt(string text)
    {
        if (string.IsNullOrEmpty(text))
            return text;

        var bytes = System.Text.Encoding.UTF8.GetBytes(text);

        for (int i = 0; i < bytes.Length; i++)
            bytes[i] = (byte)(bytes[i] ^ key);

        return Convert.ToBase64String(bytes);
    }

    /// <summary>
    /// Decrypts data
    /// </summary>
    public static string Decrypt(string encryptedText)
    {
        if (string.IsNullOrEmpty(encryptedText))
            return encryptedText;

        var bytes = Convert.FromBase64String(encryptedText);

        for (int i = 0; i < bytes.Length; i++)
            bytes[i] = (byte)(bytes[i] ^ key);

        return System.Text.Encoding.UTF8.GetString(bytes);
    }
}