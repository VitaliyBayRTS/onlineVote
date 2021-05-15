using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace OV.Services.AES_Operation
{
    public enum AES_IV_MODES
    {
        InitializationVectorEnable,
        InitializationVectorDisable,
    };

    public static class AES
    {
        public static string EncryptString(string key, string stringToEncrypt, AES_IV_MODES? aes_mode = AES_IV_MODES.InitializationVectorEnable)
        {

            byte[] encryptedArray;
            Aes aes = Aes.Create();
            aes.Key = Encoding.UTF8.GetBytes(key);

            if(aes_mode == AES_IV_MODES.InitializationVectorEnable)  includeIV(aes);

            ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, encryptor, CryptoStreamMode.Write))
                {
                    using (StreamWriter streamWriter = new StreamWriter((Stream)cryptoStream))
                    {
                        streamWriter.Write(stringToEncrypt);
                    }

                    encryptedArray = memoryStream.ToArray();
                }
            }

            return Convert.ToBase64String(encryptedArray);
        }


        public static string DecryptString(string key, string cipherText, AES_IV_MODES? aes_mode = AES_IV_MODES.InitializationVectorEnable)
        {
            byte[] buffer = Convert.FromBase64String(cipherText);

            Aes aes = Aes.Create();
            aes.Key = Encoding.UTF8.GetBytes(key);

            if (aes_mode == AES_IV_MODES.InitializationVectorEnable) includeIV(aes);

            ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

            using (MemoryStream memoryStream = new MemoryStream(buffer))
            {
                using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, decryptor, CryptoStreamMode.Read))
                {
                    using (StreamReader streamReader = new StreamReader((Stream)cryptoStream))
                    {
                        return streamReader.ReadToEnd();
                    }
                }
            }
        }


        private static void includeIV(Aes aes)
        {
            byte[] iv = new byte[16];
            aes.IV = iv;
        }
    }
}
