using NotesApi.Dtos;
using NotesApi.Service;

namespace NotesApi.Endpoints;

public static class NoteEndpoints
{
    public static void MapNoteEndpoints(this WebApplication app)
    {
        app.MapGet("/notes", async (string? search, INoteService service) =>
        {
            List<NoteDto> result = await service.GetAllAsync(search);
            return Results.Ok(result);
        });

        app.MapGet("/notes/{id}", async (int id, INoteService service) =>
        {
            if (id <= 0) return Results.BadRequest("id должен быть больше 0");

            NoteDto? result = await service.GetByIdAsync(id);
            return result is not null ? Results.Ok(result) : Results.NotFound();
        });

        app.MapPost("/notes", async (CreateNoteDto input, INoteService service) =>
        {
            if (string.IsNullOrWhiteSpace(input.Title)) return Results.BadRequest("Title не может быть пустым");

            NoteDto result = await service.CreateAsync(input);
            return Results.Created($"/notes/{result.Id}", result);
        });

        app.MapPut("/notes/{id}", async (int id, CreateNoteDto input, INoteService service) =>
        {
            if (string.IsNullOrWhiteSpace(input.Title)) return Results.BadRequest("Title не может быть пустым");
            if (id <= 0) return Results.BadRequest("id должен быть больше 0");

            bool updated = await service.UpdateAsync(id, input);
            return updated ? Results.NoContent() : Results.NotFound();
        });

        app.MapDelete("/notes/{id}", async (int id, INoteService service) =>
        {
            if (id <= 0) return Results.BadRequest("id должен быть больше 0");

            bool deleted = await service.DeleteAsync(id);
            return deleted ? Results.NoContent() : Results.NotFound();
        });
    }
}