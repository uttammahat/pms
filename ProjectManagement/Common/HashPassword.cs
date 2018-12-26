using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace ProjectManagement.Common
{
    public class HashPassword
    {
        public static string Encode(string encryptString)
        {
            string EncryptionKey = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            byte[] clearBytes = Encoding.Unicode.GetBytes(encryptString);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] {
            0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76
        });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    encryptString = Convert.ToBase64String(ms.ToArray());
                }
            }
            return encryptString;
        }
        
        public static string Decode(string password)
        {
            int mod4 = password.Length % 4;
            if (mod4 > 0)
            {
                password += new string('=', 4 - mod4);
            }

            string EncryptionKey = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            byte[] cipherBytes = Convert.FromBase64String(password);

            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);

                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                    }
                    password = Encoding.Unicode.GetString(ms.ToArray()).ToString();
                }
            }

            return password;
        }

        //public static string Decode(string cipherText)
        //{
        //    int mod4 = cipherText.Length % 4;
        //    if (mod4 > 0)
        //    {
        //        cipherText += new string('=', 4 - mod4);
        //    }

        //    string EncryptionKey = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        //    byte[] cipherBytes = Convert.FromBase64String(cipherText);
        //    using (Aes encryptor = Aes.Create())
        //    {
        //        Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
        //        encryptor.Key = pdb.GetBytes(32);
        //        encryptor.IV = pdb.GetBytes(16);
        //        //encryptor.Mode = CipherMode.CBC;
        //        using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
        //        {
        //            using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
        //            {
        //                cs.Write(cipherBytes, 0, cipherBytes.Length);
        //                // cs.Close();
        //            }
        //            cipherText = Encoding.Unicode.GetString(ms.ToArray());
        //        }
        //    }
        //    //girenMSISDN = cipherText;

        //    return cipherText;
        //}
    }
}