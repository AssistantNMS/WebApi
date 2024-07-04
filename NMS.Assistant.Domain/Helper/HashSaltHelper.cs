using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace NMS.Assistant.Domain.Helper
{
    public static class HashSaltHelper
    {
        public static string GetHashString(string inputString, string salt)
        {
            StringBuilder sb = new StringBuilder();
            foreach (byte b in GenerateSaltedHash(inputString, salt))
            {
                sb.Append(b.ToString("X2"));
            }

            return sb.ToString();
        }
        public static byte[] GenerateSaltedHash(string plainText, string salt)
        {
            byte[] plainBytes = Encoding.UTF8.GetBytes(plainText);
            byte[] saltBytes = Encoding.UTF8.GetBytes(salt);

            return GenerateSaltedHash(plainBytes, saltBytes);
        }

        public static byte[] GenerateSaltedHash(IReadOnlyList<byte> plainText, IReadOnlyList<byte> salt)
        {
            HashAlgorithm algorithm = MD5.Create();

            byte[] plainTextWithSaltBytes =
                new byte[plainText.Count + salt.Count];

            for (int i = 0; i < plainText.Count; i++)
            {
                plainTextWithSaltBytes[i] = plainText[i];
            }

            for (int i = 0; i < salt.Count; i++)
            {
                plainTextWithSaltBytes[plainText.Count + i] = salt[i];
            }

            return algorithm.ComputeHash(plainTextWithSaltBytes);
        }
    }
}
