using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace Proyecto_Donacion.Models
{
    public class SeguridadUtil
    {
        // Clave y vector de inicialización para cifrado simétrico (AES)
        private static readonly byte[] Key = Encoding.UTF8.GetBytes("ABC123DEF4567890");  // 16 bytes = 128-bit key
        private static readonly byte[] IV = Encoding.UTF8.GetBytes("1234567890ABCDEF");  // 16 bytes IV

        /// <summary>Genera el hash SHA-256 de una cadena dada y la devuelve en formato Base64.</summary>
        public static string HashSHA256(string texto)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(texto);
                byte[] hash = sha256.ComputeHash(bytes);
                // Convertir a texto legible (usamos Base64 para representarlo compactamente)
                return Convert.ToBase64String(hash);
            }
        }

        /// <summary>Cifra una cadena de texto usando AES y devuelve el resultado en Base64.</summary>
        public static string Encriptar(string textoPlano)
        {
            if (textoPlano == null) return null;
            using (Aes aes = Aes.Create())
            {
                aes.Key = Key;
                aes.IV = IV;
                // Configuramos modo y relleno (por defecto AES usa CBC y PKCS#7, que están bien)
                ICryptoTransform encriptador = aes.CreateEncryptor(aes.Key, aes.IV);
                // Convertir texto a bytes y cifrar
                byte[] textoBytes = Encoding.UTF8.GetBytes(textoPlano);
                byte[] cifradoBytes;
                using (var ms = new System.IO.MemoryStream())
                using (var cs = new CryptoStream(ms, encriptador, CryptoStreamMode.Write))
                {
                    cs.Write(textoBytes, 0, textoBytes.Length);
                    cs.FlushFinalBlock();
                    cifradoBytes = ms.ToArray();
                }
                // Devolver como Base64 para poder transmitirlo como texto
                return Convert.ToBase64String(cifradoBytes);
            }
        }

        /// <summary>Descifra una cadena en Base64 previamente cifrada con el mismo método AES.</summary>
        public static string Desencriptar(string textoCifradoBase64)
        {
            if (textoCifradoBase64 == null) return null;
            using (Aes aes = Aes.Create())
            {
                aes.Key = Key;
                aes.IV = IV;
                ICryptoTransform desencriptador = aes.CreateDecryptor(aes.Key, aes.IV);
                byte[] cifradoBytes = Convert.FromBase64String(textoCifradoBase64);
                string textoPlano;
                using (var ms = new System.IO.MemoryStream(cifradoBytes))
                using (var cs = new CryptoStream(ms, desencriptador, CryptoStreamMode.Read))
                using (var sr = new System.IO.StreamReader(cs))
                {
                    textoPlano = sr.ReadToEnd();
                }
                return textoPlano;
            }
        }
    }
}