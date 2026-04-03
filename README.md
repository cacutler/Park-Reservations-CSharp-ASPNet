# Parks Reservation Project

This is a C# ASP.Net web app built to allow users to reserve city parks.

## Functionality

- City admins are able to grant city admin access to other users for their city
- City admins are able to approve/deny reservation requests within city parks they oversee
- All users are able to make, update, and delete their own reservation requests, along with seeing current and past reservations and associated details
- All users are able to select parks and the associated availability/schedule (allow them to select a park from another city)
- City admins are able to create and update parks along with their availability/schedule
- City admins are not able to approve their own reservation requests (have other city admins approve those requests)

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
| Approve or deny a reservation     | PATCH  | /api/reseravtions/1/status | ReservationsController | Admin  | Reservation Status (sent as just a number)     |
| Grant city admin access           | POST   | /api/admin/grant           | AdminController        | Admin  | User ID                                        |
| Revoke city admin access          | DELETE | /api/admin/revoke/userId   | AdminController        | Admin  | N/A                                            |
| Get all cities                    | GET    | /api/cities                | CitiesController       | Public | N/A                                            |

## Database Design

**Users**

- ID (primary key)
- Name
- Email
- Username
- Password
- Phone Number
- City Admin/Admin for City (specifier for being an admin for a specific city)

**Parks**

- ID (primary key)
- City ID (foreign key)
- Name
- Address
- Schedule

**Reservations**

- ID (primary key)
- User ID (foreign key)
- Park ID (foreign key)
- Date
- Time
- Name
- Email
- Phone Number
- Status

**Cities**

- ID (primary key)
- Name
- County
- State