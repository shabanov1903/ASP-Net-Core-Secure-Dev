using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CardStorageService.Core.Crypto.Impl
{
    public class CryptoFatotySHA512 : ICrypto
    {
        private const string SecretKey = "jhvjhc7Jdfb87s7bhgdrewH";

        public (string passwordSalt, string passwordHash) CreatePasswordHash(string password)
        {
            // generate random salt
            byte[] buffer = new byte[16];
            RNGCryptoServiceProvider secureRandom = new();
            secureRandom.GetBytes(buffer);

            // create hash
            string passwordSalt = Convert.ToBase64String(buffer);
            string passwordHash = GetPasswordHash(password, passwordSalt);

            // done
            return (passwordSalt, passwordHash);
        }

        public bool VerifyPassword(string password, string passwordSalt, string passwordHash)
        {
            return GetPasswordHash(password, passwordSalt) == passwordHash;
        }

        public string GetPasswordHash(string password, string passwordSalt)
        {
            // build password string
            password = $"{password}~{passwordSalt}~{SecretKey}";
            byte[] buffer = Encoding.UTF8.GetBytes(password);

            // compute hash
            SHA512 sha512 = new SHA512Managed();
            byte[] passwordHash = sha512.ComputeHash(buffer);

            // done
            return Convert.ToBase64String(passwordHash);
        }
    }
}
