namespace ZahimarProject.Models
{
    public class TokenBlacklist
    {
        public int Id { get; set; }
        public string Token { get; set; }
        public DateTime ExpirationDate { get; set; }
    }
}
