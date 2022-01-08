using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.IdentityModel.JsonWebTokens;
using System.Drawing;

namespace WebApp1.Codes
{
    #region Genel Extensions

    public static class MyExtensions
    {
        #region Exception için

        public static Exception MyLastInner(this Exception _ex)
        {
            if (_ex.InnerException == null) return _ex;
            return _ex.InnerException.MyLastInner();
        }

        #endregion

        #region string işlemler

        public static string MyToStr(this object _value)
        {
            return _value == null ? "" : _value.ToString();
        }

        public static string MyToTrim(this object _value)
        {
            return _value == null ? "" : _value.ToString().Trim();
        }

        public static string MyToStrAntiInjection(this string _str)
        {
            string rV = "";
            if (!string.IsNullOrEmpty(_str))
            {
                rV = _str.Replace("+", "").Replace("'", "").Replace("-", "").Replace("\"", "");
            }

            return rV;
        }

        public static string MyToMaxLength(this string _str, int _len)
        {
            if (!String.IsNullOrEmpty(_str))
            {
                if (_str.Length > _len)
                {
                    _str = _str.Substring(0, _len);
                }
            }

            return _str;
        }

        public static string MyToLatinString(this string _str)
        {
            //burası Culture e göre değişebilir olacak
            //tr-TR
            char[] chars1 = { 'Ç', 'Ğ', 'İ', 'Ş', 'Ö', 'Ü', 'Â' };
            char[] chars2 = { 'C', 'G', 'I', 'S', 'O', 'U', 'A' };

            if (!String.IsNullOrEmpty(_str))
            {
                _str = _str.Trim().ToUpper();

                char[] charArray = _str.ToCharArray();

                for (int i = 0; i < charArray.Length; i++)
                {
                    for (int k = 0; k < chars1.Length; k++)
                    {
                        if (charArray[i] == chars1[k])
                        {
                            charArray[i] = chars2[k];
                        }
                    }
                }

                _str = new string(charArray);
            }
            return _str;
        }

        public static string MyToLower(this string _str)
        {
            if (!String.IsNullOrEmpty(_str))
            {
                _str = _str.Trim().ToLower();
            }
            return _str;
        }

        public static string MyToUpper(this string _str)
        {
            if (!String.IsNullOrEmpty(_str))
            {
                _str = _str.Trim().ToUpper();
            }
            return _str;
        }

        public static string MyToTitleCase(this string _str)
        {
            if (!String.IsNullOrEmpty(_str))
            {
                _str = _str.Trim();
                if (_str.Length > 0)
                {
                    //_str = System.Globalization.CultureInfo.DefaultThreadCurrentCulture.TextInfo.ToTitleCase(_str);
                    //dnx core TextInfo.ToTitleCase i destekleyince aşağıyı silip üstekini kullan
                    String[] words = _str.Split(' ');
                    for (int i = 0; i < words.Length; i++)
                    {
                        if (words[i].Length == 0) continue;

                        Char firstChar = Char.ToUpper(words[i][0]);
                        String rest = "";
                        if (words[i].Length > 1)
                        {
                            rest = words[i][1..].ToLower();
                        }
                        words[i] = firstChar + rest;
                    }
                    _str = String.Join(" ", words);
                }
            }
            return _str;
        }

        public static string MyStrToFirstStar(this string _str)
        {
            if (!String.IsNullOrEmpty(_str))
            {
                _str = _str.Trim();
                if (_str.Length > 0)
                {

                    String[] words = _str.Split(' ');
                    for (int i = 0; i < words.Length; i++)
                    {
                        if (words[i].Length == 0) continue;

                        Char firstChar = words[i][0];
                        String rest = "";
                        if (words[i].Length > 1)
                        {
                            rest = "".PadRight(words[i][1..].Length, '*');
                        }
                        words[i] = firstChar + rest;
                    }
                    _str = String.Join(" ", words);
                }
            }
            return _str;
        }

        public static string MyMoonToStr(this string _str)
        {
            //01-12 ye kadar olan harf olarak döner
            String rValue = "X";
            try
            {
                char[] chars = { 'X', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'K', 'L', 'M', 'N' };

                if (!String.IsNullOrEmpty(_str))
                {
                    rValue = chars[_str.MyToInt()].ToString();
                }
            }
            catch { }

            return rValue;
        }

