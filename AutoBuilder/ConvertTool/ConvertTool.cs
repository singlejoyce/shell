using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace ConvertTools
{
    public class ConvertTool
    {
        static string m_strKey = "ddianl1234567890";

        public static bool CheckDecryptContent(string file)
        {
            using (StreamReader sr = new StreamReader(file, Encoding.GetEncoding("gb18030")))
            {
                string strLine = sr.ReadLine();
                while (strLine != null)
                {
                    string decryptContent = DecryptContent(strLine);

                    if (decryptContent.Contains(" "))
                    {
                        sr.Close();

                        return false;
                    }

                    strLine = sr.ReadLine();
                }

                sr.Close();
            }

            return true;
        }

        public static string EncryptContent(string strContent)
        {
            byte[] arContent = UnicodeEncoding.Unicode.GetBytes(strContent);
            byte[] arKey = ASCIIEncoding.ASCII.GetBytes(m_strKey);

            TripleDESCryptoServiceProvider provider = new TripleDESCryptoServiceProvider();
            provider.Key = arKey;
            provider.Mode = CipherMode.ECB;
            provider.Padding = PaddingMode.PKCS7;

            ICryptoTransform encryptor = provider.CreateEncryptor();
            byte[] arData = encryptor.TransformFinalBlock(arContent, 0, arContent.Length);

            return Convert.ToBase64String(arData);
        }

        public static string DecryptContent(string strContent)
        {
            byte[] arContent = Convert.FromBase64String(strContent);
            byte[] arKey = ASCIIEncoding.ASCII.GetBytes(m_strKey);

            TripleDESCryptoServiceProvider provider = new TripleDESCryptoServiceProvider();
            provider.Key = arKey;
            provider.Mode = CipherMode.ECB;
            provider.Padding = PaddingMode.PKCS7;

            ICryptoTransform decryptor = provider.CreateDecryptor();
            byte[] arData = decryptor.TransformFinalBlock(arContent, 0, arContent.Length);

            return UnicodeEncoding.Unicode.GetString(arData);
        }
    }
}
