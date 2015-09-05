using QuizWebApp.Models;
using System.Web;
using System.Web.Mvc;

namespace QuizWebApp.Code
{
    public class AuthorizeQuizMaster : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            return IsAllow(httpContext);
        }

        public static bool IsAllow(HttpContextBase httpContext)
        {
            var userIdentity = httpContext.User.Identity;
            if (userIdentity.IsAuthenticated == false) return false;
            using (var db = new QuizWebAppDb())
            {
                var userInfo = db.Users.Find(userIdentity.UserId());
                if (userInfo == null) return false;

                return userInfo.IsAdmin;
            }
        }
    }
}