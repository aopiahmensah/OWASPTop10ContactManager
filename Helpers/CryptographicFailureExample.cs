using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using OWASPTaskManager.Models;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;


//Vulnerable Code: Weak Encryption
//This code demonstrates a cryptographic failure by using a weak encryption algorithm (e.g., DES) and hardcoding the encryption key.
//Uses DES, which is considered weak and outdated.
//Hardcodes the encryption key, making it easy to compromise.
//Uses a predictable IV (same as the key), which is not secure.
public class CryptographicFailureExample
{
    
    private static readonly string hardcodedKey = "12345678"; // DES key must be 8 bytes long

    public static string Encrypt(string plainText)
    {
        using (var des = DES.Create())
        {
            des.Key = Encoding.UTF8.GetBytes(hardcodedKey);
            des.IV = Encoding.UTF8.GetBytes(hardcodedKey); // IV should be 8 bytes for DES

            var plainBytes = Encoding.UTF8.GetBytes(plainText);
            using (var ms = new MemoryStream())
            using (var cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write))
            {
                cs.Write(plainBytes, 0, plainBytes.Length);
                cs.FlushFinalBlock();
                return Convert.ToBase64String(ms.ToArray());
            }
        }
    }

    public static string Decrypt(string encryptedText)
    {
        using (var des = DES.Create())
        {
            des.Key = Encoding.UTF8.GetBytes(hardcodedKey);
            des.IV = Encoding.UTF8.GetBytes(hardcodedKey); // IV should be 8 bytes for DES

            var encryptedBytes = Convert.FromBase64String(encryptedText);
            using (var ms = new MemoryStream())
            using (var cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write))
            {
                cs.Write(encryptedBytes, 0, encryptedBytes.Length);
                cs.FlushFinalBlock();
                return Encoding.UTF8.GetString(ms.ToArray());
            }
        }
    }
}
