# EShop API

## Overview

EShop API is a RESTful web service built with ASP.NET Core that provides endpoints for managing products in an e-commerce application. The API supports versioning and can return data from either a mock repository or a database, depending on the configuration.

## Prerequisites

Before running the application, ensure you have the following installed:

- [.NET SDK 8.0 or later](https://dotnet.microsoft.com/download/dotnet)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) (for database access)
- [Visual Studio 2022](https://visualstudio.microsoft.com/vs/) or any other IDE that supports .NET development

## Getting Started

### Clone the Repository

https://github.com/MartinGaher/Alza.git

1. Open the solution in Visual Studio.
2. Setup connection string in appsettings.json
3. Build the solution to restore dependencies and compile the project.
4. run "Update-Database" in Package Manager Console
5. run application

### Optional 

 - you can setup "UseMockData" in appsettings.json for use DB or mock data
