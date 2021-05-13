using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Filters;
using System.Web.Http.Results;

namespace LabSolution.API
{
    public class CustomAuthenticationFilter : AuthorizeAttribute, IAuthenticationFilter
    {
        public bool AllowMultiple
        {
            get { return false; }
        }

        public async Task AuthenticateAsync(HttpAuthenticationContext context, CancellationToken cancellationToken)
        {
            string authParamter = string.Empty;
            HttpRequestMessage request = context.Request;
            AuthenticationHeaderValue authentication = request.Headers.Authorization;

            string[] TokenAndUser = null;

            if(authentication == null)
            {
                context.ErrorResult = new AuthenticationFailureResult(reasonPhrase:"Missing Authorization Header", request);
                return;
            }

            if(authentication.Scheme != "Bearer")
            {
                context.ErrorResult = new AuthenticationFailureResult(reasonPhrase: "Invalid Authorization Scheme", request);
                return;
            }

            TokenAndUser = authentication.Parameter.Split(':');

            string Token = TokenAndUser[0];
            string userName = TokenAndUser[1];

            if (String.IsNullOrEmpty(TokenAndUser[0]))
            {
                context.ErrorResult = new AuthenticationFailureResult(reasonPhrase: "Missing Token", request);
                return;
            }

            string ValidUserName = TokenManager.ValidateToken(Token);
            if(userName != ValidUserName)
            {
                context.ErrorResult = new AuthenticationFailureResult(reasonPhrase: "Invalid Token for User", request);
                return;
            }    

            context.Principal = TokenManager.GetPrincipal(Token);
        }

        public async Task ChallengeAsync(HttpAuthenticationChallengeContext context, CancellationToken cancellationToken)
        {
            var result = await context.Result.ExecuteAsync(cancellationToken);
            if(result.StatusCode == HttpStatusCode.Unauthorized)
            {
                result.Headers.WwwAuthenticate.Add(new AuthenticationHeaderValue(scheme:"Basic", parameter:"realm=localhost"));
            }
            context.Result = new ResponseMessageResult(result);
        }
    }

    public class AuthenticationFailureResult : IHttpActionResult
    {
        public string ReasonPhrase;

        public HttpRequestMessage Request { set; get; }

        public AuthenticationFailureResult(string reasonPhrase, HttpRequestMessage request)
        {
            ReasonPhrase = reasonPhrase;
            Request = request;
        }

        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(Execute());
        }

        public HttpResponseMessage Execute()
        {
            HttpResponseMessage responseMessage = new HttpResponseMessage(HttpStatusCode.Unauthorized);
            responseMessage.RequestMessage = Request;
            responseMessage.ReasonPhrase = ReasonPhrase;
            return responseMessage;
        }
    }
}