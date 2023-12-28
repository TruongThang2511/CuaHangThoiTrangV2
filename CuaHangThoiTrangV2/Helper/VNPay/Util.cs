using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Http;

namespace CuaHangThoiTrangV2.Helper.VNPay
{
    public class Util
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public Util(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public static String HmacSHA512(string key, String inputData)
        {
            var hash = new StringBuilder();
            byte[] keyBytes = Encoding.UTF8.GetBytes(key);
            byte[] inputBytes = Encoding.UTF8.GetBytes(inputData);
            using (var hmac = new HMACSHA512(keyBytes))
            {
                byte[] hashValue = hmac.ComputeHash(inputBytes);
                foreach (var theByte in hashValue)
                {
                    hash.Append(theByte.ToString("x2"));
                }
            }

            return hash.ToString();
        }

        public static string GetIpAddress(IHttpContextAccessor httpContextAccessor)
        {
            string ipAddress;
            try
            {
                ipAddress = httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();

                if (string.IsNullOrEmpty(ipAddress) || (ipAddress.ToLower() == "unknown"))
                    ipAddress = httpContextAccessor.HttpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault();
            }
            catch (Exception ex)
            {
                ipAddress = "Invalid IP: " + ex.Message;
            }

            return ipAddress;
        }
    }
}
