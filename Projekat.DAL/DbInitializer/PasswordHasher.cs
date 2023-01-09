using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Projekat.DAL
{
    public static class PasswordHasher
    {
        public static byte[] GenerateSalt(int size = 64)
        {
            RNGCryptoServiceProvider generator = new RNGCryptoServiceProvider();
            byte[] salt = new byte[size];
            generator.GetBytes(salt);
            return salt;
        }

        public static byte[] GenerateSaltedHash(byte[] password, byte[] salt)
        {
            HashAlgorithm algorithm = new SHA256Managed();

            byte[] passwordWithSaltBytes = new byte[password.Length + salt.Length];

            for (int i = 0; i < password.Length; i++)
            {
                passwordWithSaltBytes[i] = password[i];
            }

            for (int i = 0; i < salt.Length; i++)
            {
                passwordWithSaltBytes[password.Length + i] = salt[i];
            }

            return algorithm.ComputeHash(passwordWithSaltBytes);
        }
    }
}
