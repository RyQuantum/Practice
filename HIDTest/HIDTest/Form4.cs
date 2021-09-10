using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HIDTest
{
    public partial class Form4 : Form
    {
        public Form4()
        {
            InitializeComponent();
            Func();
            Func2();
        }

        public void Func()
        {
            var privateKey = @"-----BEGIN RSA PRIVATE KEY-----
MIICXAIBAAKBgQDuB/wjBTa+riSddHmmXnW2J92Vws4/0lDsioZXNlhsHzwuvTc4
bPXt1x+H+IxvtijQ7EyfXwU7D91FQlCkUwMcNnrLFLDHiGc5+yXHU8Xn5YTdkUfs
myBKn7LpE2DLiBa0L+KOFLoqGFPUPVnELF3LzxCKiDZp7b2kAjr+JyaWaQIDAQAB
AoGAVEB1SXwF9ZY0FZttl6qlpZUUxHaQhFyQEjmx9VqiD5s6tRx6WDmRDt9vJLSg
GMZ0Sgbp1BtRAafK3UHsw00ysvHyjz26we8vp1pJTXph/ixu2otJRDQeBbVpHgIh
QxguZO+O3hevzqDZxWGRIjGqHjAQoDaxkAolCBLCIL4m+LUCQQD9TtyytkKWvUmB
gxsYQdRLX78lxGolw3wUtu9ub3ZWW/4gQDXxXLE0vZawQZ6VH0HJnpGg44ueRCPY
A7r2TB8zAkEA8I+PsQG6vqNpQ4qNBsJS8YyR18+puyPdChCQRU1ychA0gAEDoUwI
Ma8AoRMd+m595t3jrZNEEJSGclgnrNsj8wJAGIQ0ehuH0F0bqyxMSAm/CViIAJjF
CUilTr/x5odlNbCLpQVx8x1cjVU4K3D+b0JqNKBjSeYcEkJYEcUyqDnv3wJBAMUf
bsjGOQeUkmJlO3Dpddw3qx7kLRqLpcpkjkQr1XdAbjTiH95hlmlYFTTonG0lc4fm
FXabsW/AtXcnY8OSgx8CQDNI44PsCX/lopgPheiHqT87pFYphWiXfzcpgkTIIPDh
E0m+XzaobsIPmZHfmKGzOWY/+T3Hrpjzmb9h6XPev+4=
-----END RSA PRIVATE KEY-----";

            var rsa = RSA.Create();
            rsa.ImportFromPem(privateKey.ToCharArray());

            var decryptedBytes = rsa.Decrypt(
                Convert.FromHexString("BD4D6E760777DBB3ED95EA14368ABA9D79CEFA63C74255A64041CF7F45F69CCF0FBF577082C9A3F4875BF55F4A694CA458EBA91867567690B695B44237051685902C0C9399F7217B03AF4FEACDD8163919162515F4F61B60DED8B101995D88DA19AE45865CF9D19F119A193C68E2D32418AA9F2DC344DAC355A994DC2CD4A6CE"),
                RSAEncryptionPadding.Pkcs1
            );

            // this will print the original unencrypted string
            Debug.WriteLine(Encoding.UTF8.GetString(decryptedBytes));
        }

        public void Func2()
        {
            //2048 公钥
            string publicKey =
                "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAoQh0wEqx/R2H1v00IU12Oc30fosRC/frhH89L6G+fzeaqI19MYQhEPMU13wpeqRONCUta+2iC1sgCNQ9qGGf19yGdZUfueaB1Nu9rdueQKXgVurGHJ+5N71UFm+OP1XcnFUCK4wT5d7ZIifXxuqLehP9Ts6sNjhVfa+yU+VjF5HoIe69OJEPo7OxRZcRTe17khc93Ic+PfyqswQJJlY/bgpcLJQnM+QuHmxNtF7/FpAx9YEQsShsGpVo7JaKgLo+s6AFoJ4QldQKir2vbN9vcKRbG3piElPilWDpjXQkOJZhUloh/jd7QrKFimZFldJ1r6Q59QYUyGKZARUe0KZpMQIDAQAB";
            //2048 私钥
            string privateKey =
                "MIIEpAIBAAKCAQEAoQh0wEqx/R2H1v00IU12Oc30fosRC/frhH89L6G+fzeaqI19MYQhEPMU13wpeqRONCUta+2iC1sgCNQ9qGGf19yGdZUfueaB1Nu9rdueQKXgVurGHJ+5N71UFm+OP1XcnFUCK4wT5d7ZIifXxuqLehP9Ts6sNjhVfa+yU+VjF5HoIe69OJEPo7OxRZcRTe17khc93Ic+PfyqswQJJlY/bgpcLJQnM+QuHmxNtF7/FpAx9YEQsShsGpVo7JaKgLo+s6AFoJ4QldQKir2vbN9vcKRbG3piElPilWDpjXQkOJZhUloh/jd7QrKFimZFldJ1r6Q59QYUyGKZARUe0KZpMQIDAQABAoIBAQCRZLUlOUvjIVqYvhznRK1OG6p45s8JY1r+UnPIId2Bt46oSLeUkZvZVeCnfq9k0Bzb8AVGwVPhtPEDh73z3dEYcT/lwjLXAkyPB6gG5ZfI/vvC/k7JYV01+neFmktw2/FIJWjEMMF2dvLNZ/Pm4bX1Dz9SfD/45Hwr8wqrvRzvFZsj5qqOxv9RPAudOYwCwZskKp/GF+L+3Ycod1Wu98imzMZUH+L5dQuDGg3kvf3ljIAegTPoqYBg0imNPYY/EGoFKnbxlK5S5/5uAFb16dGJqAz3XQCz9Is/IWrOTu0etteqV2Ncs8uqPdjed+b0j8CMsr4U1xjwPQ8WwdaJtTkRAoGBANAndgiGZkCVcc9975/AYdgFp35W6D+hGQAZlL6DmnucUFdXbWa/x2rTSEXlkvgk9X/PxOptUYsLJkzysTgfDywZwuIXLm9B3oNmv3bVgPXsgDsvDfaHYCgz0nHK6NSrX2AeX3yO/dFuoZsuk+J+UyRigMqYj0wjmxUlqj183hinAoGBAMYMOBgF77OXRII7GAuEut/nBeh2sBrgyzR7FmJMs5kvRh6Ck8wp3ysgMvX4lxh1ep8iCw1R2cguqNATr1klOdsCTOE9RrhuvOp3JrYzuIAK6MpH/uBICy4w1rW2+gQySsHcH40r+tNaTFQ7dQ1tef//iy/IW8v8i0t+csztE1JnAoGABdtWYt8FOYP688+jUmdjWWSvVcq0NjYeMfaGTOX/DsNTL2HyXhW/Uq4nNnBDNmAz2CjMbZwt0y+5ICkj+2REVQVUinAEinTcAe5+LKXNPx4sbX3hcrJUbk0m+rSu4G0B/f5cyXBsi9wFCAzDdHgBduCepxSr04Sc9Hde1uQQi7kCgYB0U20HP0Vh+TG2RLuE2HtjVDD2L/CUeQEiXEHzjxXWnhvTg+MIAnggvpLwQwmMxkQ2ACr5sd/3YuCpB0bxV5o594nsqq9FWVYBaecFEjAGlWHSnqMoXWijwu/6X/VOTbP3VjH6G6ECT4GR4DKKpokIQrMgZ9DzaezvdOA9WesFdQKBgQCWfeOQTitRJ0NZACFUn3Fs3Rvgc9eN9YSWj4RtqkmGPMPvguWo+SKhlk3IbYjrRBc5WVOdoX8JXb2/+nAGhPCuUZckWVmZe5pMSr4EkNQdYeY8kOXGSjoTOUH34ZdKeS+e399BkBWIiXUejX/Srln0H4KoHnTWgxwNpTsBCgXu8Q==";

            var rsa = new RSAHelper(RSAType.RSA2, Encoding.UTF8, privateKey, publicKey);

            string str = "博客园 http://www.cnblogs.com/";

            Debug.WriteLine("原始字符串：" + str);

            //加密
            string enStr = rsa.Encrypt(str);

            Debug.WriteLine("加密字符串：" + enStr);

            //解密
            string deStr = rsa.Decrypt(enStr);

            Debug.WriteLine("解密字符串：" + deStr);

            //私钥签名
            string signStr = rsa.Sign(str);

            Debug.WriteLine("字符串签名：" + signStr);

            //公钥验证签名
            bool signVerify = rsa.Verify(str, signStr);

            Debug.WriteLine("验证签名：" + signVerify);

            //Console.ReadKey(); //?
        }
    }
}
