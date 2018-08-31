namespace BlogExperimentalPlatform.Web.Security
{
    using System.Text;
    using Microsoft.IdentityModel.Tokens;

    public static class JwtSecurityKey
    {
        public static SymmetricSecurityKey Create(string secret)
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secret));
        }
    }
}
