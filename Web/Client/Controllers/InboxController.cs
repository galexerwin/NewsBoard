/*
 
 */
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using NetBoard.Helpers;
using NetBoard.Models;
using NetBoard.Security;

namespace NetBoard.Controllers
{
    [Route("api/inbox/[action]")]
    [ApiController]
    public class InboxController : ControllerBase
    {
        // inbox entry which returns the entire JSON for the interface
        [HttpGet("{folderID?}/{page?}")]
        public async Task<IActionResult> Get(string folder, string page)
        {
            // return overloaded
            return await Refresh(folder, page, false);
        }
        // inbox entry which only returns the list pane JSON
        [HttpGet("{folderID?}/{page?}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> Refresh(string folderID, string page, bool refresh = true)
        {
            // get the current session
            SessionModel session = getTheSession();
            // format folder & page parameters if they exist to add to URI
            folderID = !string.IsNullOrWhiteSpace(folderID) ? "/" + folderID : "/INBOX";
            page   = !string.IsNullOrWhiteSpace(page) ? "/" + page : "/1";
            // format uri
            string uri = apiEndpoint + "/inbox" + (refresh ? "/read" : "") + folderID + page;
            // call the endpoint
            var result = await HTTPSClient.clientGet(uri, session.AzureADID);
            // if OK return the data
            if (result.responseOkay)
                return Ok(result.responseData);
            // default
            return BadRequest(result.responseData);
        }
    }
}
