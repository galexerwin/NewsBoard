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
    [Route("api/folders/[action]")]
    [ApiController]
    public class FoldersController : ControllerBase
    {
        // get default folder list
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> Get()
        {
            // get the current session
            SessionModel session = getTheSession();
            // format uri
            string uri = apiEndpoint + "/folders"; 
            // call the endpoint
            var result = await HTTPSClient.clientGet(uri, session.AzureADID);
            // if OK return the data
            if (result.responseOkay)
                return Ok(result.responseData);
            // default
            return BadRequest(result.responseData);
        }
        // get specific folder returns a view
        [HttpGet("{folderID}/{page?}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> Get(string folderID, string page)
        {
            // get the current session
            SessionModel session = getTheSession();
            // format folder & page parameters if they exist to add to URI
            folderID = !string.IsNullOrWhiteSpace(folderID) ? "/" + folderID : "/INBOX";
            page   = !string.IsNullOrWhiteSpace(page) ? "/" + page : "/1";
            // format uri
            string uri = apiEndpoint + "/inbox/read" + folderID + page; Console.WriteLine(uri);
            // call the endpoint
            var result = await HTTPSClient.clientGet(uri, session.AzureADID);
            // if OK return the data
            if (result.responseOkay)
                return Ok(result.responseData);
            // default
            return BadRequest(result.responseData);
        }
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> Add([FromBody] dynamic data)
        {
            // get the current session
            SessionModel session = getTheSession();
            // format uri
            string uri = apiEndpoint + "/folders";
            // call the endpoint
            var result = await HTTPSClient.clientPost(uri, session.AzureADID, Convert.ToString(data));
            // if OK return the data
            if (result.responseOkay)
                return Ok(result.responseData);
            // default
            return BadRequest(result.responseData);
        }
        [HttpDelete("{folderID}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> Delete(string folderID)
        {
            // get the current session
            SessionModel session = getTheSession();
            // format uri
            string uri = apiEndpoint + "/folders/" + folderID;
            // call the endpoint
            var result = await HTTPSClient.clientDelete(uri, session.AzureADID, "");
            // if OK return the data
            if (result.responseOkay)
                return Ok(result.responseData);
            // default
            return BadRequest(result.responseData);
        }
    }
}
