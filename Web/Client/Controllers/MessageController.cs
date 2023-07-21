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
    [Route("api/message/[action]")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        // message entry which returns the entire JSON for the interface 
        [HttpGet("{newsletterID}")]
        public async Task<IActionResult> Get(string newsletterID)
        {
            // return overloaded
            return await Refresh(newsletterID, false);
        }
        // message entry which only returns the content pane JSON
        [HttpGet("{newsletterID}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> Refresh(string newsletterID, bool refresh = true)
        {
            // get the current session
            SessionModel session = getTheSession();
            // format uri
            string uri = apiEndpoint + "/message" + (refresh ? "/read" : "") + "/" + newsletterID;
            // call the endpoint
            var result = await HTTPSClient.clientGet(uri, session.AzureADID);
            // if OK return the data
            if (result.responseOkay)
                return Ok(result.responseData);
            // default
            return BadRequest(result.responseData);
        }
        // message mark as read/unread
        [HttpPut("{state}")]
        public async Task<IActionResult> Update(string state, [FromBody] dynamic data)
        {
            // get the current session
            SessionModel session = getTheSession();
            // format uri
            string uri = apiEndpoint + "/message/state/" + state;
            // call the endpoint
            var result = await HTTPSClient.clientPut(uri, session.AzureADID, Convert.ToString(data));
            // if OK return the data
            if (result.responseOkay)
                return Ok(result.responseData);
            // default
            return BadRequest(result.responseData);
        }
        // message delete
        [HttpDelete] 
        public async Task<IActionResult> Delete([FromBody] dynamic data)
        {
            // get the current session
            SessionModel session = getTheSession();
            // format uri
            string uri = apiEndpoint + "/message";
            // call the endpoint
            var result = await HTTPSClient.clientDelete(uri, session.AzureADID, Convert.ToString(data));
            // if OK return the data
            if (result.responseOkay)
                return Ok(result.responseData);
            // default
            return BadRequest(result.responseData);
        }
        // message move
        [HttpPut("{folderID}")]
        public async Task<IActionResult> Move(string folderID, [FromBody] dynamic data)
        {
            // get the current session
            SessionModel session = getTheSession();
            // format uri
            string uri = apiEndpoint + "/message/move/" + folderID;
            // call the endpoint
            var result = await HTTPSClient.clientPut(uri, session.AzureADID, Convert.ToString(data));
            // if OK return the data
            if (result.responseOkay)
                return Ok(result.responseData);
            // default
            return BadRequest(result.responseData);
        }
        // message favorite
        [HttpGet("{newsletterID}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> Favorite(string newsletterID)
        {
            // get the current session
            SessionModel session = getTheSession();
            // format uri
            string uri = apiEndpoint + "/message/favorite/" + newsletterID;
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
