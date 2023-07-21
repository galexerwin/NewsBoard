using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using NetBoard.Models;
using NetBoard.Security;


namespace NetBoard.Controllers
{
    public class ControllerBase : Controller
    {
        public const string        apiEndpoint = "https://api.newsboard.email";
        // get the session
        SessionModel _theSession;
        // retrieve the session or an empty one on every action execute
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            // get a token/session parser
            AzureADTokens tokens = new AzureADTokens();
            // retrieve the session
            _theSession = tokens.GetUserProfile(HttpContext.Session);
        }
        // child classes can get the session here
        public SessionModel getTheSession()
        {
            return _theSession;
        }
    }
}