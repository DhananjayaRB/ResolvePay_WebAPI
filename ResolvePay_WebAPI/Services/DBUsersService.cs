using System;
using System.Collections.Generic;
using System.Data;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MySqlConnector;
using static ResolvePay_WebAPI.Startup;

namespace ResolvePay_WebAPI.Services
{
    
    public interface IDBUserService
    {
        bool IsValidUserCredentials(string userName, string password);
        string GetUserRole(string userName);
        string GetMd5Hash(string str);
    }
 
    public class DBUserService : IDBUserService
    {
        private readonly ILogger<DBUserService> _logger;

        private ConnectionStringsConfig _connectionStrings;

        MySqlConnection con;
        private IDictionary<string, string> _users;
        public DBUserService(ILogger<DBUserService> logger, IOptionsSnapshot<ConnectionStringsConfig> connectionStrings)
        {
            _logger = logger;
            _connectionStrings = connectionStrings?.Value ?? throw new ArgumentNullException(nameof(connectionStrings));
        }
         

        public bool IsValidUserCredentials(string userName, string password)
        {
            if (string.IsNullOrWhiteSpace(userName))
            {
                return false;
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                return false;
            }
             var con = new MySqlConnection(_connectionStrings.DefaultConnection);
            //con = new MySqlConnection("server=localhost;user=root;password=;database=customerdb_terex"); //( connstr_config);
            con.Open();

            var encodedPassword = GetMd5Hash(password);

            MySqlDataAdapter sql = new MySqlDataAdapter("SELECT * FROM empmaster", con);
            //MySqlDataAdapter sql = new MySqlDataAdapter("SELECT * FROM empmaster e WHERE e.Email= '" + userName + "' AND e.Password= '" + encodedPassword + "' LIMIT 1;", con);
            DataTable data = new DataTable();
            sql.Fill(data);

            if (data.Rows.Count > 0)
            {
                foreach (DataRow item in data.Rows)
                    _users = new Dictionary<string, string>
        {
            { item["Email"].ToString(),item["Password"].ToString() },
        };

            }
            else
            {

            }
            con.Close();

            return _users.TryGetValue(userName, out var p) && p == password;
        }
/*

        public bool IsAnExistingUser(string userName)
        {
            return _users.ContainsKey(userName);
        }
*/
        public string GetUserRole(string userName)
        {
           /* if (!IsAnExistingUser(userName))
            {
                return string.Empty;
            }*/

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

}
