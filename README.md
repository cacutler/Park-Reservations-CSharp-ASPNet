# Parks Reservation Project

This is a C# ASP.Net web app built to allow users to reserve city parks.

## Functionality

- City admins are able to grant city admin access to other users for their city
- City admins are able to approve/deny reservation requests within city parks they oversee
- All users are able to make, update, and delete their own reservation requests, along with seeing current and past reservations and associated details
- All users are able to select parks and the associated availability/schedule (allow them to select a park from another city)
- City admins are able to create and update parks along with their availability/schedule
- City admins are not able to approve their own reservation requests (have other city admins approve those requests)

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