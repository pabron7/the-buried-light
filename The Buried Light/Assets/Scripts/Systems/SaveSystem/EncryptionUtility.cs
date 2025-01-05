using System.Security.Cryptography;
using System.Text;
using System;
using UnityEngine;

public static class EncryptionUtility
{
    private static readonly string Key = "YourSecretKey123"; 
    public static bool BypassEncryption = true;

    public static string Encrypt(string plainText)
    {
        if (BypassEncryption)
        {
            Debug.Log("Encryption bypassed.");
            return plainText; // Return plain text directly
        }

        try
        {
            Debug.Log($"Encrypting: {plainText}");
            byte[] keyBytes = Encoding.UTF8.GetBytes(Key);
            byte[] plainBytes = Encoding.UTF8.GetBytes(plainText);

            using (var aes = Aes.Create())
            {
                aes.Key = keyBytes;
                aes.IV = new byte[16];
                using (var encryptor = aes.CreateEncryptor(aes.Key, aes.IV))
                {
                    byte[] encrypted = encryptor.TransformFinalBlock(plainBytes, 0, plainBytes.Length);
                    return Convert.ToBase64String(encrypted);
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Encryption failed: {e}");
            throw;
        }
    }

    public static string Decrypt(string encryptedText)
    {
        if (BypassEncryption)
        {
            Debug.Log("Decryption bypassed.");
            return encryptedText; // Return plain text directly
        }

        try
        {
            Debug.Log($"Decrypting: {encryptedText}");
            byte[] keyBytes = Encoding.UTF8.GetBytes(Key);
            byte[] encryptedBytes = Convert.FromBase64String(encryptedText);

            using (var aes = Aes.Create())
            {
                aes.Key = keyBytes;
                aes.IV = new byte[16];
                using (var decryptor = aes.CreateDecryptor(aes.Key, aes.IV))
                {
                    byte[] plainBytes = decryptor.TransformFinalBlock(encryptedBytes, 0, encryptedBytes.Length);
                    return Encoding.UTF8.GetString(plainBytes);
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Decryption failed: {e}");
            throw;
        }
    }
}

