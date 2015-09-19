using System;
using System.Configuration;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Web.Mvc;
using System.Web.Security;
using QuizWebApp.Models;
using System.Linq;

namespace QuizWebApp.Controllers
{
    public class AccountController : Controller
    {
        [HttpGet]
        public ActionResult SignIn()
        {
            if (User.Identity.IsAuthenticated)
            {
                return Redirect("~/");
            }
            return View(new SignInViewModel());
        }

        [HttpPost]
        public ActionResult SignIn(SignInViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var salt = ConfigurationManager.AppSettings["SaltOfUserID"];
            var user = new User
            {
                Name = model.HandleName,
                UserId = GetHashedText(string.Join("@", salt, model.HandleName.ToUpperInvariant())),
                Pass = GetHashedText(string.Join(";", salt, model.HandleName.ToUpperInvariant(), salt, model.Pass)),
                CreatedAt = DateTime.UtcNow,
                IsAdmin = false,
            };

            using (var db = new QuizWebAppDb())
            {
                var existing = db.Users.Find(user.UserId);
                if (existing == null)
                {
                    if (!db.Users.Any())
                    {
                        user.IsAdmin = true;
                    }
                    db.Users.Add(user);
                    db.SaveChanges();
                }
                else if (existing.Pass != user.Pass)
                {
                    ModelState.AddModelError("HandleName", "User already in use with other password");
                    return View(model);
                }
            }

            var cookie = FormsAuthentication.GetAuthCookie(user.Name, false);
            var ticket = FormsAuthentication.Decrypt(cookie.Value);
            ticket.GetType().InvokeMember("_UserData", BindingFlags.SetField | BindingFlags.NonPublic | BindingFlags.Instance, null, ticket, new object[] { user.UserId });
            cookie.Value = FormsAuthentication.Encrypt(ticket);
            Response.Cookies.Add(cookie);

            return Redirect("~/");
        }

        [HttpPost]
        public ActionResult SignOut()
        {
            if (User.Identity.IsAuthenticated)
            {
                FormsAuthentication.SignOut();
            }
            return Json(new { url = this.Url.Content("~/") });
        }

        private string GetHashedText(string text)
        {
            return Convert.ToBase64String(new MD5CryptoServiceProvider().ComputeHash(Encoding.UTF8.GetBytes(text)));
        }
    }
}
