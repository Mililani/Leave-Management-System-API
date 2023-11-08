using System.Security.Cryptography;
using System.Text;

namespace LeaveManagmentAPI.PasHashs
{
    public class MyPasswordHash
    {
        public static string hashPassword(string password)
        {
            var sha = SHA256.Create();
            var asBtyArr = Encoding.Default.GetBytes(password);
            var hasPass = sha.ComputeHash(asBtyArr);
            return Convert.ToBase64String(hasPass);
        }
    }
}
