using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Host
{
    public partial class Form5 : Form
    {
        public Form5()
        {
            Func();
            InitializeComponent();
        }

        public void Func()
        {
            var privateKey = @"-----BEGIN EC PRIVATE KEY-----
MHcCAQEEIP17ItsJhzB7MeAIAQq3EnldFRdNN+V4zjmyPlK+J3ZYoAoGCCqGSM49
AwEHoUQDQgAEtvkZATuuDUG41t+2Vj0HaQIComIin+8+xpDH40QV7lcD2JmBPX/S
nd+mQBJKntSqBfXYkWFmdWWSK7v9RJmIPg==
-----END EC PRIVATE KEY-----";
            var publicKey = @"-----BEGIN PUBLIC KEY-----
MFkwEwYHKoZIzj0CAQYIKoZIzj0DAQcDQgAE45uC4bJ8GTLkhYP4wE9kTHFgHFP0
It1P88Qy7z3cDiowjNc+Spt8ofRXeu5CMogSw1Ib8P5MO3Aij+cLiS+HSg==
-----END PUBLIC KEY-----";

            var ecc = ECDiffieHellmanCng.Create();
            //ecc.ImportECPrivateKey(Convert.FromBase64String(privateKey), out _);
            ecc.ImportFromPem(privateKey.ToCharArray());
            Debug.WriteLine("privKey: " + BitConverter.ToString(ecc.ExportECPrivateKey()).Replace("-", ""));

            var ecc2 = ECDiffieHellmanCng.Create();
            ecc2.ImportFromPem(publicKey.ToCharArray());

            //var nodejsKey = @"MFkwEwYHKoZIzj0CAQYIKoZIzj0DAQcDQgAE45uC4bJ8GTLkhYP4wE9kTHFgHFP0It1P88Qy7z3cDiowjNc+Spt8ofRXeu5CMogSw1Ib8P5MO3Aij+cLiS+HSg==";
            //byte[] key1 = Convert.FromBase64String(nodejsKey);

            //byte[] keyX = new byte[key1.Length / 2];
            //byte[] keyY = new byte[keyX.Length];
            //Buffer.BlockCopy(key1, 1, keyX, 0, keyX.Length);
            //Buffer.BlockCopy(key1, 1 + keyX.Length, keyY, 0, keyY.Length);
            //ECParameters parameters = new ECParameters
            //{
            //    Curve = ECCurve.NamedCurves.brainpoolP256r1,
            //    Q =
            //{
            //    X = keyX,
            //    Y = keyY,
            //},
            //};
            //ECDiffieHellman bob = ECDiffieHellman.Create(parameters);


                //byte[] bytes = Convert.FromBase64String(publicKey);
                //CngKey alicePubKey = CngKey.Import(bytes,
                //      CngKeyBlobFormat.EccPublicBlob);
                //var bob = ECDiffieHellman.Create();
            var res = ecc.DeriveKeyMaterial(ecc2.PublicKey);

            Debug.WriteLine("res: " + BitConverter.ToString(res).Replace("-", ""));

            //var decryptedBytes = rsa.Decrypt(
            //    Convert.FromHexString("BD4D6E760777DBB3ED95EA14368ABA9D79CEFA63C74255A64041CF7F45F69CCF0FBF577082C9A3F4875BF55F4A694CA458EBA91867567690B695B44237051685902C0C9399F7217B03AF4FEACDD8163919162515F4F61B60DED8B101995D88DA19AE45865CF9D19F119A193C68E2D32418AA9F2DC344DAC355A994DC2CD4A6CE"),
            //    RSAEncryptionPadding.Pkcs1
            //);

            //// this will print the original unencrypted string
            //Debug.WriteLine(Encoding.UTF8.GetString(decryptedBytes));
        }
    }
}
