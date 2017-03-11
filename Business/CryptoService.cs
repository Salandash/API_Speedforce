using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Security.Cryptography;
using System.Text;
using System.IO;

namespace API_Speedforce.Business
{
    public class CryptoService : RandomNumberGenerator
    {
        private static RandomNumberGenerator GENERATOR = RandomNumberGenerator.Create();

        public CryptoService() { }

        public static string HashPassword(string password)
        {
            return Crypto.HashPassword(password ?? "");
        }

        public static bool VerifyHashedPassword(string hashPassword, string password)
        {
            return Crypto.VerifyHashedPassword(hashPassword, password);
        }

        public override void GetBytes(byte[] buffer) { GENERATOR.GetBytes(buffer); }

        public double NextDouble()
        {
            var b = new byte[4];
            GENERATOR.GetBytes(b);
            return (double)BitConverter.ToUInt32(b, 0) / UInt32.MaxValue;
        }

        public int Next(int minValue, int maxValue) { return (int)Math.Round(NextDouble() * (maxValue - minValue - 1)) + minValue; }

        public int Next() { return Next(0, Int32.MaxValue); }

        public int Next(int maxValue) { return Next(0, maxValue); }

        public void NextBytes(byte[] buffer) { GENERATOR.GetBytes(buffer); }

        public static string GenerateToken()
        {
            using (var cryptoProvider = new RNGCryptoServiceProvider())
            {
                var secretKeyByteArray = new byte[32]; //256 bit
                cryptoProvider.GetBytes(secretKeyByteArray);
                return Convert.ToBase64String(secretKeyByteArray);
            }
        }

        public static string RandomString(int length)
        {
            Random random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public static string Encrypt(String text)
        {
            RijndaelManaged RijndaelAlg = new RijndaelManaged();
            CryptoStream cStream = null;

            try
            {
                byte[] key = { 0x02, 0x1A, 0x03, 0x04C, 0x05, 0x06, 0xAB, 0x08, 0x09, 0xEF, 0x11, 0x12, 0x13, 0x14, 0x15, 0x16 };
                byte[] IV = { 0x01, 0x11, 0x10, 0x99, 0x19, 0x06, 0x07, 0x28, 0x09, 0x10, 0x11, 0x12, 0x33, 0x14, 0x15, 0x56 };
                byte[] inputByteArray = Encoding.UTF8.GetBytes(text);

                MemoryStream memoryS = new MemoryStream();
                cStream = new CryptoStream(memoryS, RijndaelAlg.CreateEncryptor(key, IV), CryptoStreamMode.Write);
                cStream.Write(inputByteArray, 0, inputByteArray.Length);
                cStream.FlushFinalBlock();
                return Convert.ToBase64String(memoryS.ToArray());
            }
            catch (Exception e)
            {
                string y = e.Message.ToString();
                return null;
            }

            finally
            {
                if (cStream != null)
                    cStream.Close();
            }
        }

        public static string Decrypt(String text)
        {
            Rijndael RijndaelAlg = Rijndael.Create();
            CryptoStream cStream = null;

            try
            {
                byte[] key = { 0x02, 0x1A, 0x03, 0x04C, 0x05, 0x06, 0xAB, 0x08, 0x09, 0xEF, 0x11, 0x12, 0x13, 0x14, 0x15, 0x16 };
                byte[] IV = { 0x01, 0x11, 0x10, 0x99, 0x19, 0x06, 0x07, 0x28, 0x09, 0x10, 0x11, 0x12, 0x33, 0x14, 0x15, 0x56 };
                byte[] inputByteArray = Convert.FromBase64String(text);
                MemoryStream memoryS = new MemoryStream();
                cStream = new CryptoStream(memoryS, RijndaelAlg.CreateDecryptor(key, IV), CryptoStreamMode.Write);
                cStream.Write(inputByteArray, 0, inputByteArray.Length);
                cStream.FlushFinalBlock();
                Encoding encode = Encoding.UTF8;
                return encode.GetString(memoryS.ToArray());
            }
            catch (Exception e)
            {
                string y = e.Message.ToString();
                return null;
            }
            finally
            {
                if (cStream != null)
                    cStream.Close();
            }
        }
    }
}