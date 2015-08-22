using System;
using System.IO;
using System.Security.Cryptography;

namespace OpenLawOffice.Common
{
    // Credit : https://msdn.microsoft.com/en-us/library/system.security.cryptography.aesmanaged(v=vs.110).aspx
    public class Encryption
    {
        private const int KEY_SIZE = 256;
        private const CipherMode CIPHER_MODE = CipherMode.CBC;
        private const PaddingMode PADDING_MODE = PaddingMode.PKCS7;
        private AesManaged _aesAlg;

        public string Key
        {
            get { return Convert.ToBase64String(_aesAlg.Key); }
            set { _aesAlg.Key = Convert.FromBase64String(value); }
        }

        public string IV
        {
            get { return Convert.ToBase64String(_aesAlg.IV); }
            set { _aesAlg.IV = Convert.FromBase64String(value); }
        }

        public Encryption()
        {
            _aesAlg = new AesManaged();
        }

        public Encryption(string iv, string key)
        {
            _aesAlg = new AesManaged();
            Key = key;
            IV = iv;
        }

        public Package Encrypt(Package package)
        {
            if (package == null)
                throw new ArgumentNullException("package");
            //if (string.IsNullOrEmpty(package.Input))
            //    throw new ArgumentNullException("package.Input");
            if (package.Key == null || package.Key.Length <= 0)
            {
                if (_aesAlg.Key != null && _aesAlg.Key.Length > 0)
                    package.Key = _aesAlg.Key;
                else
                    throw new ArgumentNullException("package.Key");
            }
            if (package.IV == null || package.IV.Length <= 0)
            {
                if (_aesAlg.IV != null && _aesAlg.IV.Length > 0)
                    package.IV = _aesAlg.IV;
                else
                    throw new ArgumentNullException("package.IV");
            }

            _aesAlg.Key = package.Key;
            _aesAlg.IV = package.IV;

            ICryptoTransform encryptor = _aesAlg.CreateEncryptor(_aesAlg.Key, _aesAlg.IV);

            using (MemoryStream msOutput = new MemoryStream())
            {
                using (CryptoStream cryptoStream = new CryptoStream(msOutput, encryptor, CryptoStreamMode.Write))
                {
                    using (StreamWriter sw = new StreamWriter(cryptoStream))
                    {
                        sw.Write(package.Input);
                    }

                    package.Output = Convert.ToBase64String(msOutput.ToArray(), Base64FormattingOptions.None);
                    return package;
                }
            }
        }

        public Package Decrypt(Package package)
        {
            if (package == null)
                throw new ArgumentNullException("package");
            if (string.IsNullOrEmpty(package.Input))
                throw new ArgumentNullException("package.Input");
            if (package.Key == null || package.Key.Length <= 0)
            {
                if (_aesAlg.Key != null && _aesAlg.Key.Length > 0)
                    package.Key = _aesAlg.Key;
                else
                    throw new ArgumentNullException("package.Key");
            }
            if (package.IV == null || package.IV.Length <= 0)
            {
                if (_aesAlg.IV != null && _aesAlg.IV.Length > 0)
                    package.IV = _aesAlg.IV;
                else
                    throw new ArgumentNullException("package.IV");
            }

            _aesAlg.Key = package.Key;
            _aesAlg.IV = package.IV;

            ICryptoTransform decryptor = _aesAlg.CreateDecryptor(_aesAlg.Key, _aesAlg.IV);

            using (MemoryStream msOutput = new MemoryStream(Convert.FromBase64String(package.Input)))
            {
                using (CryptoStream cryptoStream = new CryptoStream(msOutput, decryptor, CryptoStreamMode.Read))
                {
                    using (StreamReader sr = new StreamReader(cryptoStream))
                    {
                        package.Output = sr.ReadToEnd();
                        return package;
                    }
                }
            }
        }

        public void GenerateIV()
        {
            _aesAlg.GenerateIV();
        }

        public void GenerateKey()
        {
            _aesAlg.GenerateKey();
        }

        public class Package
        {
            public byte[] Key { get; set; }
            public byte[] IV { get; set; }
            public string Input { get; set; }
            public string Output { get; set; }
        }
    }
}
