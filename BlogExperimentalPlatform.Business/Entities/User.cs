namespace BlogExperimentalPlatform.Business.Entities
{
    public class User : Entity
    {
        public string UserName { get; set; }

        public string Password { get; set; }

        public string FullName { get; set; }
    }
}
