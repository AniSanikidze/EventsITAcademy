# EventsITAcademy

EventsITAcademy is a web application implemented using ASP.NET WEB API and ASP.NET MVC. The application enables users to create/modify events or reserve/buy tickets of an existing event. Additionally, the application has an admin panel with the funtionalities of managing user roles,
approving/modifying an existing event and related informations. The application makes a use of the background services, which ensures archiving of already held events 
and removes ticket reservation if the purchase was not made in a specified period of time.

## Architecture
The application's REST API follows clean architecture principles, separating Presentation(API Controllers), Application(Domains and Services of the API) and 
Infrastructure(Persistence - DB layer and Repositoy implementations) layers.

Besides, the application follows SOLID principles and uses repository pattern.

The ASP.NET MVC technology was used to implement the UI of the application, the controllers implemented in MVC's presentation layer refer to the application layer 
for making calls and changes in database. The REST API is seprated from MVC and can be used to integrate with other UIs.

## Features
- Auth: The API supports authentication and authorization features and uses JWT Token for validating the user requests
- Admin panel: Management of events and application users
- Background jobs: Background services used for automatically archiving events and removing reservations
- Role based authorization: ASP.NET Identity is used for implementing role base authorization both in MVC and REST API. The application has 3 defined users: admin, user and moderator.
- Database seeding
- Global exception handler middleware
- Request, response and error logging middleware
- Api Versioning
- Health checks - Health check is added for checking REST API's connection with mssql server

## Technologies
- Web Api Core, MVC Core
- ORM – Entity Framework Core Code First, (Seed, Migration)
- Swagger Documentation
- MSSQL Server
- Fluent Validation for request models
- Data annotation validation for MVC Presentation layer
- Mapster
- JWT Token
- ASP.NET Identity
- Swagger Documentation
- MSSQL Server
- Workers (Background services)
- Unit tests
  - XUnit
  - Moq
  
