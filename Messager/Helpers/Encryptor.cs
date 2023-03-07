using System.Security.Cryptography;
using System.Text;

namespace Messager.Helpers
{
    public static class Encryptor
    {
        public static string GenerateHash(string source)
        {
            var md5 = MD5.Create();

            var bytes = Encoding.ASCII.GetBytes(source);
            var hash = md5.ComputeHash(bytes);

            var sb = new StringBuilder();
            foreach (var b in hash)
            {
                sb.Append(b.ToString("X2"));
            }

            return sb.ToString();
        }
    }
}
