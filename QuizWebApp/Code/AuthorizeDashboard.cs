
namespace QuizWebApp.Code
{
    public class AuthorizeDashboard : AuthorizeContextSetting
    {
        public AuthorizeDashboard() : base(c => c.IsDashboardAvailableForUsers)
        {

        }
    }
}