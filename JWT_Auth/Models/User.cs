using System.Text.Json.Serialization;

namespace JWT_Auth.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        // when return which is json then not show password
        [JsonIgnore]
        public string Password { get; set; }
    }
}
