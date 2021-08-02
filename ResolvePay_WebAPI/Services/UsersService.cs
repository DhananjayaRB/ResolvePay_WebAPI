using System;
using System.Collections.Generic;
using System.Data;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MySqlConnector;
using ResolvePay_WebAPI.Model;
using static ResolvePay_WebAPI.Startup;

namespace ResolvePay_WebAPI.Services
{
    public interface IUserService
    {
        bool IsAnExistingUser(string userName);
        bool IsValidUserCredentials(string userName, string password);
        GetDetails GetUserDetails(string userName);
        string GetUserRole(string userName);
    }

    public class UserService : IUserService
    {
        private readonly ILogger<UserService> _logger;

        MySqlConnection con;
        private IDictionary<string, string> _users = new Dictionary<string, string>();
        private GetDetails _userdata = new GetDetails();

        // inject your database here for user validation
        private ConnectionStringsConfig _connectionStrings;
        public UserService(ILogger<UserService> logger, IOptionsSnapshot<ConnectionStringsConfig> connectionStrings)
        {
            _logger = logger;
            _connectionStrings = connectionStrings?.Value ?? throw new ArgumentNullException(nameof(connectionStrings));
        }

        public bool IsValidUserCredentials(string userName, string password)
        {
            _logger.LogInformation($"Validating user [{userName}]");
            if (string.IsNullOrWhiteSpace(userName))
            {
                return false;
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                return false;
            }


            con = new MySqlConnection(_connectionStrings.DefaultConnection);
            //con = new MySqlConnection("server=localhost;user=root;password=;database=customerdb_terex;AllowZeroDateTime=True");
            con.Open();



            MySqlDataAdapter sql = new MySqlDataAdapter("SELECT * FROM empmaster", con);
            //MySqlDataAdapter sql = new MySqlDataAdapter("SELECT * FROM empmaster e WHERE e.Email= '" + userName + "' AND e.Password= '" + encodedPassword + "' LIMIT 1;", con);
            DataTable data = new DataTable();
            sql.Fill(data);

            if (data.Rows.Count > 0)
            {

                foreach (DataRow item in data.Rows)
                {
                    string optionKey = item["Email"].ToString();
                    if (!_users.ContainsKey(optionKey))
                        _users.Add(optionKey, item["Password"].ToString());
                }

            }
            else
            {

            }
            con.Close();


            return _users.TryGetValue(userName, out var p) && p == password;
        }

        public GetDetails GetUserDetails(string userName)
        {
            _logger.LogInformation($"Validating user [{userName}]");
            if (string.IsNullOrWhiteSpace(userName))
            {
                return null;
            }

            con = new MySqlConnection(_connectionStrings.DefaultConnection);
            //con = new MySqlConnection("server=localhost;user=root;password=;database=customerdb_terex;AllowZeroDateTime=True");
            con.Open();

            MySqlDataAdapter sql = new MySqlDataAdapter("SELECT * FROM empmaster e WHERE e.Email= '" + userName +"'" , con);
            DataTable data = new DataTable();
            sql.Fill(data);

            if (data.Rows.Count > 0)
            {

                foreach (DataRow item in data.Rows)
                {
                    _userdata.Employeenumber = item["EmpNumber"].ToString();
                    _userdata.Employeename = item["EmpFirstName"].ToString()+item["EmpMiddleName"].ToString()+item["EmpLastName"].ToString();
                    _userdata.roleid = item["RoleID"].ToString();     
                    _userdata.DOJ = item["DateOfJoining"].ToString();                    
                }

            }
            else
            {

            }
            con.Close();


            return _userdata;
        }

        public bool IsAnExistingUser(string userName)
        {
            return _users.ContainsKey(userName);
        }


        public string GetUserRole(string userName)
        {
            if (!IsAnExistingUser(userName))
            {
                return string.Empty;
            }

            if (userName == "srikanth.dg@resolveindia.com")
            {
                return UserRoles.Admin;
            }

            return UserRoles.BasicUser;
        }

        public string GetMd5Hash(string input)
        {
            using MD5 md5Hash = MD5.Create();
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));
            StringBuilder sBuilder = new StringBuilder();

            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }
            return sBuilder.ToString();
        }
    }
    /* public interface IUserService
     {
         bool IsAnExistingUser(string userName);
         bool IsValidUserCredentials(string userName, string password);
         string GetUserRole(string userName);
     }

     public class UserService : IUserService
     {
         private readonly ILogger<UserService> _logger;


         private readonly IDictionary<string, string> _users = new Dictionary<string, string>
         {
             { "test1", "password1" },
             { "test2", "password2" },
             { "admin", "securePassword" }
         };
         // inject your database here for user validation
         public UserService(ILogger<UserService> logger)
         {
             _logger = logger;
         }

         public bool IsValidUserCredentials(string userName, string password)
         {
             _logger.LogInformation($"Validating user [{userName}]");
             if (string.IsNullOrWhiteSpace(userName))
             {
                 return false;
             }

             if (string.IsNullOrWhiteSpace(password))
             {
                 return false;
             }

             return _users.TryGetValue(userName, out var p) && p == password;
         }

         public bool IsAnExistingUser(string userName)
         {
             return _users.ContainsKey(userName);
         }

         public string GetUserRole(string userName)
         {
             if (!IsAnExistingUser(userName))
             {
                 return string.Empty;
             }

             if (userName == "admin")
             {
                 return UserRoles.Admin;
             }

             return UserRoles.BasicUser;
         }
     }
    */
    public static class UserRoles
    {
        public const string Admin = nameof(Admin);
        public const string BasicUser = nameof(BasicUser);
    }
}
