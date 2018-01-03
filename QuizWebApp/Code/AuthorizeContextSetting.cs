using QuizWebApp.Models;
using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QuizWebApp.Code
{
    public abstract class AuthorizeContextSetting : AuthorizeAttribute
    {
        private Func<Context, bool> _evaluator;
        public AuthorizeContextSetting(Func<Context, bool> evaluator)
        {
            _evaluator = evaluator;
        }
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            return IsAllow(httpContext);
        }

        public bool IsAllow(HttpContextBase httpContext)
        {
            var userIdentity = httpContext.User.Identity;
            if (userIdentity.IsAuthenticated == false) return false;
            using (var db = new QuizWebAppDb())
            {
                var userInfo = db.Users.Find(userIdentity.UserId());
                if (userInfo == null) return false; // user not found can't pass
                if (userInfo.IsAdmin) return true; // Admin always passes through
                return _evaluator(db.Context);
            }
        }
    }
}