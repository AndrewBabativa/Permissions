# Permissions App

This application is a web API developed to meet the requirements specified by N5 company for registering user permissions. It implements several features and technologies to ensure a robust and efficient solution.

## Installation

To install the application, follow these steps:

1. Clone the repository from GitHub.
2. Open the solution in Visual Studio.
3. Build the solution to restore dependencies.
4. Configure the database connection string in the `appsettings.json` file.
5. Run database migrations to create the necessary tables.
6. Start the application.

## Features

- **Permissions Table**: A table was created with fields such as ID, EmployeeForename, EmployeeSurname, PermissionsDate, and PermissionsType.
- **Permission Types Table**: Another table was created to store permission types with fields such as ID, TypeName, and Description.
- **Entity Framework**: Entity Framework was used to access the database and perform CRUD operations.
- **CQRS Pattern**: The application implements the Command Query Responsibility Segregation (CQRS) pattern to separate read and write operations.
- **Repository Pattern**: The repository pattern was used to abstract data access logic, making it easier to switch between different data sources.
- **Unit of Work**: The Unit of Work pattern was used to manage transactions and ensure data consistency.
- **Apache Kafka**: Apache Kafka was integrated into the application to publish messages for each operation performed (e.g., create, update, delete).
- **Elasticsearch**: Elasticsearch was used to index and search permission records. Each time a permission is created or updated, a corresponding record is indexed in Elasticsearch.
- **Serilog**: Serilog was used as a logging library to log information about each API endpoint and operation.
- **Unit Testing**: Unit tests were implemented to test the functionality of the Request Permission, Modify Permission, and Get Permissions services.
- **Integration Testing**: Integration tests were also implemented to ensure that the services work correctly together.

## Database Structure

![image](https://github.com/AndrewBabativa/Permissions/assets/49515410/58a32172-4654-4a32-9b54-68d0cd3b085f)

## Usage

The application exposes several endpoints:

- `POST /api/Permission/RequestPermission`: Creates a new permission record.
- `PUT /api/Permission/ModifyPermission/{id}`: Modifies an existing permission record.
- `GET /api/Permission/GetPermissions`: Gets all permission records.
- `POST /api/Permission/CreatePermissionType`: Creates a new permission type.
- `GET /api/Permission/GetPermissionTypes`: Gets all permission types.

## Technologies Used

- .NET Core
- Entity Framework Core
- CQRS Pattern
- Repository Pattern
- Apache Kafka
- Elasticsearch
- Serilog
- xUnit

## Logging and Message Printing

The application logs information using Serilog. Information about each API endpoint and operation is logged. Additionally, messages for each operation performed are printed in the console. Here's an example of the message structure:

Serilog: Information logged by Serilog is displayed in the console. This includes details about each API endpoint called and the operations performed, providing valuable insights into the application's behavior.

Elasticsearch: Whenever a permission is created or updated, a corresponding record is indexed in Elasticsearch. These indexing operations are also printed in the console, showing the data being stored in Elasticsearch.

Apache Kafka: Apache Kafka is used to publish messages for each operation performed. These messages are printed in the console, showing the operation type (e.g., create, update, delete) and a random GUID identifier for each message.

This logging and message printing mechanism helps developers monitor the application's activity and troubleshoot any issues that may arise during operation.

![image](https://github.com/AndrewBabativa/Permissions/assets/49515410/b86c4109-d920-4e03-90b0-03ac7f392556)


## Credits

This application was developed by Andrés Babativa Goyeneche as part of a personal project.
