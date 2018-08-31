namespace BlogExperimentalPlatform.Web.Settings
{
    public class SecuritySettings
    {
        public int TokenTimeOut { get; set; }

        public string Issuer { get; set; }

        public string Audience { get; set; }

        public string Secret { get; set; }
    }
}
