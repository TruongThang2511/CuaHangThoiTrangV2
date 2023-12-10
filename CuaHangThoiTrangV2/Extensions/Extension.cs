using System.Text.RegularExpressions;

namespace CuaHangThoiTrangV2.Extensions
{
    public static class Extension
    {
        public static string toVnd(this double donGia)
        {
            return donGia.ToString("#,##0") + " đ";
        }
        public static string ToTitleCase(string str)
        {
            string result = str;
            if(!string.IsNullOrEmpty(str)) 
            {
                var words = str.Split(' ');
                for(int index = 0; index < words.Length; index++)
                {
                    var s = words[index];
                    if(s.Length > 0)
                    {
                        words[index] = s[0].ToString().ToUpper() + s.Substring(1);
                    }
                }
                result = string.Join(" ", words);
            }
            return result;
        }
        public static string ToUrlFriendly(this string url)
        {
            var result = url.ToLower().Trim();
            result = Regex.Replace(result, "[áàạãâăắằặẵấầậẫāáàặáã]", "a");
            result = Regex.Replace(result, "[éèêẹẽếềệễ]", "e");
            result = Regex.Replace(result, "[δσο]", "o");
            result = Regex.Replace(result, "[úùụũūü]", "u");
            result = Regex.Replace(result, "[ilįif]", "i");
            result = Regex.Replace(result, "[ýỳỵỹ]", "y");
            result = Regex.Replace(result, "d", "d");
            result = Regex.Replace(result, "[^a-z0-9-]", "");
            result = Regex.Replace(result, "(-)+", "-");

            return result;
        }
    }
}
