using System.Security.Cryptography;
using System.Text;

namespace Curaditos.Helpers
{
  
    public class AuthService
    {
        public static string ConvertirSha256(string texto)

        {
            StringBuilder Sb = new StringBuilder();
            //usar la referencia de "System.Security.Cryptography"
            using (SHA256 hash = SHA256Managed.Create())
            {
                Encoding enc = Encoding.UTF8;
                byte[] result = hash.ComputeHash(enc.GetBytes(texto));

                foreach (byte b in result)
                    Sb.Append(b.ToString("x2"));
            }
            return Sb.ToString();
        }
    }
}
