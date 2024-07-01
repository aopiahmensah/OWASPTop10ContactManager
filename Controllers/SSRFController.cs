using Humanizer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Elfie.Serialization;
using NuGet.DependencyResolver;
using System.Reflection.Metadata;
using System.Security.Policy;
using System;
using static System.Net.WebRequestMethods;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Microsoft.AspNetCore.DataProtection;
using Azure.Core;
using Azure;
using static System.Net.Mime.MediaTypeNames;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;

namespace OWASPContactManager.Controllers
{
    public class SSRFController : Controller
    {
        //Server-Side Request Forgery(SSRF) occurs when an attacker can manipulate a server-side application to make HTTP requests to arbitrary domains chosen by the attacker.
        //    This can lead to unauthorized actions or access to sensitive data within the internal network.
        //        Below is an example demonstrating a vulnerable code block and a secure code block that prevents SSRF.

        private readonly HttpClient _httpClient;

        public SSRFController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }


        //Vulnerable Code Example
        //In the below vulnerable code, the FetchData action takes a url parameter from the user and makes a request to that URL without any validation, 
        //    which can be exploited by an attacker to perform SSRF.
        [HttpGet]
        public async Task<IActionResult> FetchData(string url)
        {
            // Vulnerable: Directly using user-provided URL
            var response = await _httpClient.GetStringAsync(url);
            return Content(response);
        }

        //Secure Code Example

        //To prevent SSRF, you can validate the user-provided URL against a whitelist of allowed domains or use more restrictive URL handling.
        [HttpGet]
        public async Task<IActionResult> FetchDataSecure(string url)
        {
            if (!IsValidUrl(url))
            {
                return BadRequest("Invalid URL");
            }

            try
            {
                var response = await _httpClient.GetStringAsync(url);
                return Content(response);
            }
            catch (Exception ex)
            {
                // Handle the error appropriately
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        private bool IsValidUrl(string url)
        {
            // Allowlist of domains
            var allowedDomains = new[] { "example.com", "trustedsite.com" };

            if (Uri.TryCreate(url, UriKind.Absolute, out var uri))
            {
                return Array.Exists(allowedDomains, domain => uri.Host.EndsWith(domain, StringComparison.OrdinalIgnoreCase));
            }
            return false;
        }
    }
    //    Explanation

    //    IsValidUrl Method:
    //        This method checks if the user-provided URL belongs to an allowlist of domains.It ensures that only URLs from trusted domains are processed.
    //        The Uri.TryCreate method is used to parse the URL, and then the host part is validated against the allowed domains.

    //    FetchData Action:
    //        The FetchData action first calls IsValidUrl to validate the provided URL.
    //        If the URL is valid, it proceeds to make the HTTP request.
    //        If the URL is invalid, it returns a BadRequest response.

    //    Error Handling:
    //        The code includes basic error handling to catch exceptions that may occur during the HTTP request and respond with a 500 Internal Server Error status.

    //By implementing such validation, developers can mitigate the risk of SSRF attacks and ensure that their application only makes HTTP requests to trusted and known domains.
}
