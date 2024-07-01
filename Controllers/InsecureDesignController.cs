using Humanizer;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;
using OWASPTaskManager.Models;
using System.Buffers.Text;
using System.Reflection.Metadata;
using System.Security.Claims;
using System.Security.Policy;
using static System.Net.Mime.MediaTypeNames;
using static System.Runtime.InteropServices.JavaScript.JSType;
namespace OWASPContactManager.Controllers
{
    public class InsecureDesignController : Controller
    {
        private readonly UserManager<IdentityUser> UserManager;
        //Insecure design refers to flaws or weaknesses in the architecture or design of an application that can lead to vulnerabilities.
        //One common example of insecure design is failing to enforce proper access control mechanisms, leading to unauthorized access to sensitive data
        //or functionality.
        //Example of Insecure Design
        //In this example, there is a method that retrieves user data without enforcing proper access control, allowing any authenticated user to
        //access any other user's data. This represents an insecure design flaw.

        // Insecure design: No access control checks
        //    Issues with the Code below

        //No Access Control Checks:
        //    The GetUserDetails method allows any authenticated user to retrieve the details of any user by simply providing the userId.
        //        There are no checks to ensure that the user making the request has the right to access the specified user's details.
        [HttpGet]
        public IActionResult GetUserDetails(int userId)
        {
            var currentUser = UserManager.FindByIdAsync(userId.ToString());
            if (currentUser == null)
            {
                return NotFound();
            }

            return Ok(currentUser);
        }


        //Secure Design

        //To secure this endpoint, we should enforce proper access control checks.One way to do this is by ensuring that a user can only retrieve their own
        //details unless they have administrative privileges.
        // Secure design: Enforcing access control
        [HttpGet]
        public IActionResult GetUserDetailsSecure(int userId)
        {
            // Get the current logged-in user's ID
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Check if the current user is an admin
            var isAdmin = User.IsInRole("Admin");

            // Allow access if the user is an admin or if the requested user ID matches the current user's ID
            if (isAdmin || currentUserId == userId.ToString())
            {
                var user = UserManager.FindByIdAsync(userId.ToString());
                if (user == null)
                {
                    return NotFound();
                }

                return Ok(user);
            }

            // Deny access if the user is not authorized
            return Forbid();
        }

        //    Improvements in the Secure Design

        //Access Control Checks:
        //    The method now checks if the current user is an admin or if they are requesting their own details by comparing the userId parameter with the current user's ID.
        //    If neither condition is met, access is denied by returning a Forbid result.

        //Role-Based Access Control:
        //    The method uses role-based access control to allow administrators to access any user's details.

        //Claims-Based Authentication:
        //    The current user's ID is retrieved using claims-based authentication, ensuring that only authenticated users can make the request.
    }
}
