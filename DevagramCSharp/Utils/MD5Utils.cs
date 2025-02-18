using System.Security.Cryptography;
using System.Text;

namespace DevagramCSharp.Utils
{
    public class MD5Utils
    {
        public static string GerarHashMD5(string text)
        {
            MD5 md5hash = MD5.Create(); //cria criptografia
            var bytes = md5hash.ComputeHash(Encoding.UTF8.GetBytes(text)); //transforma o texto em bytes

            StringBuilder stringBuilder = new StringBuilder();

            for(int i = 0; i < bytes.Length; i++)
            {
                stringBuilder.Append(bytes[i]);
            }
            return stringBuilder.ToString();
        }
    }
}
