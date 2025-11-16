Spraxa Assignment – Setup & Installation Guide
📌 Project Overview

This repository contains two projects:

AssignmentSpraxa.API – .NET Core REST API

AssignmentSpraxa.Portal – MVC Portal Application

The project uses SQL Server, Entity Framework Core, and a complete role-based authentication workflow (Admin/User). A Postman collection is also included for API testing.

🔗 Clone Repository
https://github.com/adityagound6/Spraxa-Assignment.git

🛠 Requirements
Visual Studio
SQL Server
.NET SDK
Postman

📂 Database Setup (SQL Server)
Create a new database in SQL Server (any name).
Open the file:
AssignmentSpraxa.API/appsettings.json

Update your connection string:
"ConnectionStrings": {
  "DefaultConnection": "Data Source=YOUR_SERVER;Initial Catalog=YOUR_DATABASE;Integrated Security=True;Trust Server Certificate=True"
}

Replace:
YOUR_SERVER → SQL Server name
YOUR_DATABASE → Created database
⚙️ Apply Migrations (EF Core)

Open Package Manager Console and run and select default project AssignmentSpraxa.API:
Add-Migration "Migration Name"
Update-Database

This will create all database tables.

▶️ Run the Solution
Start both projects together:
AssignmentSpraxa.API
AssignmentSpraxa.Portal

👤 Create the First Admin User (Using Postman)
Import the Postman collection included in the repository.
Use:
POST /api/Auth/register
Sample body:
{
  "fullName": "your full name",
  "externalId": "your external id or unique key",
  "status": true,
  "userName": "your username",
  "email": "your email address",
  "password": "your password",
  "phoneNumber": "your phone number",
  "provider": "local",
  "roles": ["Admin"]
}


Keep the role as Admin for the first user.
🧪 API Testing
Base URL
https://localhost:7278/

You can also register directly through Swagger.
To access protected endpoints, login first and use the generated JWT token.

🌐 Portal Access
Open the portal:
https://localhost:7219/

Roles and Permissions
Admin

Manage medicines (Create/Update/Delete)
View & manage all users
Update user roles
Delete users
User
View own dashboard
View login history
Search & view medicine details
Limited access to user data
Users cannot be added directly. They must sign up, after which Admin can update their role.
