using ECBack.Controllers;
using ECBack.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Filters;
using System.Web.Mvc;
using System.Web.Mvc.Filters;

namespace ECBack.Filters
{ 
    public class AuthenticationFailureResult : IHttpActionResult
    {
        public AuthenticationFailureResult(string reasonPhrase, HttpRequestMessage request)
        {
            ReasonPhrase = reasonPhrase;
            Request = request;
        }

        public string ReasonPhrase { get; private set; }

        public HttpRequestMessage Request { get; private set; }

        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(Execute());
        }

        private HttpResponseMessage Execute()
        {
            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.Unauthorized);
            response.RequestMessage = Request;
            response.ReasonPhrase = ReasonPhrase;
            return response;
        }
    }

    // https://stackoverflow.com/questions/1064271/asp-net-mvc-set-custom-iidentity-or-iprincipal
    // https://blog.codeinside.eu/2015/04/17/basic-authentication-in-aspnet-webapi/
    public class AuthFilterTest : Attribute, System.Web.Http.Filters.IAuthenticationFilter
    {
        private readonly OracleDbContext dbContext;

        public AuthFilterTest(): base()
        {
            dbContext = new OracleDbContext();
        }

        public bool AllowMultiple { get { return false; } }

        // public bool AllowMultiple => throw new NotImplementedException();

        public async Task AuthenticateAsync(HttpAuthenticationContext context, CancellationToken cancellationToken)
        {
            HttpRequestMessage request = context.Request;
            AuthenticationHeaderValue authorization = request.Headers.Authorization;

            if (authorization == null)
            {
                // No authentication was attempted (for this authentication method).
                // Do not set either Principal (which would indicate success) or ErrorResult (indicating an error).
                return;
            }

            System.Diagnostics.Debug.WriteLine(authorization.Scheme);
            if (authorization.Scheme != "Bearer")
            {
                // no authentication was attempted (for this authentication method).
                // do not set either principal (which would indicate success) or errorresult (indicating an error).
                return;
            }

            if (String.IsNullOrEmpty(authorization.Parameter))
            {
                // Authentication was attempted but failed. Set ErrorResult to indicate an error.
                context.ErrorResult = new AuthenticationFailureResult("Missing credentials", request);
                return;
            }

            string userName = "";
            System.Diagnostics.Debug.WriteLine(authorization.Parameter);
            bool validate = AuthController.ValidateToken(authorization.Parameter, out userName);
            if (!validate)
            {
                // 403
                return;
            } else
            {
                System.Diagnostics.Debug.WriteLine(userName);
                var usr = await dbContext.Users.Where(s => s.PhoneNumber == userName).FirstOrDefaultAsync();
                if (usr == null)
                {
                    System.Diagnostics.Debug.WriteLine("User is null, refused!");
                    return;
                }
                
                // fill value
                context.Principal = usr;
                if (context.Principal != null)
                {
                    System.Diagnostics.Debug.WriteLine("User is not null, filled!");
                }
            }
            
        }

        public Task ChallengeAsync(HttpAuthenticationChallengeContext context, CancellationToken cancellationToken)
        {
            var challenge = new AuthenticationHeaderValue("Bearer", cancellationToken.ToString());
            context.Result = new AddChallengeOnUnauthorizedResult(challenge, context.Result);

            return Task.FromResult(0);
        }
    }

    public class AddChallengeOnUnauthorizedResult : IHttpActionResult
    {
        public AddChallengeOnUnauthorizedResult(AuthenticationHeaderValue challenge, IHttpActionResult innerResult)
        {
            Challenge = challenge;
            InnerResult = innerResult;
        }

        public AuthenticationHeaderValue Challenge { get; private set; }

        public IHttpActionResult InnerResult { get; private set; }

        public async Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            HttpResponseMessage response = await InnerResult.ExecuteAsync(cancellationToken);

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                // Only add one challenge per authentication scheme.
                if (!response.Headers.WwwAuthenticate.Any((h) => h.Scheme == Challenge.Scheme))
                {
                    response.Headers.WwwAuthenticate.Add(Challenge);
                }
            }

            return response;
        }
    }
} 