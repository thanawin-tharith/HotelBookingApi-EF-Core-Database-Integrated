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

### HTTP Responses

The API uses the following HTTP responses:

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

Use the displayed HTTPS address as the base URL when testing the endpoints.

## Testing the API

You can test the API using Windows PowerShell, Postman, or another HTTP client. The examples below use PowerShell's `Invoke-RestMethod`.

First, make sure the application is running. Set the base URL to the HTTPS address shown in your terminal. Replace the example port with your application's actual HTTPS port.

```powershell
$baseUrl = "https://localhost:7031"
```

### 1. Check the API

**GET `/`**

Checks that the application is running and responds with a welcome message.

```powershell
Invoke-RestMethod -Uri "$baseUrl/" -Method Get
```

Expected response:

```text
Welcome to Bunny Hotel!
```

### 2. Retrieve All Hotels

**GET `/hotels`**

Retrieves all hotel records currently stored in the database.

```powershell
Invoke-RestMethod -Uri "$baseUrl/hotels" -Method Get
```

- `200 OK` — Returns an array of hotel records.
- If the database contains no hotels, the API returns an empty array (`[]`).

### 3. Retrieve a Hotel by ID

**GET `/hotels/{id}`**

Retrieves one hotel using its ID. Replace `1` with an ID that exists in your database.

```powershell
Invoke-RestMethod -Uri "$baseUrl/hotels/1" -Method Get
```

- `200 OK` — Returns the matching hotel.
- `404 Not Found` — No hotel with the specified ID exists.

### 4. Create a Hotel

**POST `/hotels`**

Creates a new hotel record and saves it to the SQL Server database.

```powershell
$body = @{
    name = "Moonlight Hotel"
    address = "Pattaya, Thailand"
    description = "A relaxing beachfront hotel."
    rating = 4.6
} | ConvertTo-Json

Invoke-RestMethod -Uri "$baseUrl/hotels" -Method Post -Body $body -ContentType "application/json"
```

Request body fields:

| Field | Type | Description |
|---|---|---|
| `name` | String | Hotel name |
| `address` | String | Hotel address |
| `description` | String | Hotel description |
| `rating` | Number | Hotel rating, from 0 to 5 |

Validation rules:

- Name, address, and description must not be empty or contain only whitespace.
- Rating must be between `0` and `5`, inclusive.
- The ID is generated by the database; it should not be included in the request body.

Responses:

- `201 Created` — The hotel was successfully created. The response includes the newly created hotel record and its generated ID.
- `400 Bad Request` — The request contains invalid data.

### 5. Update a Hotel

**PUT `/hotels/{id}`**

Updates an existing hotel record. Replace `1` with the ID of the hotel you want to update.

```powershell
$body = @{
    name = "Moonlight Hotel Updated"
    address = "Jomtien Beach, Pattaya, Thailand"
    description = "An updated description for the hotel."
    rating = 4.8
} | ConvertTo-Json

Invoke-RestMethod -Uri "$baseUrl/hotels/1" -Method Put -Body $body -ContentType "application/json"
```

The request must include all four fields: name, address, description, and rating. This endpoint updates the provided hotel record; it does not support partial updates.

The same validation rules as the POST endpoint apply:

- Name, address, and description must not be empty or contain only whitespace.
- Rating must be between `0` and `5`, inclusive.

Responses:

- `200 OK` — The hotel was successfully updated. The response contains the updated record.
- `400 Bad Request` — The request contains invalid data.
- `404 Not Found` — No hotel with the specified ID exists.

### 6. Delete a Hotel

**DELETE `/hotels/{id}`**

Permanently deletes a hotel record from the database. Replace `1` with the ID of the hotel you want to delete.

```powershell
Invoke-RestMethod -Uri "$baseUrl/hotels/1" -Method Delete
```

Responses:

- `204 No Content` — The hotel was successfully deleted. No response body is returned.
- `404 Not Found` — No hotel with the specified ID exists.

**Note:** Deletion is permanent. Make sure you are using the intended hotel ID.

### Testing Notes

- Keep the API application running while sending requests.
- Ensure SQL Server is running and the connection string is configured correctly.
- Use the HTTPS port displayed by your application instead of assuming the example port is correct.
- POST and PUT requests require a JSON request body with all four fields.
- Changes made through POST, PUT, and DELETE are saved to the SQL Server database and persist after the application is restarted.

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
