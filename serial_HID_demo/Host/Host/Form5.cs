using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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
            InitializeComponent();
        }

        public void Func()
        {
            var privateKey = @"-----BEGIN EC PRIVATE KEY-----
//MHcCAQEEIP17ItsJhzB7MeAIAQq3EnldFRdNN+V4zjmyPlK+J3ZYoAoGCCqGSM49
//AwEHoUQDQgAEtvkZATuuDUG41t+2Vj0HaQIComIin+8+xpDH40QV7lcD2JmBPX/S
//nd+mQBJKntSqBfXYkWFmdWWSK7v9RJmIPg==
//-----END EC PRIVATE KEY-----";
            var publicKey = @"-----BEGIN PUBLIC KEY-----
MFkwEwYHKoZIzj0CAQYIKoZIzj0DAQcDQgAE45uC4bJ8GTLkhYP4wE9kTHFgHFP0
It1P88Qy7z3cDiowjNc+Spt8ofRXeu5CMogSw1Ib8P5MO3Aij+cLiS+HSg==
-----END PUBLIC KEY-----";

//            var serverPrivateKeyPem = `-----BEGIN EC PRIVATE KEY-----
//MHcCAQEEIP17ItsJhzB7MeAIAQq3EnldFRdNN+V4zjmyPlK+J3ZYoAoGCCqGSM49
//AwEHoUQDQgAEtvkZATuuDUG41t+2Vj0HaQIComIin+8+xpDH40QV7lcD2JmBPX/S
//nd+mQBJKntSqBfXYkWFmdWWSK7v9RJmIPg==
//-----END EC PRIVATE KEY-----`;
//var clientPublicKeyPem = `-----BEGIN PUBLIC KEY-----
//MFkwEwYHKoZIzj0CAQYIKoZIzj0DAQcDQgAE45uC4bJ8GTLkhYP4wE9kTHFgHFP0
//It1P88Qy7z3cDiowjNc+Spt8ofRXeu5CMogSw1Ib8P5MO3Aij+cLiS+HSg==
//-----END PUBLIC KEY-----`;

            var ecc = ECDiffieHellman.Create();
            ecc.ImportFromPem(privateKey.ToCharArray());

            ecc.DeriveKeyMaterial(publicKey);

            //var decryptedBytes = rsa.Decrypt(
            //    Convert.FromHexString("BD4D6E760777DBB3ED95EA14368ABA9D79CEFA63C74255A64041CF7F45F69CCF0FBF577082C9A3F4875BF55F4A694CA458EBA91867567690B695B44237051685902C0C9399F7217B03AF4FEACDD8163919162515F4F61B60DED8B101995D88DA19AE45865CF9D19F119A193C68E2D32418AA9F2DC344DAC355A994DC2CD4A6CE"),
            //    RSAEncryptionPadding.Pkcs1
            //);

            //// this will print the original unencrypted string
            //Debug.WriteLine(Encoding.UTF8.GetString(decryptedBytes));
        }
    }
}
