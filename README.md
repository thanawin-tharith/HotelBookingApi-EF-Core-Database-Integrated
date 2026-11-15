# Hotel Booking API — EF Core & SQL Server

A database-integrated RESTful API built with **C#, ASP.NET Core Minimal APIs, Entity Framework Core, and Microsoft SQL Server**.

This project extends my previous in-memory Hotel Booking API by replacing temporary in-memory storage with persistent database storage using Entity Framework Core.

The project focuses on understanding how an ASP.NET Core application communicates with a relational database and performs asynchronous CRUD operations.

## Features

- **CRUD Operations:** Create, retrieve, update, and delete hotel records.
- **Database Integration:** Uses Entity Framework Core with Microsoft SQL Server.
- **Asynchronous Operations:** Uses async database operations with `async` and `await`.
- **Input Validation:** Validates required fields and hotel ratings.
- **Database Migrations:** Uses EF Core migrations to manage the database schema.
- **Dependency Injection:** Registers and injects the database context into API endpoints.

## Technologies

| Technology | Purpose |
|---|---|
| C# | Programming language |
| .NET 10 | Development platform |
| ASP.NET Core Minimal APIs | HTTP API development |
| Entity Framework Core 10 | Object-relational mapping (ORM) |
| Microsoft SQL Server | Relational database |
| NuGet | Package management |

## API Endpoints

Base URL: `https://localhost:<port>`

| Method | Endpoint | Description |
|---|---|---|
| GET | `/` | Welcome message |
| GET | `/hotels` | Retrieve all hotels |
| GET | `/hotels/{id}` | Retrieve a hotel by ID |
| POST | `/hotels` | Create a new hotel |
| PUT | `/hotels/{id}` | Update an existing hotel |
| DELETE | `/hotels/{id}` | Delete a hotel |

### Example Request

**POST `/hotels`**

Request body:

```json
{
  "name": "Moonlight Hotel",
  "address": "Pattaya, Thailand",
  "description": "A relaxing beachfront hotel.",
  "rating": 4.6
}
```

The API validates the required fields and ensures the rating is between 0 and 5 before saving the new hotel record.

### HTTP Responses

The API uses appropriate HTTP responses, including:

- `200 OK` — Successful retrieval or update.
- `201 Created` — Hotel successfully created.
- `204 No Content` — Hotel successfully deleted.
- `400 Bad Request` — Invalid request data.
- `404 Not Found` — Requested hotel does not exist.

## Getting Started

### Prerequisites

Before running the project, make sure you have:

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- [Microsoft SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)
- [SQL Server Management Studio (SSMS)](https://learn.microsoft.com/en-us/ssms/install/install)
- EF Core command-line tools (`dotnet-ef`)

### 1. Clone the Repository

```bash
git clone https://github.com/thanawin-tharith/HotelBookingApi-EF-Core-Database-Integrated.git
```

Navigate to the solution directory:

```bash
cd HotelBookingApi-EF-Core-Database-Integrated/HotelBookingAPI
```

### 2. Restore Dependencies

```bash
dotnet restore
```

This downloads and resolves the NuGet packages required by the project.

### 3. Configure the Database Connection

The application reads its database connection string from the `DefaultConnection` configuration.

You can configure it using an environment variable.

For Windows PowerShell:

```powershell
$env:ConnectionStrings__DefaultConnection = "Server=.\SQLEXPRESS;Database=HotelBookingDB;Trusted_Connection=True;TrustServerCertificate=True;"
```

Replace the server name and authentication settings with values appropriate for your SQL Server installation.

Alternatively, you can configure the connection string in `appsettings.json`.

**Note:** Do not commit personal credentials or sensitive connection information to a public repository.

### 4. Apply Database Migrations

Make sure the EF Core command-line tool is available. If it is not installed:

```bash
dotnet tool install --global dotnet-ef
```

Apply the existing migrations to create the database schema:

```bash
dotnet ef database update --project HotelBookingAPI/HotelBookingAPI.csproj
```

This creates the database and the required `Hotels` table according to the project's migration history.

### 5. Run the Application

```bash
dotnet run --project HotelBookingAPI/HotelBookingAPI.csproj
```

The application will display its local HTTP and HTTPS addresses in the terminal.

Use the displayed address as the base URL when testing the endpoints.

## Testing the API

You can test the API using PowerShell, Postman, or another HTTP client.

For example, to retrieve all hotel records:

```powershell
Invoke-RestMethod -Uri "https://localhost:7031/hotels" -Method Get
```

To create a hotel:

```powershell
$body = @{
    name = "Moonlight Hotel"
    address = "Pattaya, Thailand"
    description = "A relaxing beachfront hotel."
    rating = 4.6
} | ConvertTo-Json

Invoke-RestMethod -Uri "https://localhost:7031/hotels" -Method Post -Body $body -ContentType "application/json"
```

Replace the example port with the HTTPS port displayed by your application.

You can then test the remaining operations:

- GET `/hotels/{id}` to retrieve a specific hotel.
- PUT `/hotels/{id}` to update an existing hotel.
- DELETE `/hotels/{id}` to delete a hotel.

## Project Structure

```text
HotelBookingAPI/
├── HotelBookingAPI.sln
└── HotelBookingAPI/
    ├── Data/
    │   └── HotelDbContext.cs
    ├── Migrations/
    ├── Models/
    │   ├── Hotel.cs
    │   └── CreateHotelRequest.cs
    ├── Properties/
    ├── Program.cs
    ├── appsettings.json
    └── HotelBookingAPI.csproj
```

## What I Learned

Through this project, I explored:

- Integrating Microsoft SQL Server with an ASP.NET Core application.
- Using Entity Framework Core and `DbContext` to interact with relational databases.
- Configuring dependency injection for database access.
- Managing database schemas with EF Core migrations.
- Implementing asynchronous database operations.
- Handling HTTP requests, validation, and responses in a database-integrated API.

## Scope

This is a learning-focused hotel management API. It manages hotel records and demonstrates database-backed CRUD functionality. It does not currently implement hotel reservations, booking transactions, authentication, or authorization.
