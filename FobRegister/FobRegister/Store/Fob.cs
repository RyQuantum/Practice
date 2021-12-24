using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace FobRegister
{
    public class Fob
    {
        public int Id { get; set; }
        public string UID { get; set; }
        public string Key { get; set; }
        public bool Uploaded { get; set; }

        //[NotMapped]
        //public string DecID { get; set; }
        //[NotMapped]
        //public string DecryptedKey { get; set; }
        //[NotMapped]
        //public string MixedKey { get; set; }

        [NotMapped]
        public string DecID { get { return Int64.Parse(UID, System.Globalization.NumberStyles.HexNumber).ToString(); }}
        [NotMapped]
        public string DecryptedKey { get { return decrypt(Key); } }
        [NotMapped]
        public string MixedKey { get { return "****************************" + DecryptedKey.Substring(28); } }
        [NotMapped]
        private static readonly byte[] IvBytes = { 0x01, 0x23, 0x25, 0x67, 0x80, 0xAB, 0xAD, 0xEF };

        public static string encrypt(string input)
        {
            var keyBytes = Encoding.UTF8.GetBytes("Rently12");

            var des = DES.Create();
            des.Mode = CipherMode.ECB;
            des.Padding = PaddingMode.Zeros; //自动补 0

            using (var ms = new MemoryStream())
            {
                var data = Encoding.UTF8.GetBytes(input);

                using (var cs = new CryptoStream(ms, des.CreateEncryptor(keyBytes, IvBytes), CryptoStreamMode.Write))
                {
                    cs.Write(data, 0, data.Length);
                    cs.FlushFinalBlock();
                }

                return BitConverter.ToString(ms.ToArray()).Replace("-", string.Empty); ; ;
            }
        }

        public static string decrypt(string data)
        {
            var arr = Enumerable.Range(0, data.Length)
                    .Where(x => x % 2 == 0)
                    .Select(x => Convert.ToByte(data.Substring(x, 2), 16))
                    .ToArray();
            var keyBytes = Encoding.UTF8.GetBytes("Rently12");

            var des = DES.Create();
            des.Mode = CipherMode.ECB;
            des.Padding = PaddingMode.Zeros; //自动补0

            using (var ms = new MemoryStream())
            {
                using (var cs = new CryptoStream(ms, des.CreateDecryptor(keyBytes, IvBytes), CryptoStreamMode.Write))
                {
                    cs.Write(arr, 0, arr.Length);

                    cs.FlushFinalBlock();
                }

                string res = Encoding.UTF8.GetString(ms.ToArray());
                return res.TrimEnd('\0');
            }
        }
    }
}
