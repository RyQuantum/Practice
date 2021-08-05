using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace AHID_demo
{
    public class RSACryption
    {
        public RSACryption()
        {
        }

        #region RSA 加密解密 
        #region RSA 的密钥产生 

        /// <summary>
        /// RSA 的密钥产生 产生私钥 和公钥 
        /// </summary>
        /// <param name="xmlKeys"></param>
        /// <param name="xmlPublicKey"></param>
        public void RSAKey(out string xmlKeys, out string xmlPublicKey)
        {
            System.Security.Cryptography.RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            rsa.FromXmlString("<RSAKeyValue><Modulus>0+CJc2FxooiK0QNEV/MqLmL/000cuJJ29mOukeMS9ZNcntE/rJ5XbrPuG16zGXhJX8gwFaEAHTtvmda+pTO9OYa/6PvTmHLl8NJJVL71rWzoSys3ZC+kkcIqIBqyoIHSYDTYnd59YFy2/t0a86GXbZE4Nm42/flOLaW1C4QasqbktEYQO7G9myrZCKNspnOpGq1D7Mr834GSYAsfk7/pmVd0xhjQTQ+aG6imlADvX+NvLyzW9wsm/Po8HrsqXjtjMSCWFK4Mq/Vi/n85fQZ2tY/JI4P4JFso3MJyW6b+SzUqk02/vCFxNh49HDqdNOM/isXzHZLBKznx87ZAhtWEyQ==</Modulus><Exponent>AQAB</Exponent><P>7j1ZVm+NztchRMvZVoLTpd9PqzYCHmRbdc/p/OmCYrt5n1R9oogEC0B1P4Ftn/vnsUv1x2Hfd1JsXRBxm+cuhDqG+fGPMNrjTylwrjQIWNGi4nU8+HRKhELVbSXyytdACCkPXhHlTcSh8DULWj5c2DRNuq+7t/DA/yJHfj6kZQM=</P><Q>46wTW4IJQePT82NiWRC9vjdAa8mNjCxwpmPnRQpfyZrA5Kv6lXwDBZLbF7FlbrRtKDjMXBhopfHclD2Ptt2pNIN0mDlGSdulBnEfoI5J9D2tFDM71KUvo+Pov3zJzYFgudHTVjIGstfKY7vFQ9cKoSMhUCk74+kLTWx9pwUEB0M=</Q><DP>0FNdN7Q8sM52DQ6kcn3cTYPEytZja+geAhtZZQtcNLHzXFwpsGJ4Bs7KuEmZbvh8VhEyTrNLEbBFievDLK0vg/kZ5p5QtEvz0VlPa0Wctu46HQSf2DL4pkc7EkLI2I75MymUgpHrjeGQKdh4oOU/kt/JyKG66NHiMjfrhH2Ki7k=</DP><DQ>crIlrlv8Pe2AwYS37xa4ADPliGepn4xj0+9Qez+WsgH8WzcorlYjT5OEPJNp/jN3KmQyz9KcJb2K2kOJOx7AN2xiOK3h8yNsJ6qlvETyjNDleN+2rJHrRi1y+8TqBVXAXNyQE0Rwi3iaxVO7NUoIWNVR2whq245z2zVkt9eXB/E=</DQ><InverseQ>jq/ZuTmuGXFk/Z6RQ5qBIC+6gdIfCcbitb51CclneaHmCBdDxxOmAyiWTYCOcjeRFP6fM9K+OHv+jRyUED788vq2FCmRSbKiFQ43JafLzUIrjswwjqMMw/gAq7hBeCtR0EJGs1Mhc37m9/7WulLRq7EL4r+CjgRTdnzazRqrbIQ=</InverseQ><D>ejOsMz8Fkm/cWLbgqTVXo893q8BzC0V2lZ9ipUPpwHgQ2Izpg78DwJbIg0K4W71T3ImSqjbk4PdR49HGvXO89LvyXgc4wFmso5w1rUpCWpdQ12ICnacY7PSm5UNguVCHLPPt6AfWGkj/Rrnx/Ii71PcIC+S/lPZAj/VeGVlFKLoqv5JlXqpKATsNHbdGhui4U6ocKmunijwA/3sUh0sWCOpdOC7rBlyFCZPdnLggOkErs1G9Wf0H0112QDKf1hl0ytqTikFd1cbjDTtUaQDWcMuos0z6s1iY12JZcFDJIgNU3GwI/nkcC5lgWajV+6ug4iWC62bsPC3YUgL57WHKHQ==</D></RSAKeyValue>");
            xmlKeys = rsa.ToXmlString(true);
            xmlPublicKey = rsa.ToXmlString(false);
        }
        #endregion
        #region RSA的加密函数 
        //############################################################################## 
        //RSA 方式加密 
        //说明KEY必须是XML的行式,返回的是字符串 
        //在有一点需要说明！！该加密方式有 长度 限制的！！ 
        //############################################################################## 
        //RSA的加密函数 string
        public string RSAEncrypt(string xmlPublicKey, string m_strEncryptString)
        {

            byte[] PlainTextBArray;
            byte[] CypherTextBArray;
            string Result;
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            rsa.FromXmlString(xmlPublicKey);
            PlainTextBArray = (new UnicodeEncoding()).GetBytes(m_strEncryptString);
            CypherTextBArray = rsa.Encrypt(PlainTextBArray, false);
            Result = Convert.ToBase64String(CypherTextBArray);
            return Result;

        }
        //RSA的加密函数 byte[]
        public string RSAEncrypt(string xmlPublicKey, byte[] EncryptString)
        {

            byte[] CypherTextBArray;
            string Result;
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            rsa.FromXmlString(xmlPublicKey);
            CypherTextBArray = rsa.Encrypt(EncryptString, false);
            Result = Convert.ToBase64String(CypherTextBArray);
            return Result;

        }
        #endregion
        #region RSA的解密函数 
        //RSA的解密函数 string
        public string RSADecrypt(string xmlPrivateKey, string m_strDecryptString)
        {
            byte[] PlainTextBArray;
            byte[] DypherTextBArray;
            string Result;
            System.Security.Cryptography.RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            rsa.FromXmlString(xmlPrivateKey);
            PlainTextBArray = Convert.FromBase64String(m_strDecryptString);
            DypherTextBArray = rsa.Decrypt(PlainTextBArray, false);
            Result = (new UnicodeEncoding()).GetString(DypherTextBArray);
            return Result;

        }
        //RSA的解密函数 byte
        public string RSADecrypt(string xmlPrivateKey, byte[] DecryptString)
        {
            byte[] DypherTextBArray;
            string Result;
            System.Security.Cryptography.RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            rsa.FromXmlString(xmlPrivateKey);
            DypherTextBArray = rsa.Decrypt(DecryptString, false);
            Result = (new UnicodeEncoding()).GetString(DypherTextBArray);
            return Result;

        }
        #endregion
        #endregion
        #region RSA数字签名 
        #region 获取Hash描述表 
        //获取Hash描述表 ，sharejs.com
        public bool GetHash(string m_strSource, ref byte[] HashData)
        {
            //从字符串中取得Hash描述 
            byte[] Buffer;
            System.Security.Cryptography.HashAlgorithm MD5 = System.Security.Cryptography.HashAlgorithm.Create("MD5");
            Buffer = System.Text.Encoding.GetEncoding("utf-8").GetBytes(m_strSource);
            HashData = MD5.ComputeHash(Buffer);
            return true;
        }
        //获取Hash描述表 
        public bool GetHash(string m_strSource, ref string strHashData)
        {

            //从字符串中取得Hash描述 
            byte[] Buffer;
            byte[] HashData;
            System.Security.Cryptography.HashAlgorithm MD5 = System.Security.Cryptography.HashAlgorithm.Create("MD5");
            Buffer = System.Text.Encoding.GetEncoding("utf-8").GetBytes(m_strSource);
            HashData = MD5.ComputeHash(Buffer);
            strHashData = Convert.ToBase64String(HashData);
            return true;

        }
        //获取Hash描述表 
        public bool GetHash(System.IO.FileStream objFile, ref byte[] HashData)
        {

            //从文件中取得Hash描述 
            System.Security.Cryptography.HashAlgorithm MD5 = System.Security.Cryptography.HashAlgorithm.Create("MD5");
            HashData = MD5.ComputeHash(objFile);
            objFile.Close();
            return true;

        }
        //获取Hash描述表 
        public bool GetHash(System.IO.FileStream objFile, ref string strHashData)
        {

            //从文件中取得Hash描述 
            byte[] HashData;
            System.Security.Cryptography.HashAlgorithm MD5 = System.Security.Cryptography.HashAlgorithm.Create("MD5");
            HashData = MD5.ComputeHash(objFile);
            objFile.Close();
            strHashData = Convert.ToBase64String(HashData);
            return true;

        }
        #endregion
        #region RSA签名 
        //RSA签名 
        public bool SignatureFormatter(string p_strKeyPrivate, byte[] HashbyteSignature, ref byte[] EncryptedSignatureData)
        {

            System.Security.Cryptography.RSACryptoServiceProvider RSA = new System.Security.Cryptography.RSACryptoServiceProvider();
            RSA.FromXmlString(p_strKeyPrivate);
            System.Security.Cryptography.RSAPKCS1SignatureFormatter RSAFormatter = new System.Security.Cryptography.RSAPKCS1SignatureFormatter(RSA);
            //设置签名的算法为MD5 
            RSAFormatter.SetHashAlgorithm("MD5");
            //执行签名 
            EncryptedSignatureData = RSAFormatter.CreateSignature(HashbyteSignature);
            return true;

        }
        //RSA签名 
        public bool SignatureFormatter(string p_strKeyPrivate, byte[] HashbyteSignature, ref string m_strEncryptedSignatureData)
        {

            byte[] EncryptedSignatureData;
            System.Security.Cryptography.RSACryptoServiceProvider RSA = new System.Security.Cryptography.RSACryptoServiceProvider();
            RSA.FromXmlString(p_strKeyPrivate);
            System.Security.Cryptography.RSAPKCS1SignatureFormatter RSAFormatter = new System.Security.Cryptography.RSAPKCS1SignatureFormatter(RSA);
            //设置签名的算法为MD5 
            RSAFormatter.SetHashAlgorithm("MD5");
            //执行签名 
            EncryptedSignatureData = RSAFormatter.CreateSignature(HashbyteSignature);
            m_strEncryptedSignatureData = Convert.ToBase64String(EncryptedSignatureData);
            return true;

        }
        //RSA签名 
        public bool SignatureFormatter(string p_strKeyPrivate, string m_strHashbyteSignature, ref byte[] EncryptedSignatureData)
        {

            byte[] HashbyteSignature;
            HashbyteSignature = Convert.FromBase64String(m_strHashbyteSignature);
            System.Security.Cryptography.RSACryptoServiceProvider RSA = new System.Security.Cryptography.RSACryptoServiceProvider();
            RSA.FromXmlString(p_strKeyPrivate);
            System.Security.Cryptography.RSAPKCS1SignatureFormatter RSAFormatter = new System.Security.Cryptography.RSAPKCS1SignatureFormatter(RSA);
            //设置签名的算法为MD5 
            RSAFormatter.SetHashAlgorithm("MD5");
            //执行签名 
            EncryptedSignatureData = RSAFormatter.CreateSignature(HashbyteSignature);
            return true;

        }
        //RSA签名 
        public bool SignatureFormatter(string p_strKeyPrivate, string m_strHashbyteSignature, ref string m_strEncryptedSignatureData)
        {

            byte[] HashbyteSignature;
            byte[] EncryptedSignatureData;
            HashbyteSignature = Convert.FromBase64String(m_strHashbyteSignature);
            System.Security.Cryptography.RSACryptoServiceProvider RSA = new System.Security.Cryptography.RSACryptoServiceProvider();
            RSA.FromXmlString(p_strKeyPrivate);
            System.Security.Cryptography.RSAPKCS1SignatureFormatter RSAFormatter = new System.Security.Cryptography.RSAPKCS1SignatureFormatter(RSA);
            //设置签名的算法为MD5 
            RSAFormatter.SetHashAlgorithm("MD5");
            //执行签名 
            EncryptedSignatureData = RSAFormatter.CreateSignature(HashbyteSignature);
            m_strEncryptedSignatureData = Convert.ToBase64String(EncryptedSignatureData);
            return true;

        }
        #endregion
        #region RSA 签名验证 
        public bool SignatureDeformatter(string p_strKeyPublic, byte[] HashbyteDeformatter, byte[] DeformatterData)
        {

            System.Security.Cryptography.RSACryptoServiceProvider RSA = new System.Security.Cryptography.RSACryptoServiceProvider();
            RSA.FromXmlString(p_strKeyPublic);
            System.Security.Cryptography.RSAPKCS1SignatureDeformatter RSADeformatter = new System.Security.Cryptography.RSAPKCS1SignatureDeformatter(RSA);
            //指定解密的时候HASH算法为MD5 
            RSADeformatter.SetHashAlgorithm("MD5");
            if (RSADeformatter.VerifySignature(HashbyteDeformatter, DeformatterData))
            {
                return true;
            }
            else
            {
                return false;
            }

        }
        public bool SignatureDeformatter(string p_strKeyPublic, string p_strHashbyteDeformatter, byte[] DeformatterData)
        {

            byte[] HashbyteDeformatter;
            HashbyteDeformatter = Convert.FromBase64String(p_strHashbyteDeformatter);
            System.Security.Cryptography.RSACryptoServiceProvider RSA = new System.Security.Cryptography.RSACryptoServiceProvider();
            RSA.FromXmlString(p_strKeyPublic);
            System.Security.Cryptography.RSAPKCS1SignatureDeformatter RSADeformatter = new System.Security.Cryptography.RSAPKCS1SignatureDeformatter(RSA);
            //指定解密的时候HASH算法为MD5 
            RSADeformatter.SetHashAlgorithm("MD5");
            if (RSADeformatter.VerifySignature(HashbyteDeformatter, DeformatterData))
            {
                return true;
            }
            else
            {
                return false;
            }

        }
        public bool SignatureDeformatter(string p_strKeyPublic, byte[] HashbyteDeformatter, string p_strDeformatterData)
        {

            byte[] DeformatterData;
            System.Security.Cryptography.RSACryptoServiceProvider RSA = new System.Security.Cryptography.RSACryptoServiceProvider();
            RSA.FromXmlString(p_strKeyPublic);
            System.Security.Cryptography.RSAPKCS1SignatureDeformatter RSADeformatter = new System.Security.Cryptography.RSAPKCS1SignatureDeformatter(RSA);
            //指定解密的时候HASH算法为MD5 
            RSADeformatter.SetHashAlgorithm("MD5");
            DeformatterData = Convert.FromBase64String(p_strDeformatterData);
            if (RSADeformatter.VerifySignature(HashbyteDeformatter, DeformatterData))
            {
                return true;
            }
            else
            {
                return false;
            }

        }
        public bool SignatureDeformatter(string p_strKeyPublic, string p_strHashbyteDeformatter, string p_strDeformatterData)
        {

            byte[] DeformatterData;
            byte[] HashbyteDeformatter;
            HashbyteDeformatter = Convert.FromBase64String(p_strHashbyteDeformatter);
            System.Security.Cryptography.RSACryptoServiceProvider RSA = new System.Security.Cryptography.RSACryptoServiceProvider();
            RSA.FromXmlString(p_strKeyPublic);
            System.Security.Cryptography.RSAPKCS1SignatureDeformatter RSADeformatter = new System.Security.Cryptography.RSAPKCS1SignatureDeformatter(RSA);
            //指定解密的时候HASH算法为MD5 
            RSADeformatter.SetHashAlgorithm("MD5");
            DeformatterData = Convert.FromBase64String(p_strDeformatterData);
            if (RSADeformatter.VerifySignature(HashbyteDeformatter, DeformatterData))
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        #endregion

        #endregion
    }
}
