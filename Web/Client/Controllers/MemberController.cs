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
    [Route("api/member/[action]")]
    [ApiController]
    public class MemberController : ControllerBase
    {
        // default
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public IActionResult Index()
        {
            // not found
            return NotFound();
        }
        // member functions
        [HttpGet("{requestID}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> Check(string requestID)
        {
            // format uri
            string uri = apiEndpoint + "/member/check/" + requestID;
            // call the endpoint
            var result = await HTTPSClient.clientGet(uri, "");
            // if OK return the data
            if (result.responseOkay)
                return Ok(result.responseData);
            // default
            return BadRequest(result.responseData);
        }
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] dynamic data)
        {
            // get the current session
            SessionModel session = getTheSession();
            // format uri
            string uri = apiEndpoint + "/member";
            // call the endpoint
            var result = await HTTPSClient.clientPost(uri, session.AzureADID, Convert.ToString(data));
            // if OK return the data
            if (result.responseOkay)
                return Ok(result.responseData);
            // default
            return BadRequest(result.responseData);
        }
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] dynamic data)
        {
            // get the current session
            SessionModel session = getTheSession();
            // format uri
            string uri = apiEndpoint + "/member";
            // call the endpoint
            var result = await HTTPSClient.clientPut(uri, session.AzureADID, Convert.ToString(data));
            // if OK return the data
            if (result.responseOkay)
                return Ok(result.responseData);
            // default
            return BadRequest(result.responseData);
        }
    }
}
