# OWASPTop10ContactManager
Overview
The "OWASP Top Ten Vulnerability Demonstration" project aims to provide a practical learning platform for developers to understand and mitigate common web application vulnerabilities as identified by the OWASP Top Ten. This project includes a simple contact management web application developed using ASP.NET Core and C#. The application showcases both vulnerable and secure implementations for each of the OWASP Top Ten vulnerabilities, allowing developers to compare and learn effective security practices.

Application Purpose and Architecture

The application is a contact manager where users can:

    Register and log in
    Create, view, edit, and delete contacts

Each contact includes a name, description, and creation date, associated with a specific user.
Architecture

    Frontend: Razor Pages for user interface
    Backend: ASP.NET Core with Entity Framework Core for data handling and business logic
    Database: SQL Server for data storage

Getting Started
Prerequisites

    .NET Core SDK (version X.X or later)
    SQL Server

Installation

    Clone the repository:

    bash

git clone https://github.com/yourusername/owasp-top-ten-demo.git
cd owasp-top-ten-demo

Setup the database:

Update the appsettings.json file with your SQL Server connection string.

Run the application:

bash

    dotnet restore
    dotnet ef database update
    dotnet run

    Open the application:

    Navigate to https://localhost:5001 in your web browser.

Navigating the Vulnerabilities
1. Cryptographic Failures

    Controller: CryptographicFailuresController
    Explore insecure cryptographic practices and their secure alternatives.

2. Identification and Authentication Failures

    Controller: IdentificationAndAuthenticationFailuresController
    Learn about common authentication issues and secure mechanisms.

3. Injection

    Controller: InjectionController
    Review examples of injection attacks and secure coding practices.

4. Insecure Design

    Controller: InsecureDesignController
    Identify and avoid insecure design patterns.

5. Security Logging and Monitoring Failures

    Controller: SecurityLoggingAndMonitoringFailuresController
    Implement effective logging and monitoring.

6. Software and Data Integrity Failures

    Controller: SoftwareAndDataIntegrityFailuresController
    Ensure data integrity and secure software updates.

7. SSRF (Server-Side Request Forgery)

    Controller: SSRFController
    Prevent SSRF vulnerabilities through input validation and sanitization.

8. Security Misconfiguration

    File: SecurityMisconfiguration.cs
    Configure your application securely to mitigate misconfigurations.

9. Vulnerable and Outdated Components

    File: VulnerableAndOutdatedComponents.cs
    Keep your dependencies up-to-date and secure.

Contributing

Contributions are welcome! Please open an issue or submit a pull request for any changes.
License

This project is licensed under the MIT License - see the LICENSE file for details.
Acknowledgments

    OWASP Foundation for their invaluable resources on web application security.