        public static string MyToReverseStr(this string _str)
        {
            char[] charArray = _str.MyToStr().ToCharArray();
            Array.Reverse(charArray);
            return new string(charArray);
        }

        #endregion

        #region ad soyad split 
        public static string MyToSurname(this string _nameSurname)
        {
            return _nameSurname.Split(' ').LastOrDefault();
        }
        public static string MyToName(this string _nameSurname)
        {
            return _nameSurname.Replace(_nameSurname.MyToSurname(), "");
        }
        #endregion

        #region xml
        public static string MyObjectToXmlStr<T>(this T value)
        {
            //kullanım >>> var xmlString = obj.Serialize();
            string rV = "";

            if (value == null)
            {
                rV = string.Empty;
            }
            try
            {
                var xmlserializer = new System.Xml.Serialization.XmlSerializer(typeof(T));
                var stringWriter = new System.IO.StringWriter();
                using var writer = System.Xml.XmlWriter.Create(stringWriter);
                xmlserializer.Serialize(writer, value);
                rV = stringWriter.ToString();

            }
            catch { }

            return rV;
        }
        #endregion

        #region int
        public static int MyToInt(this string _str)
        {
            int rValue = 0;
            if (!String.IsNullOrEmpty(_str))
            {
                _ = int.TryParse(_str, out rValue);
            }
            return rValue;
        }

        public static int MyToInt(this object _value)
        {
            int rValue = 0;
            if (_value != null)
            {
                try
                {
                    rValue = Convert.ToInt32(_value);
                }
                catch { }
            }
            return rValue;
        }

        #endregion

        #region decimal
        public static decimal MyToDecimal(this string _str)
        {
            decimal rValue = 0;
            if (!String.IsNullOrEmpty(_str))
            {
                _ = decimal.TryParse(_str, out rValue);
            }
            return rValue;
        }

        public static decimal MyToDecimal(this object _value)
        {
            decimal rValue = 0;
            if (_value != null)
            {
                try
                {
                    rValue = Convert.ToDecimal(_value);
                }
                catch { }
            }
            return rValue;
        }
        #endregion

        #region Roman
        public static string MyToRoman(this int num)
        {
            string[] thou = { "", "M", "MM", "MMM" };
            string[] hun = { "", "C", "CC", "CCC", "CD", "D", "DC", "DCC", "DCCC", "CM" };
            string[] ten = { "", "X", "XX", "XXX", "XL", "L", "LX", "LXX", "LXXX", "XC" };
            string[] ones = { "", "I", "II", "III", "IV", "V", "VI", "VII", "VIII", "IX" };
            string roman = "";
            roman += thou[(int)(num / 1000) % 10];
            roman += hun[(int)(num / 100) % 10];
            roman += ten[(int)(num / 10) % 10];
            roman += ones[num % 10];
            return roman;
        }

        public static int MyRomanToInt(this string roman)
        {
            Dictionary<char, int> RomanMap = new() { { 'I', 1 }, { 'V', 5 }, { 'X', 10 }, { 'L', 50 }, { 'C', 100 }, { 'D', 500 }, { 'M', 1000 } };
            int number = 0;
            char previousChar = roman[0];
            foreach (char currentChar in roman)
            {
                number += RomanMap[currentChar];
                if (RomanMap[previousChar] < RomanMap[currentChar])
                {
                    number -= RomanMap[previousChar] * 2;
                }
                previousChar = currentChar;
            }
            return number;
        }
        #endregion

        #region Cryptography
        public static string MyGetMD5HashFromFile(this string _fileName)
        {
            string rV = "";
            if (!String.IsNullOrEmpty(_fileName))
            {
                if (System.IO.File.Exists(_fileName))
                {
                    using var md5 = System.Security.Cryptography.MD5.Create();
                    using var stream = System.IO.File.OpenRead(_fileName);
                    rV = BitConverter.ToString(md5.ComputeHash(stream)).Replace("-", string.Empty);
                }
            }
            return rV;
        }

        public static string MyToMD5(this byte[] _byteArray)
        {
            string rV = "";
            if (_byteArray != null)
            {
                MD5 MD5Pass = MD5.Create();
                byte[] MD5Buff = MD5Pass.ComputeHash(_byteArray);
                rV = BitConverter.ToString(MD5Buff).Replace("-", string.Empty);
            }
            return rV;
        }

