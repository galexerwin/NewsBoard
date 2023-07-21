using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using NetBoard.Models;
using NetBoard.Security;

namespace NetBoard.Controllers
{
    [Authorize]
    public class NewsletterController : ControllerBase
    {
        public NewsletterController(IOptions<AzureAdB2COptions> b2cOptions)
        {
            AzureAdB2COptions = b2cOptions.Value;
        }

        public AzureAdB2COptions AzureAdB2COptions { get; set; }

        //[Route("Mailbox/Index")]
        public IActionResult Index()
        {
            // get the current session
            SessionModel session = getTheSession();
            // return the newsletter section
            return View(session);
        }
    }
}
