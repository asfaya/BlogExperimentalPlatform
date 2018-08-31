namespace BlogExperimentalPlatform.Utils
{
    using System.Security.Cryptography;
    using System.Text;

    public static class StringUtils
    {
        public static string MD5Hash(this string input)
        {
            using (var md5 = MD5.Create())
            {
                var result = md5.ComputeHash(Encoding.ASCII.GetBytes(input));
                var sb = new StringBuilder();

                for (int i = 0; i < result.Length; i++)
                    sb.Append(result[i].ToString("x2"));

                return sb.ToString();
            }
        }
    }
}
