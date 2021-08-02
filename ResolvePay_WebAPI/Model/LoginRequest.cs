using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ResolvePay_WebAPI.Model
{
    public class LoginRequest
    {
        [Required]
        [JsonPropertyName("username")]
        public string UserName { get; set; }

        [Required]
        [JsonPropertyName("password")]
        public string Password { get; set; }
    }


    public class LoginResult
    {
        [JsonPropertyName("username")]
        public string UserName { get; set; }

        [JsonPropertyName("role")]
        public string Role { get; set; }

        [JsonPropertyName("refreshToken")]
        public string RefreshToken { get; set; } 
        
        [JsonPropertyName("accessToken")]
        public string AccessToken { get; set; }
    }
     public class GetDetails
    {
    
        [JsonPropertyName("empname")]
        public string Employeename { get; set; } 

        [JsonPropertyName("employeenumber")]
        public string Employeenumber { get; set; }

        [JsonPropertyName("roleid")]
        public string roleid { get; set; }

        [JsonPropertyName("doj")]
        public string DOJ { get; set; }
        
        [JsonPropertyName("accessToken")]
        public string AccessToken { get; set; }
    }

    public class RefreshTokenRequest
    {
        [JsonPropertyName("refreshToken")]
        public string RefreshToken { get; set; }
    }

}