using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;


public class Security : MonoBehaviour
{
    public static string Encrypt(string text)
    {
        using (Aes aes = Aes.Create())
        {
            // Đảm bảo khóa và IV có đủ độ dài
            aes.Key = Encoding.UTF8.GetBytes("1234567890123456"); // 16 bytes cho khóa 128-bit
            aes.IV = Encoding.UTF8.GetBytes("1234567890123456"); // 16 bytes cho IV 128-bit

            // Tạo bộ mã hóa và giải mã
            ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
            byte[] encryptedBytes = null;

            // Mã hóa dữ liệu
            using (var msEncrypt = new System.IO.MemoryStream())
            {
                using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                {
                    using (var swEncrypt = new System.IO.StreamWriter(csEncrypt))
                    {
                        swEncrypt.Write(text);
                    }
                    encryptedBytes = msEncrypt.ToArray();
                }
            }

            // Chuyển đổi sang chuỗi Base64
            return Convert.ToBase64String(encryptedBytes);
        }
    }


    public static string Decrypt(string encrypted)
    {
        using (Aes aes = Aes.Create())
        {
            // Đảm bảo khóa và IV có đủ độ dài
            aes.Key = Encoding.UTF8.GetBytes("1234567890123456"); // 16 bytes cho khóa 128-bit
            aes.IV = Encoding.UTF8.GetBytes("1234567890123456"); // 16 bytes cho IV 128-bit

            // Tạo bộ mã hóa và giải mã
            ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
            string decryptedText = null;

            // Giải mã dữ liệu
            using (var msDecrypt = new System.IO.MemoryStream(Convert.FromBase64String(encrypted)))
            {
                using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                {
                    using (var srDecrypt = new System.IO.StreamReader(csDecrypt))
                    {
                        decryptedText = srDecrypt.ReadToEnd();
                    }
                }
            }

            return decryptedText;
        }
    }


    static Aes getAes()
    {
        var keyBytes = new byte[16];
        var skeyBytes = Encoding.UTF8.GetBytes("12345678901234567890123456789012");
        Array.Copy(skeyBytes, keyBytes, Math.Min(keyBytes.Length, skeyBytes.Length));

        Aes aes = Aes.Create();
        aes.Mode = CipherMode.CBC;
        aes.Padding = PaddingMode.PKCS7;
        aes.KeySize = 128;
        aes.Key = keyBytes;
        aes.IV = keyBytes;

        return aes;
    }
}
