using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ResolvePay_WebAPI.Model
{
    public class LoginRequest_PGSQL
    {
        [Required]
        [JsonPropertyName("username")]
        public string email { get; set; }

        [Required]
        [JsonPropertyName("password")]
        public string password { get; set; }
    }

    public class GetTestDetails_PGSQL
    {
        [Key]
        [JsonPropertyName("empid")]
        public int empmasterid { get; set; } 
        
        [JsonPropertyName("empname")]
        public string empnumber { get; set; } 

        [JsonPropertyName("employeenumber")]
        public string empfirstname { get; set; }

        [JsonPropertyName("password")]
        public string password { get; set; }

        [JsonPropertyName("userid")]
        public string email { get; set; }
        [JsonPropertyName("doj")]
        public DateTime dateofjoining { get; set; }
        
    }

    public class ResultTestDetails_PGSQL
    {          
        [JsonPropertyName("EmployeeNumber")]
        public string empnumber { get; set; } 

        [JsonPropertyName("EmpName")]
        public string empfirstname { get; set; } 

        [JsonPropertyName("UserId")]
        public string email { get; set; }

        [JsonPropertyName("DateOfJoining")]
        public string dateofjoining { get; set; }
        
    }

    //Classes for MySQL Connection
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
        [JsonPropertyName("empid")]
        public string UserName { get; set; }

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
    public class GetTestDetails
    {
        [Key]
        [JsonPropertyName("empid")]
        public int ID { get; set; } 
        
        [JsonPropertyName("empname")]
        public string EmpNumber { get; set; } 

        [JsonPropertyName("photo")]
        public string Photo { get; set; }
        
        [JsonPropertyName("aadhaarnum")]
        public string AADHARNumber { get; set; }
        
        [JsonPropertyName("createdon")]
        public string CreatedOn { get; set; }

        [JsonPropertyName("modifiedon")]
        public string ModifiedOn { get; set; }

        [JsonPropertyName("status")]
        public int Status { get; set; } 
         
    }

    public class RefreshTokenRequest
    {
        [JsonPropertyName("refreshToken")]
        public string RefreshToken { get; set; }
    }

}