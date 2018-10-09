using System;
using System.Security.Cryptography;
using System.Text;

namespace IRunesServices
{
    public class HashService : IHashService
    {
        public string Hash(string textToHash)
        {
            textToHash = textToHash + "sALt1818#_mYaPP";

            using (var sha256 = SHA256.Create())
            {
                // Send a sample text to hash.  
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(textToHash));
                // Get the hashed string.  
                var hash = BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
                return hash;
            }
        }
    }
}
