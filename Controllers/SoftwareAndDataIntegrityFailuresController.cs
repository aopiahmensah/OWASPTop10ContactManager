using Humanizer;
using Microsoft.AspNetCore.Mvc;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Model;
using static System.Net.Mime.MediaTypeNames;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Threading.Channels;
using Newtonsoft.Json;
using Microsoft.AspNetCore.DataProtection;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Diagnostics.Metrics;
using System.IdentityModel.Tokens.Jwt;
namespace OWASPContactManager.Controllers
{
    public class SoftwareAndDataIntegrityFailuresController : Controller
    {
        //        Software and data integrity failures occur when an application fails to protect against unauthorized changes to software or data.
        //            This can happen through untrusted data deserialization, using untrusted software libraries, or improper validation of data integrity.
        //        Here's a sample code block that demonstrates a vulnerable scenario due to untrusted data deserialization and then shows a secure way to handle deserialization.
        //Vulnerable Code Example

        //Vulnerable Code: Untrusted Deserialization
        [HttpPost]
        public IActionResult ProcessData(string jsonData)
        {
            try
            {
                // Vulnerable: Directly deserializing untrusted input
                var data = JsonConvert.DeserializeObject<MyData>(jsonData);
                // Process data...
                return Ok();
            }
            catch (Exception ex)
            {
                // Handle exception
                return BadRequest();
            }
        }

        //        Secure Code Example

        //Secure Code: Validating and Securing Deserialization
        [HttpPost]
        public IActionResult ProcessDataSecure(string jsonData)
        {
            try
            {
                // Secure: Validate the JSON structure before deserialization
                if (!IsValidJson(jsonData))
                {
                    return BadRequest("Invalid JSON data");
                }

                // Secure: Use a safe and restricted deserialization approach
                var settings = new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.None,
                    // Additional security settings can be added here
                };

                var data = JsonConvert.DeserializeObject<MyData>(jsonData, settings);

                // Validate the deserialized object
                if (data == null || !IsValidData(data))
                {
                    return BadRequest("Invalid data");
                }

                // Process data...
                return Ok();
            }
            catch (Exception ex)
            {
                // Handle exception
                return BadRequest();
            }
        }

        private bool IsValidJson(string jsonData)
        {
            try
            {
                var obj = JToken.Parse(jsonData);
                return true;
            }
            catch (JsonReaderException)
            {
                return false;
            }
        }

        private bool IsValidData(MyData data)
        {
            // Implement your validation logic here
            // Example: Ensure Name is not null and Age is within a valid range
            return !string.IsNullOrWhiteSpace(data.Name) && data.Age > 0 && data.Age < 120;
        }

    }

    public class MyData
    {
        public string? Name { get; set; }
        public int Age { get; set; }
    }
    //    Explanation

    //    Vulnerable Code:
    //        The vulnerable code directly deserializes the untrusted jsonData input without any validation.This can lead to various security issues, including deserialization attacks.

    //    Secure Code:
    //        The secure code first validates the JSON structure using IsValidJson method.
    //    It uses a restricted JsonSerializerSettings to avoid insecure deserialization features like TypeNameHandling.
    //    The deserialized object is further validated using IsValidData method to ensure it meets the expected criteria before processing.

    //By following these secure practices, one can prevent software and data integrity failures related to untrusted data deserialization in your applications.
}
