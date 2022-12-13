namespace PTO_Server.Models
{
    public class AuthenticatedResponse
    {
        public string? Token { get; set; }
        public string? RefreshToken { get; set; }
        public string? UserName { get; set; }
        public bool? found { get; set; }
    }
    public class GenericRespomse
    {
        public Object? Result { get; set; }

    }
}
