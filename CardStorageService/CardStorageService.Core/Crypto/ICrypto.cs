using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CardStorageService.Core.Crypto
{
    public interface ICrypto
    {
        public (string passwordSalt, string passwordHash) CreatePasswordHash(string password);
        public bool VerifyPassword(string password, string passwordSalt, string passwordHash);
        public string GetPasswordHash(string password, string passwordSalt);
    }
}
