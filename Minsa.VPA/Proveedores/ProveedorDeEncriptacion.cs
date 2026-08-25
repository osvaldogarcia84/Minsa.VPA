using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace Minsa.VPA.Proveedores
{
    public class ProveedorDeEncriptacion
    {
        private readonly byte[] _key =
      {
            53, 222, 11, 27, 209, 162, 85, 19, 29, 196, 112, 26, 173, 24, 45, 17, 144, 175, 37, 123,
            236, 24, 217, 24, 218, 209, 184, 241, 131, 114, 26, 53
        };

        private readonly byte[] _vector = { 32, 114, 23, 79, 112, 111, 3, 231, 119, 64, 146, 252, 113, 191, 121, 156 };

        private readonly ICryptoTransform _encryptorTransform;
        private readonly ICryptoTransform _decryptorTransform;
        private readonly System.Text.UTF8Encoding _utfEncoder;

        public ProveedorDeEncriptacion()
        {
            var rm = new RijndaelManaged();
            _encryptorTransform = rm.CreateEncryptor(_key, _vector);
            _decryptorTransform = rm.CreateDecryptor(_key, _vector);
            _utfEncoder = new System.Text.UTF8Encoding();
        }

        public string EncryptToString(string textValue)
        {
            return ByteArrToString(Encrypt(textValue));
        }

        public byte[] Encrypt(string textValue)
        {
            Byte[] bytes = _utfEncoder.GetBytes(textValue);
            var memoryStream = new MemoryStream();
            var cs = new CryptoStream(memoryStream, _encryptorTransform, CryptoStreamMode.Write);
            cs.Write(bytes, 0, bytes.Length);
            cs.FlushFinalBlock();

            memoryStream.Position = 0;
            var encrypted = new byte[memoryStream.Length];
            memoryStream.Read(encrypted, 0, encrypted.Length);

            cs.Close();
            memoryStream.Close();

            return encrypted;
        }

        public string DecryptString(string encryptedString)
        {
            return Decrypt(StrToByteArray(encryptedString));
        }

        public string Decrypt(byte[] encryptedValue)
        {
            var encryptedStream = new MemoryStream();
            var decryptStream = new CryptoStream(encryptedStream, _decryptorTransform, CryptoStreamMode.Write);
            decryptStream.Write(encryptedValue, 0, encryptedValue.Length);
            decryptStream.FlushFinalBlock();

            encryptedStream.Position = 0;
            var decryptedBytes = new Byte[encryptedStream.Length];
            encryptedStream.Read(decryptedBytes, 0, decryptedBytes.Length);
            encryptedStream.Close();

            return _utfEncoder.GetString(decryptedBytes);
        }

        public byte[] StrToByteArray(string str)
        {
            if (str.Length == 0)
                throw new Exception("Invalid string value in StrToByteArray");

            var byteArr = new byte[str.Length / 3];
            int i = 0;
            int j = 0;
            do
            {
                byte val = byte.Parse(str.Substring(i, 3));
                byteArr[j++] = val;
                i += 3;
            }
            while (i < str.Length);
            return byteArr;
        }

        public string ByteArrToString(byte[] byteArr)
        {
            string tempStr = "";
            for (int i = 0; i <= byteArr.GetUpperBound(0); i++)
            {
                byte val = byteArr[i];
                if (val < 10)
                    tempStr += "00" + val;
                else if (val < 100)
                    tempStr += "0" + val;
                else
                    tempStr += val.ToString(CultureInfo.InvariantCulture);
            }
            return tempStr;
        }
    }
}