        public static string MyToMD5(this string _str)
        {
            if (!String.IsNullOrEmpty(_str))
            {
                MD5 MD5Pass = MD5.Create();
                byte[] MD5Buff = MD5Pass.ComputeHash(Encoding.UTF8.GetBytes(_str));
                _str = BitConverter.ToString(MD5Buff).Replace("-", string.Empty);
            }
            return _str;
        }

        public static string MyToBase64Str(this string _str)
        {
            return Convert.ToBase64String(Encoding.Unicode.GetBytes(_str.MyToStr()));
        }

        public static string MyFromBase64Str(this string _str)
        {
            return Encoding.Unicode.GetString(Convert.FromBase64String(_str.MyToStr()));
        }

        public static string MyToEncrypt(this string _str, string _key, MyCipher.EnmSCType enmSCType, Encoding encoding)
        {
            return new MyCipher().Encrypt(_str.MyToStr(), _key.MyToStr(), enmSCType, encoding);
        }

        public static string MyToDecrypt(this string _str, string _key, MyCipher.EnmSCType enmSCType, Encoding encoding)
        {
            return new MyCipher().Decrypt(_str.MyToStr(), _key.MyToStr(), enmSCType, encoding);
        }

        public static string MyToEncrypt(this string _str, string _key)
        {
            return new MyCipher().Encrypt(_str.MyToStr(), _key, MyCipher.EnmSCType.Hex, Encoding.UTF8);
        }

        public static string MyToDecrypt(this string _str, string _key)
        {
            return new MyCipher().Decrypt(_str.MyToStr(), _key, MyCipher.EnmSCType.Hex, Encoding.UTF8);
        }

        public static string MyToEncryptPassword(this string _str)
        {
            return new MyCipher().Encrypt(_str.MyToStr(), "6856", MyCipher.EnmSCType.Hex, Encoding.UTF8);
        }

        public static string MyToDecryptPassword(this string _str)
        {
            return new MyCipher().Decrypt(_str.MyToStr(), "6856", MyCipher.EnmSCType.Hex, Encoding.UTF8);
        }

        #endregion

        #region Hex
        public static string MyStrToHex(this object _value, Encoding _encoding)
        {
            var sb = new StringBuilder();

            var bytes = _encoding.GetBytes(_value.MyToStr());
            foreach (var t in bytes)
            {
                sb.Append(t.ToString("X2"));
            }

            return sb.ToString();
        }

        public static string MyHexToStr(this object _value, Encoding _encoding)
        {
            string rValue = _value.MyToStr();

            var bytes = new byte[rValue.Length / 2];
            for (var i = 0; i < bytes.Length; i++)
            {
                bytes[i] = Convert.ToByte(rValue.Substring(i * 2, 2), 16);
            }

            rValue = _encoding.GetString(bytes);

            return rValue;
        }
        #endregion

        #region enum ex
        public static string MyGetDescription(this Enum GenericEnum)
        {
            Type genericEnumType = GenericEnum.GetType();
            System.Reflection.MemberInfo[] memberInfo = genericEnumType.GetMember(GenericEnum.ToString());
            if ((memberInfo != null && memberInfo.Length > 0))
            {
                var _Attribs = memberInfo[0].GetCustomAttributes(typeof(System.ComponentModel.DescriptionAttribute), false);
                if ((_Attribs != null && _Attribs.Length > 0))
                {
                    return ((System.ComponentModel.DescriptionAttribute)_Attribs.ElementAt(0)).Description;
                }
            }
            return GenericEnum.ToString();
        }

        #endregion 

