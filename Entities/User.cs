namespace IdentityManagement.Entities
{
    public class User
    {
        public string Name { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public string[] MethodsAllowed { get; set; }
    }
}
