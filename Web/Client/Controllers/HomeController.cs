/*
 
 
 Articles:
    https://www.telerik.com/blogs/how-to-pass-multiple-parameters-get-method-aspnet-core-mvc
 
 */
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Client;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;
using NetBoard.Models;
using NetBoard.Security;

namespace NetBoard.Controllers
{
    public class HomeController : ControllerBase
    {
        AzureAdB2COptions   AzureAdB2COptions;
        public HomeController(IOptions<AzureAdB2COptions> azureAdB2COptions)
        {
            AzureAdB2COptions = azureAdB2COptions.Value;
        }
        // index page
        public IActionResult Index()
        {
            // get the current session
            SessionModel session = getTheSession();
            // return the session data to views
            return View(session);
        }
        // for the about page
        [Route("About")]
        [Route("Home/About")]
        public IActionResult About()
        {
            // get the current session
            SessionModel session = getTheSession();
            // return the session data to views
            return View(session);
        }
        // for support 
        [Route("Support")]
        [Route("Home/Support")]
        public IActionResult Support()
        {
            // get the current session
            SessionModel session = getTheSession();
            // return the session data to views
            return View(session);
        }
        // for error handling
        public IActionResult Error(string message)
        {
            ViewBag.Message = message;
            return View();
        }
    }
}
