# Parks Reservation Project

This is a C# ASP.Net web app built to allow users to reserve city parks.

## Technologies Used

- **Framework**: .NET 8.0
- **Web Framework**: ASP.NET Core (with Razor Pages and MVC Controllers)
- **Database**: SQLite with Entity Framework Core
- **Authentication**: JWT Bearer Tokens and ASP.NET Core Identity
- **Frontend**: Razor Pages with Bootstrap and jQuery

## Prerequisites

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- SQLite (automatically handled by Entity Framework Core)

## Installation

1. Clone the repository:
   ```bash
   git clone <repository-url>
   cd park_reservations
   ```

2. Restore dependencies:
   ```bash
   dotnet restore
   ```

3. Apply database migrations:
   ```bash
   dotnet ef database update
   ```

## Running the Application

1. Start the application:
   ```bash
   dotnet run
   ```

2. Open your browser and navigate to `https://localhost:5001` (or the URL shown in the console output).

3. For development, you can also use the Visual Studio debugger or VS Code with the C# extension.

## Functionality

- City admins are able to grant city admin access to other users for their city
- City admins are able to approve/deny reservation requests within city parks they oversee
- All users are able to make, update, and delete their own reservation requests, along with seeing current and past reservations and associated details
- All users are able to select parks and the associated availability/schedule (allow them to select a park from another city)
- City admins are able to create and update parks along with their availability/schedule
- City admins are not able to approve their own reservation requests (have other city admins approve those requests)

## Project Structure

- **Controllers/**: API controllers for handling HTTP requests
- **Data/**: Database context and factory
- **DTOs/**: Data Transfer Objects for API requests/responses
- **Migrations/**: Entity Framework database migrations
- **Models/**: Entity models (User, Park, Reservation, City)
- **Pages/**: Razor Pages for the web interface
- **Services/**: Business logic services
- **wwwroot/**: Static files (CSS, JS, libraries)

## REST Endpoints

| Name                              | Method | Path                       | Controller             | Access | Fields                                         |
| --------------------------------- | ------ | -------------------------- | ---------------------- | ------ | ---------------------------------------------- |
| Register a new user               | POST   | /api/auth/register         | AuthController         | Public | Name, Email, Username, Password, Phone Number  |
| Login                             | POST   | /api/auth/login            | AuthController         | Public | Username, Password                             |
| Get all parks                     | GET    | /api/parks                 | ParksController        | Public | N/A                                            |
| Get filtered parks by city        | GET    | /api/parks?cityId=1        | ParksController        | Public | City ID Parameter                              |
| Get a single park                 | GET    | /api/parks/1               | ParksController        | Public | N/A                                            |
| Create a park                     | POST   | /api/parks                 | ParksController        | Admin  | City ID, Name, Address, Schedule               |
| Update a park                     | PUT    | /api/parks/1               | ParksController        | Admin  | City ID, Name, Address, Schedule               |
| Get own reservations              | GET    | /api/reservations/mine     | ReservationsController | Auth   | N/A                                            |
| Get pending reservations for city | GET    | /api/reservations/pending  | ReservationsController | Admin  | N/A                                            |
| Create a reservation              | POST   | /api/reservations          | ReservationsController | Auth   | Park ID, Date, Time, Name, Email, Phone Number |
| Update a reservation              | PUT    | /api/reservations/1        | ReservationsController | Auth   | Park ID, Date, Time, Name, Email, Phone Number |
| Delete a reservation              | DELETE | /api/reservations/1        | ReservationsController | Auth   | N/A                                            |
| Approve or deny a reservation     | PATCH  | /api/reservations/1/status | ReservationsController | Admin  | Reservation Status (sent as just a number)     |
| Grant city admin access           | POST   | /api/admin/grant           | AdminController        | Admin  | User ID                                        |
| Revoke city admin access          | DELETE | /api/admin/revoke/userId   | AdminController        | Admin  | N/A                                            |
| Get all cities                    | GET    | /api/cities                | CitiesController       | Public | N/A                                            |

### API Usage Examples

#### Authentication
```bash
# Register
curl -X POST https://localhost:5001/api/auth/register \
  -H "Content-Type: application/json" \
  -d '{"name":"John Doe","email":"john@example.com","username":"johndoe","password":"password123","phoneNumber":"123-456-7890"}'

# Login
curl -X POST https://localhost:5001/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{"username":"johndoe","password":"password123"}'
```

#### Parks
```bash
# Get all parks
curl -X GET https://localhost:5001/api/parks

# Get parks by city
curl -X GET "https://localhost:5001/api/parks?cityId=1"
```

## Database Design

**Users**

- ID (primary key)
- Name
- Email
- Username
- Password (hashed)
- Phone Number
- City Admin/Admin for City (foreign key to Cities, nullable for specifying admin role)

**Parks**

- ID (primary key)
- City ID (foreign key to Cities)
- Name
- Address
- Schedule (availability information)

**Reservations**

- ID (primary key)
- User ID (foreign key to Users)
- Park ID (foreign key to Parks)
- Date
- Time
- Name
- Email
- Phone Number
- Status (enum: Pending, Approved, Denied, etc.)

**Cities**

- ID (primary key)
- Name
- County
- State