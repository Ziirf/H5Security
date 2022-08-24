using System;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.Xml;
using System.Text;
using H5Security.Services.Interfaces;

namespace H5Security.Services
{
    public class Encryption : IEncryption
    {
        private const int SALT_SIZE = 10;
        private const int HASH_SIZE = 24;

        private readonly Random _random = new Random();
        private readonly UnicodeEncoding _encoder = new UnicodeEncoding();

        private readonly string _privateKey;
        private readonly string _publicKey;

        public Encryption()
        {
            var rsa = new RSACryptoServiceProvider(2048);
            _privateKey = rsa.ToXmlString(true);
            _publicKey = rsa.ToXmlString(false);
        }

        public byte[] GenerateSalt()
        {
            RNGCryptoServiceProvider provider = new RNGCryptoServiceProvider();
            byte[] salt = new byte[SALT_SIZE];
            provider.GetBytes(salt);

            return salt;
        }

        public string Hash(string input, byte[] salt, int Iterations)
        {
            Rfc2898DeriveBytes pbkdf2 = new Rfc2898DeriveBytes(input, salt, Iterations);

            return Convert.ToBase64String(pbkdf2.GetBytes(HASH_SIZE));
        }

        public string HashSha256(string input, byte[] salt)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(input + Convert.ToBase64String(salt));
            SHA256Managed sHA256ManagedString = new SHA256Managed();
            byte[] hash = sHA256ManagedString.ComputeHash(bytes);

            return Convert.ToBase64String(hash);
        }

        public int GetIteration()
        {
            return _random.Next(10000, 99999);
        }

        public string Encrypt(string message)
        {
            byte[] encryptedByteArray;
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
            {
                rsa.FromXmlString(_publicKey);
                var dataToEncrypt = _encoder.GetBytes(message);
                encryptedByteArray = rsa.Encrypt(dataToEncrypt, false).ToArray();
            }

            return Convert.ToBase64String(encryptedByteArray);
        }

        public string Decrypt(string message)
        {
            byte[] decryptedByteArray;
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
            {
                rsa.FromXmlString(_privateKey);
                decryptedByteArray = rsa.Decrypt(Convert.FromBase64String(message), false);
            }

            return _encoder.GetString(decryptedByteArray);
        }
    }
}
