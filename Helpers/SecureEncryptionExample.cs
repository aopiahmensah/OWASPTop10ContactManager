using Microsoft.AspNetCore.DataProtection;
using OWASPTaskManager.Models;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;


//Secure Code: Strong Encryption
//This code demonstrates proper encryption practices by using a strong algorithm(AES) and generating keys securely.
//Uses AES, a strong and widely recommended encryption algorithm.
//Generates the encryption key and IV securely, ensuring they are not hardcoded or predictable.
//Properly handles the encryption and decryption processes with securely managed keys and IVs.
public class SecureEncryptionExample
{
    public static string Encrypt(string plainText, byte[] key, byte[] iv)
    {
        using (var aes = Aes.Create())
        {
            aes.Key = key;
            aes.IV = iv;

            var plainBytes = Encoding.UTF8.GetBytes(plainText);
            using (var ms = new MemoryStream())
            using (var cs = new CryptoStream(ms, aes.CreateEncryptor(), CryptoStreamMode.Write))
            {
                cs.Write(plainBytes, 0, plainBytes.Length);
                cs.FlushFinalBlock();
                return Convert.ToBase64String(ms.ToArray());
            }
        }
    }

    public static string Decrypt(string encryptedText, byte[] key, byte[] iv)
    {
        using (var aes = Aes.Create())
        {
            aes.Key = key;
            aes.IV = iv;

            var encryptedBytes = Convert.FromBase64String(encryptedText);
            using (var ms = new MemoryStream())
            using (var cs = new CryptoStream(ms, aes.CreateDecryptor(), CryptoStreamMode.Write))
            {
                cs.Write(encryptedBytes, 0, encryptedBytes.Length);
                cs.FlushFinalBlock();
                return Encoding.UTF8.GetString(ms.ToArray());
            }
        }
    }

    public static (byte[] key, byte[] iv) GenerateKeyAndIV()
    {
        using (var aes = Aes.Create())
        {
            aes.GenerateKey();
            aes.GenerateIV();
            return (aes.Key, aes.IV);
        }
    }
}
