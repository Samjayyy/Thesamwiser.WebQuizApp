﻿using System.Security.Principal;
using System.Web.Security;

namespace QuizWebApp
{
    public static class IIdentityExtension
    {
        public static string UserId(this IIdentity identity)
        {
            return identity.IsAuthenticated ? (identity as FormsIdentity).Ticket.UserData : "";
        }
    }
}