        #region Validasyon
        public static bool IsValidTCKimlikNo(this string kimlikNo)
        {
            int Algoritma_Adim_Kontrol = 0, TekBasamaklarToplami = 0, CiftBasamaklarToplami = 0;

            if (kimlikNo.Length == 11) Algoritma_Adim_Kontrol = 1;
            foreach (char chr in kimlikNo) { if (Char.IsNumber(chr)) Algoritma_Adim_Kontrol = 2; }
            if (kimlikNo.Substring(0, 1) != "0") Algoritma_Adim_Kontrol = 3;

            int[] arrTC = System.Text.RegularExpressions.Regex.Replace(kimlikNo, "[^0-9]", "").Select(x => (int)Char.GetNumericValue(x)).ToArray();

            for (int i = 0; i < kimlikNo.Length; i++)
            {
                if (((i + 1) % 2) == 0)
                    if (i + 1 != 10) CiftBasamaklarToplami += Convert.ToInt32(arrTC[i]);
                    else
                    if (i + 1 != 11) TekBasamaklarToplami += Convert.ToInt32(arrTC[i]);
            }

            if (Convert.ToInt32(kimlikNo.Substring(9, 1)) == (((TekBasamaklarToplami * 7) - CiftBasamaklarToplami) % 10)) Algoritma_Adim_Kontrol = 4;
            if (Convert.ToInt32(kimlikNo.Substring(10, 1)) == ((arrTC.Sum() - Convert.ToInt32(kimlikNo.Substring(10, 1))) % 10)) Algoritma_Adim_Kontrol = 5;

            return (Algoritma_Adim_Kontrol == 5);
        }
        public static bool IsValidEmail(this string mailAdres)
        {
            return new EmailAddressAttribute().IsValid(mailAdres);
        }
        #endregion

        #region String i bir dönüşüme sokar, mail için kullanılacak. 
        public static string MyStrToMD5ByteSumHex(this string _value)
        {
            int values = 0;
            Encoding encoding = Encoding.UTF8;
            _value = _value.MyToMD5();
            //byte sum
            var bytes = encoding.GetBytes(_value);
            foreach (int t in bytes)
            {
                values += t;
            }
            return values.MyStrToHex(encoding).ToString();
        }
        #endregion

        #region DataURL to Image
        public static Image MyToImage(this string _value)
        {
            Image rValue = null;
            if (_value != null)
            {
                try
                {
                    var match = System.Text.RegularExpressions.Regex.Match(_value, @"data:(?<type>.+?);base64,(?<data>.+)");
                    var base64Data = match.Groups["data"].Value;
                    var contentType = match.Groups["type"].Value;
                    var imageBytes = Convert.FromBase64String(base64Data);

                    using var ms = new System.IO.MemoryStream(imageBytes, 0, imageBytes.Length);
                    ms.Write(imageBytes, 0, imageBytes.Length);
                    rValue = Image.FromStream(ms, true);
                }
                catch { }
            }
            return rValue;
        }
        #endregion

        #region Object To JsonText
        public static string MyObjToJsonText(this object _obj)
        {
            string rV = "";
            if (_obj != null)
            {
                rV = JsonConvert.SerializeObject(_obj);
            }
            return rV;
        }
        #endregion

        #region Veri dili için
        public static string MyVeriDiliToStr(this string _jsonText, string _lang, string _field, string _filedCurrentValue)
        {

            String rValue = _filedCurrentValue.MyToTrim();
            try
            {
                if (!String.IsNullOrEmpty(_jsonText))
                {
                    dynamic data = Newtonsoft.Json.Linq.JObject.Parse(_jsonText);
                    if (data[_field] != null)
                    {
                        if (data[_field][_lang] != null)
                        {
                            rValue = data[_field][_lang].ToString();
                        }
                    }

                }
            }
            catch { }

            return rValue;
        }
        #endregion
    }

    #endregion

    #region EnumerableExtensions
    public static class EnumerableExtensions
    {
        public static IEnumerable<IterationElement<T>> MyToDetailed<T>(this IEnumerable<T> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            using var enumerator = source.GetEnumerator();
            bool isFirst = true;
            bool hasNext = enumerator.MoveNext();
            int index = 0;
            while (hasNext)
            {
                T current = enumerator.Current;
                hasNext = enumerator.MoveNext();
                yield return new IterationElement<T>(index, current, isFirst, !hasNext);
                isFirst = false;
                index++;
            }
        }

        public struct IterationElement<T>
        {
            public int Index { get; }
            public bool IsFirst { get; }
            public bool IsLast { get; }
            public T Value { get; }

            public IterationElement(int index, T value, bool isFirst, bool isLast)
            {
                Index = index;
                IsFirst = isFirst;
                IsLast = isLast;
                Value = value;
            }
        }
    }
    #endregion
}
