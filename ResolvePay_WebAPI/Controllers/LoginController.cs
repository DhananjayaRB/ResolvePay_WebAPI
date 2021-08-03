using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ResolvePay_WebAPI.Infrastructure;
using ResolvePay_WebAPI.Model;
using ResolvePay_WebAPI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ResolvePay_WebAPI.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class LoginController : Controller
    {
        private readonly ILogger<LoginController> _logger;
        private IUserService _userService;
        private readonly IJwtAuthManager _jwtAuthManager;

        public LoginController(ILogger<LoginController> logger, IUserService userService, IJwtAuthManager jwtAuthManager)
        {
            _logger = logger;
            _userService = userService;
            _jwtAuthManager = jwtAuthManager;
        }

        [AllowAnonymous]
        [HttpPost("loginpgsql")]
        public ActionResult Login_pgsql([FromBody] LoginRequest_PGSQL request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }
                var encodedPassword = GetMd5Hash(request.password);

                if (!_userService.IsValidUserCredentials_PGSQL(request.email, encodedPassword))
                {
                    return Unauthorized();
                }

                var role = _userService.GetUserRole(request.email);
                var claims = new[]
                {
                new Claim(ClaimTypes.Name,request.email),
                new Claim(ClaimTypes.Role, role)
            };

                var jwtResult = _jwtAuthManager.GenerateTokens(request.email, claims, DateTime.Now);
                _logger.LogInformation($"User [{request.email}] logged in the system.");
                return Ok(new LoginResult
                {
                    UserName = request.email,
                    Role = role,
                    AccessToken = jwtResult.AccessToken,
                    RefreshToken = jwtResult.RefreshToken.TokenString
                });
            }
            catch(Exception ex)
            {
                _logger.LogInformation($"Error at Login_pgsql : [{ex.Message}], [{request.email}]");
                throw;
            }
        }

        [Authorize]
        [HttpGet("GetDatapgsql")]
        public ActionResult GetData_PGSQL()
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }

                if (_userService.GetUserDetails_PGSQL(User.Identity?.Name) == null)
                {
                    return Unauthorized();
                }

                var data = _userService.GetUserDetails_PGSQL(User.Identity?.Name);

                _logger.LogInformation($"User [{User.Identity?.Name}] logged in the system.");

                return Ok(new ResultTestDetails_PGSQL
                {
                    empfirstname = data.empfirstname,
                    empnumber = data.empnumber,
                    dateofjoining = data.dateofjoining.ToString("MM-dd-yyyy"),
                    email = data.email,
                });
            }
            catch(Exception ex)
            {
                _logger.LogInformation($"Error at GetData_PGSQL : [{ex.Message}], [{User.Identity?.Name}]");
                throw;
            }
        }

        //API methods with MySQL Connection
        [AllowAnonymous]
        [HttpPost("login")]
        public ActionResult Login([FromBody] LoginRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }
                var encodedPassword = GetMd5Hash(request.Password);

                if (!_userService.IsValidUserCredentials(request.UserName, encodedPassword))
                {
                    return Unauthorized();
                }

                var role = _userService.GetUserRole(request.UserName);
                var claims = new[]
                {
                new Claim(ClaimTypes.Name,request.UserName),
                new Claim(ClaimTypes.Role, role)
            };

                var jwtResult = _jwtAuthManager.GenerateTokens(request.UserName, claims, DateTime.Now);
                _logger.LogInformation($"User [{request.UserName}] logged in the system.");
                return Ok(new LoginResult
                {
                    UserName = request.UserName,
                    Role = role,
                    AccessToken = jwtResult.AccessToken,
                    RefreshToken = jwtResult.RefreshToken.TokenString
                });
            }
            catch(Exception ex)
            {
                _logger.LogInformation($"Error at Login : [{ex.Message}], [{request.UserName}]");
                throw;
            }
        }

        [Authorize]
        [HttpGet("GetData")]
        public ActionResult GetData()
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }

                if (_userService.GetUserDetails(User.Identity?.Name) == null)
                {
                    return Unauthorized();
                }

                var data = _userService.GetUserDetails(User.Identity?.Name);
                var role = _userService.GetUserRole(User.Identity?.Name);

                _logger.LogInformation($"User [{User.Identity?.Name}] logged in the system.");
                return Ok(new GetDetails
                {
                    Employeenumber = data.Employeenumber,
                    UserName = User.Identity?.Name,
                    Employeename = data.Employeename,
                    roleid = data.roleid,
                    DOJ = Convert.ToDateTime(data.DOJ).ToString("dd-MM-yyyy")
                });
            }
            catch(Exception ex)
            {
                _logger.LogInformation($"Error at GetData : [{ex.Message}], [{User.Identity?.Name}]");
                throw;
            }
        }

        //To encrypt password
        public string GetMd5Hash(string input)
        {
            try
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
            catch (Exception ex)
            {
                _logger.LogInformation($"Error at GetMd5Hash : [{ex.Message}], [{User.Identity?.Name}]");
                throw;
            }
        }



        /*
                [HttpGet("getData")]
                [Authorize]
                public ActionResult GetCurrentUser()
                {
                    return Ok(new LoginResult
                    {
                        UserName = User.Identity?.Name,
                        Role = User.FindFirst(ClaimTypes.Role)?.Value ?? string.Empty,
                        Employeename = User.FindFirst("OriginalUserName")?.Value
                    });
                }*/
    }
}
