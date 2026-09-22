using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Polly;
using TinCan;
using Xasu.Auth.Utils;
using Xasu.Exceptions;
using Xasu.Requests;

// 1: Workaround for task result from: https://stackoverflow.com/questions/13127177/if-my-interface-must-return-task-what-is-the-best-way-to-have-a-no-operation-imp

namespace Xasu.Auth.Protocols
{
    public class BearerProtocol : IAuthProtocol
    {
        private readonly string fieldMissingMessage = "Field \"{0}\" required for \"Bearer\" authentication is missing!";
        private readonly string headerNullMessage = "Param \"headerParams\" required for \"Bearer\" authentication is null!";
        private readonly string authTokenField = "auth_token";
        private readonly string homePageField = "homepage";
        private string token;

        public IAsyncPolicy Policy { get; set; }
        public IHttpRequestHandler RequestHandler { get; set; }

        public Agent Agent { get; private set; }

        public AuthState State { get; private set; }

        public string ErrorMessage { get; private set; }

        public Task Init(IDictionary<string, string> config)
        {
            if (!config.ContainsKey(authTokenField))
            {
                throw new MissingFieldException(string.Format(fieldMissingMessage, authTokenField));
            }

            if (!config.ContainsKey(homePageField))
            {
                throw new MissingFieldException(string.Format(fieldMissingMessage, homePageField));
            }

            var authToken = config.Value(authTokenField);
            var homePage = config.Value(homePageField); 

             
            token = authToken;
            State = AuthState.Working;

            var payload = JwtPayload.FromToken(authToken);
            if (!string.IsNullOrWhiteSpace(payload.homepage))
            {
                homePage = payload.homepage;
            }

            Agent = new Agent
            {
                account = new AgentAccount {
                    homePage = homePage,
                    name = payload.username
                }
            };
            XasuTracker.Instance.Log($"[Bearer] Config agent 'auth_token' is '{token}'");
            XasuTracker.Instance.Log($"[Bearer] Config agent 'name' is '{Agent.account.name}'");
            XasuTracker.Instance.Log($"[Bearer] Config agent 'homepage' is '{Agent.account.homePage}'");


            // #1
            return Task.FromResult(0);
        }

        public Task UpdateParamsForAuth(MyHttpRequest request)
        {
            if (request.headers == null)
            {
                throw new ArgumentNullException(headerNullMessage);
            }

            // Authorization
            request.headers.Add("Authorization", token);

            return Task.FromResult(0);
        }

        public void Unauthorized(APIException apiException)
        {
            State = AuthState.Errored;
            ErrorMessage = "Bearer credentials unauthorized: " + apiException.ToString();
        }

        public void Forbidden(APIException apiException)
        {
            State = AuthState.Errored;
            ErrorMessage = "Bearer credentials forbidden: " + apiException.ToString();
        }
    }
}
