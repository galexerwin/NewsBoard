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
    [Route("api/notes/[action]")]
    [ApiController]
    public class NotesController : ControllerBase
    {
        // default
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public IActionResult Get()
        {
            // not found
            return NotFound();
        }
        // notes retrieve from associated newsletter
        [HttpGet("{newsletterID}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> Get(string newsletterID) 
        {
            // get the current session
            SessionModel session = getTheSession();
            // format uri
            string uri = apiEndpoint + "/annotations/" + newsletterID;
            // call the endpoint
            var result = await HTTPSClient.clientGet(uri, session.AzureADID);
            // if OK return the data
            if (result.responseOkay)
                return Ok(result.responseData);
            // default
            return BadRequest(result.responseData);            
        }
        // notes add
        [HttpPost("{newsletterID}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> Add(string newsletterID, [FromBody] dynamic data) 
        {
            // get the current session
            SessionModel session = getTheSession();
            // format uri
            string uri = apiEndpoint + "/annotations/" + newsletterID;
            // call the endpoint
            var result = await HTTPSClient.clientPost(uri, session.AzureADID, Convert.ToString(data));
            // if OK return the data
            if (result.responseOkay)
                return Ok(result.responseData);
            // default
            return BadRequest(result.responseData);
        }
        // notes update
        [HttpPut("{newsletterID}/{noteID}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> Update(string newsletterID, string noteID, [FromBody] dynamic data)
        {
            // get the current session
            SessionModel session = getTheSession();
            // format uri
            string uri = apiEndpoint + "/annotations/" + newsletterID + "/" + noteID;
            // call the endpoint
            var result = await HTTPSClient.clientPut(uri, session.AzureADID, Convert.ToString(data));
            // if OK return the data
            if (result.responseOkay)
                return Ok(result.responseData);
            // default
            return BadRequest(result.responseData);
        }
        // notes delete
        [HttpDelete("{newsletterID}/{noteID}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> Delete(string newsletterID, string noteID) 
        {
            // get the current session
            SessionModel session = getTheSession();
            // format uri
            string uri = apiEndpoint + "/annotations/" + newsletterID + "/" + noteID;
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
