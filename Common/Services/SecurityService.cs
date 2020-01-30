using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace Common.Services
{
    /// <summary>
    /// A custom service that contains many commonly-used methods used for encryptions and security.
    /// </summary>
    public class Security
    {
        private const string _chars = "1234567890abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
        private static Random _random = new Random();

        public static string RandomString(int size)
        {
            char[] buffer = new char[size];
            //TODO: need to understand random and how it works. Everything I can see pegs an importance
            //to no creating it each time as it can produce identical results.
            //but it also says it is not thread safe....so for simplicity sake, im using a statically created one
            //and locking it to ensure no two threads call it at the same time
            //I'll want to come back to this and flush it out better.
            lock (_random)
            {
                for (int i = 0; i < size; i++)
                {
                    buffer[i] = _chars[_random.Next(_chars.Length)];
                }
            }
            return new string(buffer);
        }

        public static byte[] EncryptToBytes(string plainText, byte[] key, byte[] salt)
        {
            byte[] encrypted;

            using (var aesAlg = new AesManaged())
            {
                aesAlg.Key = key;
                aesAlg.IV = salt;

                var encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);
                using (var msEncrypt = new MemoryStream())
                {
                    using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (var swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(plainText);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }
            return encrypted;
        }

        public static string DecryptFromBytes(byte[] encrypted, byte[] key, byte[] salt)
        {
            string plaintext = null;

            using (var aesAlg = new AesManaged())
            {
                aesAlg.Key = key;
                aesAlg.IV = salt;

                var decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                using (var msDecrypt = new MemoryStream(encrypted))
                {
                    using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (var srDecrypt = new StreamReader(csDecrypt))
                        {
                            plaintext = srDecrypt.ReadToEnd();
                        }
                    }
                }
            }
            return plaintext;
        }

        public static byte[] EncryptToBytes(string plainText, string key, string iv)
        {
            byte[] encrypted;

            using (var aesAlg = new AesManaged())
            using (var hasher = new SHA256Managed())
            {
                aesAlg.Key = hasher.ComputeHash(Encoding.UTF8.GetBytes(key));
                aesAlg.IV = Encoding.UTF8.GetBytes(iv);

                var encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);
                using (var msEncrypt = new MemoryStream())
                {
                    using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (var swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(plainText);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }
            return encrypted;
        }

        public static string Encrypt(string plainText, string key, string iv)
        {
            byte[] encrypted;

            using (var aesAlg = new AesManaged())
            using (var hasher = new SHA256Managed())
            {
                aesAlg.Key = hasher.ComputeHash(Encoding.UTF8.GetBytes(key));
                aesAlg.IV = Encoding.UTF8.GetBytes(iv);

                var encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);
                using (var msEncrypt = new MemoryStream())
                {
                    using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (var swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(plainText);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }
            return Convert.ToBase64String(encrypted);
        }

        public static string Decrypt(string encrypted, string key, string iv)
        {
            string plaintext = null;

            using (var aesAlg = new AesManaged())
            using (var hasher = new SHA256Managed())
            {
                aesAlg.Key = hasher.ComputeHash(Encoding.UTF8.GetBytes(key));
                aesAlg.IV = Encoding.UTF8.GetBytes(iv);

                var decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                using (var msDecrypt = new MemoryStream(Convert.FromBase64String(encrypted)))
                {
                    using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (var srDecrypt = new StreamReader(csDecrypt))
                        {
                            plaintext = srDecrypt.ReadToEnd();
                        }
                    }
                }
            }
            return plaintext;
        }

        public static string UrlEncrypt(string plainText, string key, string iv)
        {
            return HttpContext.Current.Server.UrlEncode(Encrypt(plainText, key, iv));
        }

        public static string UrlDecrypt(string plainText, string key, string iv)
        {
            return Decrypt(HttpContext.Current.Server.UrlDecode(plainText), key, iv);
        }

        public static string DecryptFromBytes(byte[] encrypted, string key, string iv)
        {
            string plaintext = null;

            using (var aesAlg = new AesManaged())
            using (var hasher = new SHA256Managed())
            {
                aesAlg.Key = hasher.ComputeHash(Encoding.UTF8.GetBytes(key));
                aesAlg.IV = Encoding.UTF8.GetBytes(iv);

                var decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                using (var msDecrypt = new MemoryStream(encrypted))
                {
                    using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (var srDecrypt = new StreamReader(csDecrypt))
                        {
                            plaintext = srDecrypt.ReadToEnd();
                        }
                    }
                }
            }
            return plaintext;
        }

        public static byte[] ComputeHash(string plainText)
        {
            SHA256Managed sha = new SHA256Managed();
            return sha.ComputeHash(Encoding.UTF8.GetBytes(plainText));
        }

        public static byte[] ComputeHashWithRandomSalt(string plainText)
        {
            byte[] salt = new byte[8];
            //create a 5 byte salt
            RNGCryptoServiceProvider saltCreator = new RNGCryptoServiceProvider();
            saltCreator.GetBytes(salt);
            return ComputeHash(plainText, salt);
        }

        public static byte[] ComputeHash(string plainText, byte[] saltBytes)
        {
            //get bytes of text
            byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);

            //combine data and salt (salt at end)
            byte[] plainTextWithSaltBytes = new byte[plainTextBytes.Length + saltBytes.Length];
            Array.Copy(plainTextBytes, plainTextWithSaltBytes, plainTextBytes.Length);
            Array.Copy(saltBytes, 0, plainTextWithSaltBytes, plainTextBytes.Length, saltBytes.Length);

            //create hash on data and salt
            SHA256Managed sha = new SHA256Managed();
            byte[] hashBytes = sha.ComputeHash(plainTextWithSaltBytes);

            //combine salt with hash
            byte[] hashWithSaltBytes = new byte[hashBytes.Length + saltBytes.Length];

            //combine hash and salt (salt at end)
            Array.Copy(hashBytes, hashWithSaltBytes, hashBytes.Length);
            Array.Copy(saltBytes, 0, hashWithSaltBytes, hashBytes.Length, saltBytes.Length);
            return hashWithSaltBytes;
        }

        public static string GetHashString(string inputString)
        {
            StringBuilder sb = new StringBuilder();
            foreach (byte b in ComputeHash(inputString))
                sb.Append(b.ToString("X2"));

            return sb.ToString();
        }

        public static bool VerifySaltedHash(string plainText, byte[] hash)
        {
            byte[] hashWithSaltBytes = hash;

            int hashSizeInbits = 256;
            int hashSizeInBytes = hashSizeInbits / 8;

            //we are using 256 bit hash so any less than 32 bytes is bad
            if (hashWithSaltBytes.Length < hashSizeInBytes)
                return false;

            byte[] saltBytes = new byte[hashWithSaltBytes.Length - hashSizeInBytes];

            //grab just the salt bytes
            Array.Copy(hashWithSaltBytes, hashSizeInBytes, saltBytes, 0, saltBytes.Length);

            //hash the new plan text deal
            byte[] compareHash = ComputeHash(plainText, saltBytes);

            if (hash.Length != compareHash.Length)
                return false;

            //comapare the array
            for (int i = 0; i < hash.Length; i++)
            {
                if (hash[i] != compareHash[i])
                    return false;
            }

            return true;
        }

        public static string IdentityEncrypt(object plainText, int customerID)
        {
            return IdentityEncrypt(plainText, customerID, string.Empty);
        }
        public static string IdentityEncrypt(object plainText, int customerID, string key)
        {
            if (plainText == null) return string.Empty;
            return HttpContext.Current.Server.UrlEncode(Encrypt(plainText.ToString(), customerID.ToString() + key, GlobalSettings.Encryptions.General.IV));
        }

        public static string IdentityDecrypt(string encryptedText, int customerID)
        {
            return IdentityDecrypt(encryptedText, customerID, string.Empty);
        }
        public static string IdentityDecrypt(string encryptedText, int customerID, string key)
        {
            var urlDecodedEncryptedtext = encryptedText;
            if (HttpUtility.HtmlDecode(encryptedText) != encryptedText)
            {
                urlDecodedEncryptedtext = HttpContext.Current.Server.UrlDecode(urlDecodedEncryptedtext);
            }
            return Decrypt(urlDecodedEncryptedtext, customerID.ToString() + key, GlobalSettings.Encryptions.General.IV);
        }
    }
}