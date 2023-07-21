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

namespace NetBoard.Security
{
    public class AzureADTokens
    {
       public Dictionary<string, Claim> DecodeJWTToken(string jwtInput)
        {
            // token claims dictionary
            Dictionary<string, Claim> dicClaim = null;
            // token handler
            var jwtHandler = new JwtSecurityTokenHandler();
            // is token in correct format
            var readableToken = jwtHandler.CanReadToken(jwtInput);
            // check result
            if (readableToken == true)
            {
                // read token in
                var token = jwtHandler.ReadJwtToken(jwtInput);
                // get the claims section
                var claims = token.Claims;
                // push into dictionary object
                dicClaim = claims
                  .GroupBy(claim => claim.Type) // Desired Key
                  .SelectMany(group => group
                     .Select((item, index) => group.Count() <= 1
                        ? Tuple.Create(group.Key, item) // One claim in group
                        : Tuple.Create($"{group.Key}_{index + 1}", item) // Many claims
                      ))
                  .ToDictionary(tuple => tuple.Item1, tuple => tuple.Item2);
            }
            // return
            return dicClaim;
        }
        // return the claim value as string
        public string GetClaimValue(Claim claim)
        {
            return ((claim == null) ? "" : (claim.Value == null ? "" : claim.Value));
        }
        // return the value if found
        public string SearchClaims(Dictionary<string, Claim> claims, string keyname)
        {
            // container for the claim search
            Claim claim;
            // found
            bool found = claims.TryGetValue(keyname, out claim);
            // return
            return found ? GetClaimValue(claim) : "";
        }
        // set a user profile into the user session
        public async void SetUserProfile(string jwtInput, string accessToken, ISession session)
        {
            // get the token contents
            Dictionary<string, Claim> jwtClaims = DecodeJWTToken(jwtInput);
            // create a session model
            SessionModel UserProfile = new SessionModel
            {
                AccessToken = accessToken,
                AzureADID = SearchClaims(jwtClaims, "oid"),
                FirstName = SearchClaims(jwtClaims, "given_name"),
                LastName = SearchClaims(jwtClaims, "family_name"),
                EmailAddress = SearchClaims(jwtClaims, "emails"),
                LocalUserName = SearchClaims(jwtClaims, "extension_PreferredUsername"),
                SelectionType = SearchClaims(jwtClaims, "extension_UsernameSelectionType"),
                UserIsNew = SearchClaims(jwtClaims, "newUser"),
                isNull = false
            };

            //foreach (string key in jwtClaims.Keys)
            //    Console.WriteLine(key + "=" + jwtClaims.GetValueOrDefault(key)?.Value.ToString());
            // attempt to store the data locally and remotely (if new user)
            try
            {
                // store the user profile
                session.SetString("_UserProfile", JsonSerializer.Serialize(UserProfile));
                // push the new profile to our database to create an account
                if (UserProfile.UserIsNew.ToLower().Equals("true"))
                {
                    // attempt to store the user in the system
                    if (!(await RequestNewUserAdd(UserProfile)))
                    {
                        // invalidate the user profile
                        session.SetString("_UserProfile", JsonSerializer.Serialize(GetEmptyProfile()));
                        // show some type of error
                    }
                }
            } 
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        // get a user profile from the user session
        public SessionModel GetUserProfile(ISession session)
        {
            // if there is a user profile stored
            if (!string.IsNullOrEmpty(session.GetString("_UserProfile")))
                return JsonSerializer.Deserialize<SessionModel>(session.GetString("_UserProfile"));
            // return an empty model
            return GetEmptyProfile();
        }
        // clear user profile
        public static void ClearUserProfile(ISession session)
        {
            // get an empty model
            SessionModel UserProfile = GetEmptyProfile();
            // try to remove the session
            try
            {
                // store the user profile
                session.SetString("_UserProfile", JsonSerializer.Serialize(UserProfile));
            } 
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }
        // empty profile return
        private static SessionModel GetEmptyProfile()
        {
            // return an empty model
            return new SessionModel
            {
                AccessToken = "",
                AzureADID = "",
                FirstName = "NONE",
                LastName = "NONE",
                EmailAddress = "NONE",
                LocalUserName = "NONE",
                SelectionType = "NONE",
                UserIsNew = "false",
                isNull = true
            };
        }
        // store a new profile acccount in our DB
        public async Task<bool> RequestNewUserAdd(SessionModel newUser)
        {
            try
            {
                // url is hardcoded for now
                const string uri = "https://api.newsboard.email/member/";
                // we need to send only  a data subset 
                var sendData = new Dictionary<string, string>();
                // add required
                sendData.Add("givenName", newUser.FirstName);
                sendData.Add("surName", newUser.LastName);
                sendData.Add("localEmail", newUser.LocalUserName);
                sendData.Add("remoteEmail", newUser.EmailAddress);
                // new http client
                HttpClient client = new HttpClient();
                // ****** temporary patch (not using authentication) 
                var requestMessage = new HttpRequestMessage
                {
                    Method = HttpMethod.Post,
                    Content = new StringContent(JsonSerializer.Serialize(sendData), Encoding.UTF8, "application/json"),
                    RequestUri = new Uri(uri)
                };
                // ****** temporary patch (submitting a user identifier directly)
                client.DefaultRequestHeaders.Add("azureID", newUser.AzureADID);
                // get a response
                var response = await client.SendAsync(requestMessage);
                // check the response values
                if (response.IsSuccessStatusCode)
                {
                    try
                    {
                        // get the content returned
                        var content = JsonSerializer.Deserialize<ResponseModel>(response.Content.ReadAsStringAsync().Result);
                        // check the state and throw an error if necessary
                        if (!content.success) 
                        {
                            Console.WriteLine("Unsuccessful");
                            return false;
                        }
                        // success
                        return true;
                    }
                    catch (Exception ex1)
                    {
                        Console.WriteLine(ex1.Message);
                        return false;
                    }
                }  
                else
                {
                    Console.WriteLine(response.StatusCode);
                    Console.WriteLine(response.Content.ToString());

                    foreach (var header in response.Headers)
                        Console.WriteLine(header.Key + " " + header.Value.FirstOrDefault());


                    return false;
                } 
            }
            catch (Exception ex2)
            {
                Console.WriteLine(ex2.Message);
                return false;
            }        
        }
    }
    /*
                //// create the request
                //var request = new HttpRequestMessage(HttpMethod.Post, uri)
                //{
                //    Content = JsonContent.Create(sendData)
                //};
                //// create an authorization header
                //client.DefaultRequestHeaders.Add("Authorization", "Bearer " + newUser.AccessToken);
                ////request.DefaultRequestHeader.Authorization = new AuthenticationHeaderValue("Bearer", newUser.AccessToken);
                ////Console.WriteLine("DEBUG TEST ACCESS");
                ////Console.WriteLine(request.Headers.GetValues("Bearer").FirstOrDefault());

                //// send the request and await a response
                ///
                //Console.WriteLine("AccessTOkem" + newUser.AccessToken);
                //using var response = await client.SendAsync(request);
    Console.WriteLine("Sending Headers");
    foreach (var header in client.DefaultRequestHeaders)
        Console.WriteLine(header.Key + " " + header.Value.FirstOrDefault());

    Console.WriteLine(requestMessage.Content.ToString());
    Console.WriteLine(JsonSerializer.Serialize(sendData));                    
     */
}
