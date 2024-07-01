using Humanizer;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json.Linq;
using OWASPTaskManager.Models;
using System.IO;
using System.Net.NetworkInformation;

namespace OWASPTaskManager.Controllers
{
    public class InjectionController : Controller
    {
        private const string connectionString = "";

        //Vulnerable Code Example(SQL Injection)
        //This code directly concatenates user input into a SQL query string, which is susceptible to SQL Injection attacks.
        //The user input(userId) is directly concatenated into the SQL query string.
        //This makes the query vulnerable to SQL Injection attacks if an attacker inputs malicious SQL code.
        // Vulnerable SQL Injection endpoint
        public IActionResult GetTasks(string userId)
        {
            var query = $"SELECT * FROM Tasks WHERE UserId = '{userId}'";
            var tasks = new List<Tasks>();
            using (var connection = new SqlConnection(connectionString))
            {
                var command = new SqlCommand(query, connection);
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        tasks.Add(new Tasks { Id = reader.GetInt32(0), Name = reader.GetString(1), UserId = reader.GetString(2) });
                    }
                }
            }
            return Ok(tasks);
        }

        //Secure Code Example(Using Parameterized Queries)
        //This code uses parameterized queries to safely handle user input, mitigating the risk of SQL Injection attacks.
        //The user input is passed as a parameter to the SQL query using SqlCommand.Parameters.AddWithValue.
        //This approach ensures that the input is properly escaped and treated as a literal value rather than executable SQL code,
        //thus preventing SQL Injection attacks.
        // Secure SQL Injection endpoint
        public IActionResult GetTasksSecure(string userId)
        {
            var tasks = new List<Tasks>();
            using (var connection = new SqlConnection(connectionString))
            {
                var command = new SqlCommand("SELECT * FROM Tasks WHERE UserId = @UserId", connection);
                command.Parameters.AddWithValue("@UserId", userId);
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        tasks.Add(new Tasks { Id = reader.GetInt32(0), Name = reader.GetString(1), UserId = reader.GetString(2) });
                    }
                }
            }
            return Ok(tasks);
        }

        //Example of Vulnerability Exploitation
        //If the userId input is "1'; DROP TABLE Tasks;--", the vulnerable code would generate the following SQL query:
        //SELECT* FROM Tasks WHERE UserId = '1'; DROP TABLE Tasks;--
        //This query would execute the SELECT statement and then drop the Tasks table, causing data loss.The secure version using parameterized queries would treat the input as a literal string, preventing such an attack.

    }
}
