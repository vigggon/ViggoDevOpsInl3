using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net;

namespace VA_Function
{
    public static class SmokeTestFunction
    {
        private static readonly HttpClient httpClient = new HttpClient();

        [FunctionName("SmokeTest")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            string url = "https://viggodevopsinl3app.azurewebsites.net";

            if (string.IsNullOrEmpty(url))
            {
                return new BadRequestObjectResult("Please pass a URL on the query string");
            }

            try
            {
                HttpResponseMessage response = await httpClient.GetAsync(url);
                return new OkObjectResult($"Status Code for {url}: {response.StatusCode}");
            }
            catch (HttpRequestException e)
            {
                log.LogError($"Error fetching website: {e.Message}");
                // Return an appropriate error result
                return new StatusCodeResult(StatusCodes.Status503ServiceUnavailable);
            }
        }
    }
}
