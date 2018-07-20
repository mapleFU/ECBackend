using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using System.IdentityModel.Tokens;
using System.Security.Claims;
using System.Collections.Specialized;
using ECBack.Models;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Net.Http.Formatting;

namespace ECBack.Controllers
{
    public class UserData
    {
        public string UserID { get; set; }
        public string Password { get; set; }
    }

    public class RegisterData
    {
        /// <summary>
        /// Phone
        /// </summary>
        public string PhoneNumber { get; set; }

        /// <summary>
        /// PWD
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// RealName
        /// </summary>
        public string NickName { get; set; }
    }


    [AllowAnonymous]
    public class AuthController : ApiController
    {
        // https://stackoverflow.com/questions/40281050/jwt-authentication-for-asp-net-web-api
        private OracleDbContext db = new OracleDbContext();
        private const int defaultExpireMinutes = 20;
        // 我恨asp.net，我觉得这是个傻屌玩意
        private const string Secret = "5oiR5oGoYXNwLm5ldO+8jOaIkeinieW+l+i/meaYr+S4quWCu+WxjOeOqeaEjw==";

        public static string GenerateToken(string username, int expireMinutes = 20)
        {
            var symmetricKey = Convert.FromBase64String(Secret);
            var tokenHandler = new JwtSecurityTokenHandler();

            var now = DateTime.UtcNow;
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                        {
                        new Claim(ClaimTypes.Name, username)
                    }),

                Expires = now.AddMinutes(Convert.ToInt32(expireMinutes)),

                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(symmetricKey), SecurityAlgorithms.HmacSha256Signature)
            };

            var stoken = tokenHandler.CreateToken(tokenDescriptor);
            var token = tokenHandler.WriteToken(stoken);

            return token;
        }


        [Route("api/register")]
        [HttpPost]
        public HttpResponseMessage Register([FromBody] RegisterData registerData)
        {
            HttpResponseMessage response;
            
            var duplicate = db.Users.Any(x => x.PhoneNumber == registerData.PhoneNumber);
            if (duplicate)
            {
                // find duplicate in program.
                response = Request.CreateErrorResponse(HttpStatusCode.Conflict, "Phone number duplicate.");
            } else
            {
                User user = new User()
                {
                    PhoneNumber = registerData.PhoneNumber,
                    NickName = registerData.NickName,
                    PasswordHash = registerData.Password,
                };
                // 204, OK
                db.Users.Add(user);
                db.SaveChanges();
                response = Request.CreateResponse(HttpStatusCode.OK, user);
                response.Headers.Add("Location", "/users/" + user.UserID);
                
            }

            return response;
        }

        [Route("api/login")]
        [HttpPost]
        public HttpResponseMessage Login([FromBody] UserData data)
        {
            HttpResponseMessage response;
            if (data.UserID == null)
            {
                response =  Request.CreateResponse((HttpStatusCode)422, "No User ID");
                return response;
            }
            
            var result = db.Users.First(x => string.Equals(x.PhoneNumber, data.UserID));
            if (result == null)
            {
                response = Request.CreateResponse(HttpStatusCode.NotFound, "the phone number not exists");
            } else if (!string.Equals(result.PasswordHash, data.Password))
            {
                response = Request.CreateResponse(HttpStatusCode.Unauthorized, "password error");
            } else
            {
                var jwt = GenerateToken(result.PhoneNumber);
                response = Request.CreateResponse(HttpStatusCode.OK, new {
                    token = jwt,
                    timeout = defaultExpireMinutes
                });
            }
            

            return response;
        }

        public static bool ValidateToken(string token, out string username)
        {
            username = null;

            var simplePrinciple = GetPrincipal(token);
            var identity = simplePrinciple.Identity as ClaimsIdentity;

            if (identity == null)
                return false;

            if (!identity.IsAuthenticated)
                return false;

            var usernameClaim = identity.FindFirst(ClaimTypes.Name);
            username = usernameClaim?.Value;

            if (string.IsNullOrEmpty(username))
                return false;

            // More validate to check whether username exists in system

            return true;
        }

        public static ClaimsPrincipal GetPrincipal(string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var jwtToken = tokenHandler.ReadToken(token) as JwtSecurityToken;

                if (jwtToken == null)
                    return null;

                var symmetricKey = Convert.FromBase64String(Secret);

                var validationParameters = new TokenValidationParameters()
                {
                    RequireExpirationTime = true,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    IssuerSigningKey = new SymmetricSecurityKey(symmetricKey)
                };

                SecurityToken securityToken;
                var principal = tokenHandler.ValidateToken(token, validationParameters, out securityToken);

                return principal;
            }

            catch (Exception)
            {
                //should write log
                return null;
            }
        }

        protected Task<IPrincipal> AuthenticateJwtToken(string token)
        {
            string username;

            if (ValidateToken(token, out username))
            {
                // based on username to get more information from database in order to build local identity
                var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, username)
                // Add more claims if needed: Roles, ...
            };

                var identity = new ClaimsIdentity(claims, "Jwt");
                IPrincipal user = new ClaimsPrincipal(identity);

                return Task.FromResult(user);
            }

            return Task.FromResult<IPrincipal>(null);
        }
    }
}
