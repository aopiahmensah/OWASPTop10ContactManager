using Humanizer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using OWASPTaskManager.Models;
using OWASPTop10TaskManager.Data;

namespace OWASPTaskManager.Controllers
{
    public class CryptographicFailuresController : Controller
    {
        //By comparing these two examples, developers can understand the importance of 
        //using strong encryption algorithms and proper key management to avoid cryptographic failures.
        private void EncryptText()
    {
        string originalText = "Sensitive Data";

        // Demonstrate vulnerable encryption
            string encryptedText = CryptographicFailureExample.Encrypt(originalText);
            string decryptedText = CryptographicFailureExample.Decrypt(encryptedText);
            Console.WriteLine($"Vulnerable Encrypted: {encryptedText}");
            Console.WriteLine($"Vulnerable Decrypted: {decryptedText}");

            // Demonstrate secure encryption
            var (key, iv) = SecureEncryptionExample.GenerateKeyAndIV();
            encryptedText = SecureEncryptionExample.Encrypt(originalText, key, iv);
            decryptedText = SecureEncryptionExample.Decrypt(encryptedText, key, iv);
            Console.WriteLine($"Secure Encrypted: {encryptedText}");
            Console.WriteLine($"Secure Decrypted: {decryptedText}");

        }

    }
}
