using System;
using System.Security.Cryptography;

namespace KEKCore.Utils
{
    public class PasswordHash
    {
        public const int SaltByteSize = 24;
        public const int HashByteSize = 24;
        public const int Pbkdf2Iterations = 1000;

        public static string[] CreateHash(string password)
        {
            RNGCryptoServiceProvider csPrng = new RNGCryptoServiceProvider();
            byte[] salt = new byte[SaltByteSize];
            csPrng.GetBytes(salt);

            byte[] hash = PBKDF2(password, salt, Pbkdf2Iterations, HashByteSize);

            return new string[] { Convert.ToBase64String(hash),
                                  Convert.ToBase64String(salt),
                                  Pbkdf2Iterations.ToString() };
        }

        public static bool ValidatePassword(string password, string passHash, string _salt, string _iterations)
        {
            if (password == null) password = string.Empty;

            int iterations = int.Parse(_iterations);
            byte[] salt = Convert.FromBase64String(_salt);
            byte[] hash = Convert.FromBase64String(passHash);

            byte[] testHash = PBKDF2(password, salt, iterations, hash.Length);
            return SlowEquals(hash, testHash);
        }

        private static bool SlowEquals(byte[] a, byte[] b)
        {
            uint diff = (uint)a.Length ^ (uint)b.Length;
            for (int i = 0; i < a.Length && i < b.Length; i++)
                diff |= (uint)(a[i] ^ b[i]);
            return diff == 0;
        }

        private static byte[] PBKDF2(string password, byte[] salt, int iterations, int outputBytes)
        {           
            Rfc2898DeriveBytes pbkdf2 = new Rfc2898DeriveBytes(password, salt);
            pbkdf2.IterationCount = iterations;
            return pbkdf2.GetBytes(outputBytes);
        }
    }
}