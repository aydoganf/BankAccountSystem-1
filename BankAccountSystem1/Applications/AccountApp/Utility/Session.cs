using AydoganFBank.Service.Dispatcher.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AccountApp.Utility
{
    public class Session
    {
        private readonly HttpContextBase httpContext;

        private Session(HttpContextBase httpContext)
        {
            this.httpContext = httpContext;
        }

        private TokenInfo _sessionToken;
        private TokenInfo _SessionToken
        {
            get
            {
                if (_sessionToken == null)
                {
                    var session = httpContext.Session[Application.SESSION_LOGIN_KEY];
                    if (session != null && session is TokenInfo)
                    {
                        _sessionToken = (TokenInfo)session;
                    }
                }
                return _sessionToken;
            }

            set
            {
                _sessionToken = value;
                httpContext.Session[Application.SESSION_LOGIN_KEY] = _sessionToken;
            }
        }

        public TokenInfo GetToken() => _SessionToken;
        public void SetToken(TokenInfo token) => _SessionToken = token;
        public bool IsTokenExistsAndValid
        {
            get
            {
                return _SessionToken != null && _SessionToken.IsValid && _SessionToken.ValidUntil > DateTime.Now;
            }
        }

        public PersonInfo GetPerson()
        {
            return _SessionToken != null ? _SessionToken.PersonInfo : null;
        }

        public static Session With(HttpContextBase httpContext)
        {
            return new Session(httpContext);
        }
    }

    public static class HttpContextExtensions
    {
        public static Session GetSession(this HttpContextBase httpContext)
        {
            return Session.With(httpContext);
        }
    }
}