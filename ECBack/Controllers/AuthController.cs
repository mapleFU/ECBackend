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
using System.ComponentModel.DataAnnotations;
using System.Web;
using ECBack.Filters;

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
        [Required]
        public string PhoneNumber { get; set; }

        /// <summary>
        /// PWD
        /// </summary>
        [Required]
        public string Password { get; set; }

        /// <summary>
        /// RealName
        /// </summary>
        [Required]
        public string NickName { get; set; }

        public DateTime Birthday { get; set; }
    }

    public class RegisterSeller
    {
        [Required]
        public string Password { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
    }


    [AllowAnonymous]
    public class AuthController : ApiController
    {
        // https://stackoverflow.com/questions/40281050/jwt-authentication-for-asp-net-web-api
        private OracleDbContext db = new OracleDbContext();
        private const int defaultExpireMinutes = 120;
        // 我恨asp.net，我觉得这是个傻屌玩意
        private const string Secret = "5oiR5oGoYXNwLm5ldO+8jOaIkeinieW+l+i/meaYr+S4quWCu+WxjOeOqeaEjw==";
        private const string SellerSecret = "5L2g5aaI5q275LqG77yM5oiR5piv5L2g5ZOl5ZOl77yM5oiR5Lus6YO95piv5L2g5aaI55qE5YS/5a2Q77yB";
        public static string GenerateToken(string username, int expireMinutes = defaultExpireMinutes, string mode = "User")
        {
            string usingSecretString;
            if (mode == "User")
            {
                usingSecretString = Secret;
            } else
            {
                usingSecretString = SellerSecret;
            }
            var symmetricKey = Convert.FromBase64String(usingSecretString);
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

        [Route("api/Sellers/register")]
        [HttpPost]
        public async Task<IHttpActionResult> AddSeller([FromBody]RegisterSeller registerSeller )
        {
            HttpResponseMessage response;
            if (!ModelState.IsValid)
            {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.BadRequest, "参数错误"));
            }
            var duplicate = db.Sellers.Any(x => x.Phone == registerSeller.PhoneNumber)
                    ;
            if (duplicate)
            {
                response = Request.CreateErrorResponse(HttpStatusCode.Conflict, "电话号码重复");
            }
            else
            {
                Seller seller = new Seller()
                {
                    PasswordHash = registerSeller.Password,
                    PhoneNumber = registerSeller.PhoneNumber

                };
                db.Sellers.Add(seller);
                await db.SaveChangesAsync();

                response = Request.CreateResponse(HttpStatusCode.NoContent);
                response.Headers.Add("Location", "api/Sellers/" + seller.SellerID);
                
            }
            return ResponseMessage(response);
        }

        [Route("api/register")]
        [HttpPost]
        public HttpResponseMessage Register([FromBody] RegisterData registerData)
        {
            HttpResponseMessage response;
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "参数错误");
            }
            try
            {
                var duplicate = db.Users.First(x => x.PhoneNumber == registerData.PhoneNumber || x.NickName == registerData.NickName)
                    ;
                string confict;
                if (duplicate.PhoneNumber == registerData.PhoneNumber)
                {
                    confict = "电话号码";
                }
                else
                {
                    confict = "用户名";
                }
                response = Request.CreateErrorResponse(HttpStatusCode.Conflict, confict + "重复");
            } catch (Exception ex)
            {
                User user = new User()
                {
                    PhoneNumber = registerData.PhoneNumber,
                    NickName = registerData.NickName,
                    PasswordHash = registerData.Password,
                    BirthDay = registerData.Birthday
                };
                

                // 204, OK
                
                db.Users.Add(user);
                db.SaveChanges();
                db.Carts.Add(new Cart()
                {
                    User = user,
                    UserID = user.UserID
                });
                db.VIPs.Add(new VIP()
                {
                    User = user,
                    UserID = user.UserID
                });

                db.SaveChanges();
                response = Request.CreateResponse(HttpStatusCode.OK, new {
                    UserID = user.UserID
                });
                response.Headers.Add("Location", "api/Users/" + user.UserID);
                
            }
            
           
            return response;
        }

        [Route("api/Sellers/login")]
        [HttpPost]
        public HttpResponseMessage SellerLogin([FromBody] UserData data)
        {
            HttpResponseMessage response;
            if (data.UserID == null)
            {
                response = Request.CreateResponse((HttpStatusCode)422, "No User ID");
                return response;
            }

            try
            {
                if (db.Users == null)
                {
                    System.Diagnostics.Debug.WriteLine("Sellers is fucking null");
                }
                var result = db.Sellers.First(x => x.Phone == data.UserID);
                if (!string.Equals(result.PasswordHash, data.Password))
                {
                    response = Request.CreateResponse(HttpStatusCode.Unauthorized, "password error");
                }
                else
                {
                    var jwt = GenerateToken(result.PhoneNumber,mode:"Seller");
                    HttpContext.Current.User = result;
                    response = Request.CreateResponse(HttpStatusCode.OK, new
                    {
                        Token = jwt,
                        Timeout = defaultExpireMinutes,
                        SellerID = result.SellerID
                    });
                }
            }
            catch (ArgumentNullException)
            {
                response = Request.CreateResponse(HttpStatusCode.NotFound, "the phone number not exists");
            }
            catch (InvalidOperationException)
            {
                response = Request.CreateResponse(HttpStatusCode.NotFound, "the phone number not exists");
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
            
            try
            {
                if (db.Users == null)
                {
                    System.Diagnostics.Debug.WriteLine("Users is fucking null");
                }
                var result = db.Users.First(x => x.PhoneNumber == data.UserID);
                if (!string.Equals(result.PasswordHash, data.Password))
                {
                    response = Request.CreateResponse(HttpStatusCode.Unauthorized, "password error");
                } else
                {
                    var jwt = GenerateToken(result.PhoneNumber);
                    HttpContext.Current.User = result;
                    response = Request.CreateResponse(HttpStatusCode.OK, new
                    {
                        Token = jwt,
                        Timeout = defaultExpireMinutes,
                        UserID = result.UserID
                    });
                }
            } catch (ArgumentNullException)
            {
                response = Request.CreateResponse(HttpStatusCode.NotFound, "the phone number not exists");
            } catch (InvalidOperationException)
            {
                response = Request.CreateResponse(HttpStatusCode.NotFound, "the phone number not exists");
            }
            return response;
        }

        [SellerAuthFilter]
        [Route("api/Sellers/test")]
        [HttpGet]
        public IHttpActionResult TestLogin()
        {
            Seller seller = (Seller)HttpContext.Current.User;
            return Ok(seller.PhoneNumber);
        }

        public static bool ValidateToken(string token, out string username, string mode="User")
        {
            username = null;

            var simplePrinciple = GetPrincipal(token, mode);
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

        public static ClaimsPrincipal GetPrincipal(string token, string mode)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var jwtToken = tokenHandler.ReadToken(token) as JwtSecurityToken;

                if (jwtToken == null)
                    return null;

                string usingSecretString;
                if (mode == "User")
                {
                    usingSecretString = Secret;
                }
                else
                {
                    usingSecretString = SellerSecret;
                }

                var symmetricKey = Convert.FromBase64String(usingSecretString);

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
