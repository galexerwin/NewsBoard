using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using NetBoard.Models;

namespace NetBoard.Helpers
{
    public class HTTPSClient
    {
        // get a request
        public static async Task<APIModel> clientGet(string uri, string azureID)
        {
            try 
            { 
                // new http client
                HttpClient client = new HttpClient();
                // ****** temporary patch (not using authentication) 
                var requestMessage = new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri(uri)
                };
                // ****** temporary patch (submitting a user identifier directly)
                client.DefaultRequestHeaders.Add("azureID", azureID);
                // get a response
                var response = await client.SendAsync(requestMessage);
                // create a new empty response model
                var rJSON = new ResponseModel { success = false };
                // grab the data
                var rData = response.Content.ReadAsStringAsync().Result;
                // try evaluating the response data
                try { rJSON = JsonSerializer.Deserialize<ResponseModel>(rData); } catch (Exception) { }
                // return a result
                return new APIModel
                {
                    responseOkay = response.IsSuccessStatusCode,
                    responseData = rData,
                    responseJSON = rJSON
                };
            }
            catch (Exception ex)
            {
                // log message
                Console.WriteLine("Error: " + ex.Message);
                // return empty
                return new APIModel
                {
                    responseOkay = false
                };
            }
        }
        // post a request
        public static async Task<APIModel> clientPost(string uri, string azureID, string data)
        {
            try 
            { 
                // new http client
                HttpClient client = new HttpClient();
                // ****** temporary patch (not using authentication) 
                var requestMessage = new HttpRequestMessage
                {
                    Method = HttpMethod.Post,
                    Content = new StringContent(data, Encoding.UTF8, "application/json"),
                    RequestUri = new Uri(uri)
                };
                // ****** temporary patch (submitting a user identifier directly)
                client.DefaultRequestHeaders.Add("azureID", azureID);
                // get a response
                var response = await client.SendAsync(requestMessage);
                // create a new empty response model
                var rJSON = new ResponseModel { success = false };
                // grab the data
                var rData = response.Content.ReadAsStringAsync().Result;
                // try evaluating the response data
                try { rJSON = JsonSerializer.Deserialize<ResponseModel>(rData); } catch (Exception) { }
                // return a result
                return new APIModel
                {
                    responseOkay = response.IsSuccessStatusCode,
                    responseData = rData,
                    responseJSON = rJSON
                };
            }
            catch (Exception ex)
            {
                // log message
                Console.WriteLine("Error: " + ex.Message);
                // return empty
                return new APIModel
                {
                    responseOkay = false
                };
            }
        }
        // update something
        public static async Task<APIModel> clientPut(string uri, string azureID, string data)
        {
            try 
            { 
                // new http client
                HttpClient client = new HttpClient();
                // ****** temporary patch (not using authentication) 
                var requestMessage = new HttpRequestMessage
                {
                    Method = HttpMethod.Put,
                    Content = new StringContent(data, Encoding.UTF8, "application/json"),
                    RequestUri = new Uri(uri)
                };
                // ****** temporary patch (submitting a user identifier directly)
                client.DefaultRequestHeaders.Add("azureID", azureID);
                // get a response
                var response = await client.SendAsync(requestMessage);
                // create a new empty response model
                var rJSON = new ResponseModel { success = false };
                // grab the data
                var rData = response.Content.ReadAsStringAsync().Result;
                // try evaluating the response data
                try { rJSON = JsonSerializer.Deserialize<ResponseModel>(rData); } catch (Exception) { }
                // return a result
                return new APIModel
                {
                    responseOkay = response.IsSuccessStatusCode,
                    responseData = rData,
                    responseJSON = rJSON
                };
            }
            catch (Exception ex)
            {
                // log message
                Console.WriteLine("Error: " + ex.Message);
                // return empty
                return new APIModel
                {
                    responseOkay = false
                };
            }
        }
        // delete something
        public static async Task<APIModel> clientDelete(string uri, string azureID, string data)
        {
            try 
            { 
                // new http client
                HttpClient client = new HttpClient();
                // ****** temporary patch (not using authentication) 
                HttpRequestMessage requestMessage;
                // check contents
                if (string.IsNullOrWhiteSpace(data))
                {
                    requestMessage = new HttpRequestMessage
                    {
                        Method = HttpMethod.Delete,
                        RequestUri = new Uri(uri)
                    }; 
                } 
                else
                {
                    requestMessage = new HttpRequestMessage
                    {
                        Method = HttpMethod.Delete,
                        Content = new StringContent(data, Encoding.UTF8, "application/json"),
                        RequestUri = new Uri(uri)
                    };
                }
                // ****** temporary patch (submitting a user identifier directly)
                client.DefaultRequestHeaders.Add("azureID", azureID);
                // get a response
                var response = await client.SendAsync(requestMessage);
                // create a new empty response model
                var rJSON = new ResponseModel { success = false };
                // grab the data
                var rData = response.Content.ReadAsStringAsync().Result;
                // try evaluating the response data
                try { rJSON = JsonSerializer.Deserialize<ResponseModel>(rData); } catch (Exception) { }
                // return a result
                return new APIModel
                {
                    responseOkay = response.IsSuccessStatusCode,
                    responseData = rData,
                    responseJSON = rJSON
                };
            }
            catch (Exception ex)
            {
                // log message
                Console.WriteLine("Error: " + ex.Message);
                // return empty
                return new APIModel
                {
                    responseOkay = false
                };
            }
        }
    }
}
