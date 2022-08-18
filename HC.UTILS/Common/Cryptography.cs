using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace HC.UTILS.Common
{
    public static class Cryptography
    {
        //Encryption
     
        // Herhangi bir metni şifrelemek için kullanılır.
        public static string Encryption(string text)
        {
            var arrayBytes = Encoding.ASCII.GetBytes("12345678");
            if (string.IsNullOrEmpty(text))
                return string.Empty;
            var cryptoProvider = new DESCryptoServiceProvider();
            var memoryStream = new MemoryStream();
            var cryptoStream = new CryptoStream(memoryStream, cryptoProvider.CreateEncryptor(arrayBytes, arrayBytes),
              CryptoStreamMode.Write);
            var streamWriter = new StreamWriter(cryptoStream);
            streamWriter.Write(text);
            streamWriter.Flush();
            cryptoStream.FlushFinalBlock();
            streamWriter.Flush();
            return Convert.ToBase64String(memoryStream.GetBuffer(), 0, (int)memoryStream.Length);
        }
        

       //Decryption

       // Şifrelenmiş metnin şifresini açmak için kullanılır.
        public static string Decryption(string text)
        {
            var arrayBytes = Encoding.ASCII.GetBytes("12345678");
            if (string.IsNullOrEmpty(text))
                return string.Empty;
            var cryptoProvider = new DESCryptoServiceProvider();
            var memoryStream = new MemoryStream(Convert.FromBase64String(text));
            var cryptoStream = new CryptoStream(memoryStream, cryptoProvider.CreateDecryptor(arrayBytes, arrayBytes),
              CryptoStreamMode.Read);
            var streamReader = new StreamReader(cryptoStream);
            return streamReader.ReadToEnd();
        }

    }
}