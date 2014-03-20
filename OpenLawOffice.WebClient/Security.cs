namespace OpenLawOffice.WebClient
{
    using System;

    public class Security
    {
        public static string ClientHashPassword(string plainTextPassword)
        {
            return Hash(plainTextPassword);
        }

        public static string ServerHashPassword(string plainTextPassword, string salt)
        {
            return Hash(salt + plainTextPassword);
        }

        private static string Hash(string str)
        {
            System.Security.Cryptography.SHA512Managed sha512 = new System.Security.Cryptography.SHA512Managed();
            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(str);
            bytes = sha512.ComputeHash(bytes);
            return BitConverter.ToString(bytes).Replace("-", "");
        }
    }
}