# NoteApi
Hi! My name is Adele, I’m 18 years old, and I’m a backend developer.
A simple REST API for notes. You can create, read, update, delete and search notes.

## Tech stack

- ASP.NET Core Minimal API
- Entity Framework Core (EF Core)
- PostgreSQL (with the Npgsql provider)
- Swagger UI for testing the API

## Features

- CRUD operations for notes
- Search notes by title (not case-sensitive): `GET /notes?search=text`
- DTOs: the API accepts and returns only the needed fields
- Input check: the title is required
- Database migrations with EF Core

## Endpoints

| Method | URL | Description | Success response |
|---|---|---|---|
| GET | `/notes` | Get all notes. Optional: `?search=text` | 200 OK |
| GET | `/notes/{id}` | Get one note | 200 OK (or 404 Not Found) |
| POST | `/notes` | Create a note | 201 Created (or 400 Bad Request) |
| PUT | `/notes/{id}` | Update a note | 204 No Content (or 400 / 404) |
| DELETE | `/notes/{id}` | Delete a note | 204 No Content (or 404) |

Example request body for `POST /notes` and `PUT /notes/{id}`:

```json
{
  "title": "Buy bread",
  "content": "At the shop"
}
```

Example response for `GET /notes/1`:

```json
{
  "id": 1,
  "title": "Buy bread",
  "content": "At the shop",
  "createdAt": "2026-10-17T18:56:09Z"
}
```

## Getting started

### Requirements

- .NET SDK (the target version is in `NotesApi.csproj`)
- PostgreSQL
- Git

### Steps

1. Clone the repository:

   ```bash
   git clone https://github.com/zxcPUDG/NotesApi.git
   cd NotesApi
   ```

2. Create the file `appsettings.Development.json` in the project folder and add your connection string:

   ```json
   {
     "ConnectionStrings": {
       "Default": "Host=localhost;Port=5432;Database=notesdb;Username=postgres;Password=YOUR_PASSWORD"
     }
   }
   ```

   This file is not in the repository because it contains a password. Git ignores it.

3. Install the EF Core tool (one time):

   ```bash
   dotnet tool install --global dotnet-ef
   ```

4. Create the database and the tables:

   ```bash
   dotnet ef database update
   ```

5. Run the application:

   ```bash
   dotnet run
   ```

6. Open Swagger in your browser: the address from the console plus `/swagger`, for example `https://localhost:7000/swagger`. Swagger is enabled only in the Development environment, which `dotnet run` uses by default.

## Project structure

```text
NotesApi/
├── Program.cs         # application setup and endpoints
├── Note.cs            # the Note entity
├── AppDbContext.cs    # EF Core database context
├── Migrations/        # database migrations
└── appsettings.json   # settings (the password is a placeholder)
```

## Roadmap

- Layered architecture (controllers, services, repositories)
- Authentication with JWT
- Unit and integration tests
- Docker and CI (GitHub Actions)