using System;
using System.Text;

namespace WebApp1.Codes
{
    public class MyCipher
    {
        //String Convert Type
        public enum EnmSCType
        {
            None,
            Base64,
            Hex
        }

        #region private

        private string ToStr(string s)
        {
            return s == null ? "" : s.ToString();
        }

        private string EncryptOrDecrypt(string _str, string _key)
        {
            var result = new StringBuilder();

            for (int i = 0; i < _str.Length; i++)
            {
                // take next character from string
                char character = _str[i];

                // cast to a uint
                uint charCode = (uint)character;

                // figure out which character to take from the key
                int keyPosition = i % _key.Length; // use modulo to "wrap round"

                // take the key character
                char keyChar = _key[keyPosition];

                // cast it to a uint also
                uint keyCode = (uint)keyChar;

                // perform XOR on the two character codes
                uint combinedCode = charCode ^ keyCode;

                // cast back to a char
                char combinedChar = (char)combinedCode;

                // add to the result
                result.Append(combinedChar);
            }

            return result.ToString();
        }

        #endregion

        public string Encrypt(string text, string key, EnmSCType enmSCType, Encoding encoding)
        {
            text = this.ToStr(text);
            key = this.ToStr(key);

            text = EncryptOrDecrypt(text, key);

            if (enmSCType == EnmSCType.Base64)
            {
                text = Convert.ToBase64String(encoding.GetBytes(text));
            }

            if (enmSCType == EnmSCType.Hex)
            {
                text = text.MyStrToHex(encoding);
            }

            return text;
        }

        public string Decrypt(string text, string key, EnmSCType enmSCType, Encoding encoding)
        {
            text = this.ToStr(text);
            key = this.ToStr(key);

            if (enmSCType == EnmSCType.Base64)
            {
                text = encoding.GetString(Convert.FromBase64String(text));
            }

            if (enmSCType == EnmSCType.Hex)
            {
                text = text.MyHexToStr(encoding);
            }

            text = EncryptOrDecrypt(text, key);

            return text;
        }


    }
}