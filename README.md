# Entertainment Company Backend API

A .NET 8 Web API service for managing entertainment venue operations, including user management, ride tickets, and game credits.

## Features

- User Management
- Ride Ticket System (purchase and redemption)
- Game Credit System (purchase and usage)
- Swagger UI for API documentation and testing
- Comprehensive unit test coverage
- SQLite database with Entity Framework Core

## Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- An IDE (Visual Studio 2022 recommended)
- Git (for cloning the repository)

## Getting Started

### 1. Clone the Repository

```bash
git clone https://github.com/brianscott81/Demo-Entertainment-Company-Backend-API.git
cd Demo-Entertainment-Company-Backend-API
```

### 2. Database Setup

The application uses SQLite with Entity Framework Core. The database will be created automatically on first run. The connection string is configured in `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=entertainment.db"
  }
}
```

### 3. Running the Application

#### Using Visual Studio 2022
1. Open `Demo Entertainment Company Backend API.sln`
2. Set `Demo Entertainment Company Backend API` as the startup project
3. Press F5 or click the Run button

#### Using Command Line
```bash
dotnet restore
dotnet build
cd "Demo Entertainment Company Backend API"
dotnet run
```

The API will start and be available at:
- HTTPS: https://localhost:7001
- HTTP: http://localhost:5001

## API Documentation

### Swagger UI
Access the Swagger UI documentation at:
- https://localhost:7001/swagger (when running with HTTPS)
- http://localhost:5001/swagger (when running with HTTP)

### Available Endpoints

#### User Management
- `POST /api/User` - Create a new user
- `GET /api/User` - Get all users
- `GET /api/User/{id}` - Get user by ID

#### Ride Tickets
- `POST /api/RideTickets/add` - Add tickets to user
  ```json
  {
    "userId": 1,
    "amount": 5
  }
  ```
- `POST /api/RideTickets/deduct` - Deduct tickets from user
  ```json
  {
    "userId": 1,
    "amount": 2
  }
  ```

#### Game Credits
- `POST /api/GameCredits/add` - Add credits to user
  ```json
  {
    "userId": 1,
    "amount": 10
  }
  ```
- `POST /api/GameCredits/deduct` - Deduct credits from user
  ```json
  {
    "userId": 1,
    "amount": 5
  }
  ```

## Development

### Project Structure
- `Controllers/` - API endpoints
- `Models/` - Data models and DTOs
- `Services/` - Business logic implementation
- `Migrations/` - Database migrations

### Running Tests

The solution includes comprehensive unit tests for all controllers and services. To run tests:

```bash
dotnet test
```

Test projects cover:
- Controller functionality
- Service layer business logic
- Database operations
- Error handling

## Technologies Used

- ASP.NET Core 8.0
- Entity Framework Core
- SQLite
- Swagger/OpenAPI
- NUnit (for testing)
- Entity Framework Core In-Memory Database (for testing)
