using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using StrongGrid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text;

namespace FunctionApp
{
    public static class NewsBoard
    {
        [FunctionName("Parser")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            // save something to analytics
            log.LogInformation("Newsletter Parser has been triggered");
            // execute code   
            try
            {
                // service bus connection information
                const string connString = "Endpoint=sb://newsboard.servicebus.windows.net/;SharedAccessKeyName=NewsletterInbound;SharedAccessKey=fhUsmB7EW27thklhXaVlFfZv6eIs3BurJYURV0JdgC0=";
                const string queueName = "newslettersinbound";
                // from containers
                string fName = "", fEmail = "", fValue = null;
                // create a service bus queue client
                IQueueClient queueClient = new QueueClient(connString, queueName);
                // create a parser
                var parser = new WebhookParser();
                // parse the inbound email
                var inbound = parser.ParseInboundEmailWebhook(req.Body);
                var strHTML = inbound.Html ?? "";
                var b64HTML = PrepareNewsletterOutput(strHTML);
                // iterate over the headers
                for (int i = 0; i < inbound.Headers.Length; i++)
                {
                    // check for the real from value
                    if (inbound.Headers[i].Key.ToLower().Equals("from"))
                    {
                        log.LogInformation("found");
                        // get full value
                        fValue = inbound.Headers[i].Value;
                        // get the beginning of the email address
                        int spos = fValue.LastIndexOf('<');
                        // parse into data parts
                        fName = fValue.Substring(0, spos - 1).Trim();
                        fEmail = fValue.Substring(spos + 1, fValue.Length - (spos + 2)).Trim();
                        // terminate
                        break;
                    }
                }
                // set final output
                fName = string.IsNullOrWhiteSpace(fValue) ? "" : fName;
                fEmail = string.IsNullOrWhiteSpace(fValue) ? inbound.From.Email : fEmail;
                // construct the email output
                var newEmail = new
                {
                    To = inbound.To.FirstOrDefault()?.Email,
                    SenderName = fName,
                    SenderEmail = fEmail,
#pragma warning disable IDE0037 // Use inferred member name
                    Subject = inbound.Subject,
#pragma warning restore IDE0037 // Use inferred member name
                    HTML = b64HTML
                };
                log.LogInformation("Content Output");
                // log this to our analytics
                log.LogInformation(JsonConvert.SerializeObject(newEmail, Formatting.Indented));
                // construct the message to the queue
                var message = new Message
                {
                    Label = "NewsletterReceivedEvent",
                    Body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(newEmail))
                };
                // send the message to the queue
                await queueClient.SendAsync(message);
                // close the connection
                await queueClient.CloseAsync();
                // return ok
                return new OkResult();
            }
            catch (System.Exception ex)
            {
                // log the error
                log.LogError(ex.Message);
                // return error code
                return new BadRequestResult();
            }
        }
        // handles decoding quoted printable to UTF8 and then to a b64 string
        private static string PrepareNewsletterOutput(string input)
        {
            // wrap in try catch
            try
            {
                var i = 0;
                // list of byte arrays
                var output = new List<byte>();
                // iterate over input
                while (i < input.Length)
                {
                    // if we see something that looks like quoted printable, look ahead
                    if (input[i] == '=' && input[i + 1] == '\r' && input[i + 2] == '\n')
                    {
                        // skip
                        i += 3;
                    }
                    else if (input[i] == '=')
                    {
                        // hold the next two characters
                        string sHex = input;
                        // look past the equal sign by two 
                        sHex = sHex.Substring(i + 1, 2);
                        // get the hex value
                        int hex = Convert.ToInt32(sHex, 16);
                        // convert to byte from hex
                        byte b = Convert.ToByte(hex);
                        // add the byte value
                        output.Add(b);
                        // increment past three characters
                        i += 3;
                    }
                    else
                    {
                        // directly add to bytes out
                        output.Add((byte)input[i]);
                        // increment by 1
                        i++;
                    }
                }
                // return the base64 encode string
                return Convert.ToBase64String(output.ToArray());
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }
            // we only get here if there's an exception
            // generate bytes
            var byteArray = Encoding.UTF8.GetBytes(input);
            // return a base64 encode string
            return Convert.ToBase64String(byteArray);
        }
    }
}
