using System;
using System.Text;
using System.Security.Cryptography;
using Cysharp.Text;

namespace QuickUnity.Extensions.Security
{
    public static class Encrypt
    {
        public static string DeTripleDesc(string key, string value)
        {
            // retrieve encrypt
            var secret = MD5(key);
            var bytes = Convert.FromBase64String(value);

            // decrypt value 3DES
            TripleDES des = new TripleDESCryptoServiceProvider();
            des.Key = secret;
            des.Mode = CipherMode.ECB;
            ICryptoTransform xform = des.CreateDecryptor();
            var decrypt = xform.TransformFinalBlock(bytes, 0, bytes.Length);

            return Encoding.UTF8.GetString(decrypt);
        }

        public static string TripleDesc(string key, string value)
        {
            // encrypt value
            var secret = MD5(key);
            var bytes = Encoding.UTF8.GetBytes(value);

            TripleDES des = new TripleDESCryptoServiceProvider();
            des.Key = secret;
            des.Mode = CipherMode.ECB;
            ICryptoTransform xform = des.CreateEncryptor();
            var encrypted = xform.TransformFinalBlock(bytes, 0, bytes.Length);

            // convert encrypt
            return Convert.ToBase64String(encrypted);
        }

        public static byte[] MD5(byte[] bytes)
        {
            MD5CryptoServiceProvider md5Hash = new MD5CryptoServiceProvider();
            return md5Hash.ComputeHash(bytes);
        }

        public static byte[] MD5(string str)
        {
            return MD5(Encoding.UTF8.GetBytes(str));
        }

        public static string MD5ToString(byte[] bytes)
        {
            var bs = MD5(bytes);
            Utf8ValueStringBuilder builder = ZString.CreateUtf8StringBuilder();

            // to hex decimal
            foreach (var b in bs)
            {
                builder.Append(b.ToString("x2"));
            }

            return builder.ToString();
        }

        public static string MD5ToString(string str)
        {
            var bytes = MD5(str);
            Utf8ValueStringBuilder builder = ZString.CreateUtf8StringBuilder();

            // to hex decimal
            foreach (var b in bytes)
            {
                builder.Append(b.ToString("x2"));
            }

            return builder.ToString();
        }

        public static string SHA1Key(string strToEncrypt)
        {
            UTF8Encoding utf8Encoding = new UTF8Encoding();
            var bytes = utf8Encoding.GetBytes(strToEncrypt);

            SHA1 sha = new SHA1CryptoServiceProvider();
            var hashBytes = sha.ComputeHash(bytes);

            Utf8ValueStringBuilder builder = ZString.CreateUtf8StringBuilder();
            for (var i = 0; i < hashBytes.Length; i++)
            {
                builder.Append(Convert.ToString(hashBytes[i], 16).PadLeft(2, '0'));
            }

            return builder.ToString().PadLeft(32, '0');
        }
    }
}
