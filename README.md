Project name: NoteApi

Stack: ASP.NET Core Minimal API, EF Core, PostgreSQL, SwaggerUI

List of endpoints:
- GET /notes 
- GET /notes/{id}
- POST /notes
- PUT /notes/{id}
- DELTE /notes/{id}

Steps to launch:

1. What needs to be installed in advance: .NET SDK and PostgreSQL.

2. Download the project: use the git clone command and provide the address of your repository.

3. Create the appsettings.Development.json file with the connection string. The file is intentionally missing from the repository, so the reader must know that they need to create it themselves.

4. Apply migrations: dotnet ef database update (you need the dotnet-ef tool, which is installed using the command dotnet tool install --global dotnet-ef).

5. Run: dotnet run.

6. Open Swagger in the browser: the address will be in the form https://localhost:port/swagger; the port will be printed in the console at startup.
