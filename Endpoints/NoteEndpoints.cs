using NotesApi.Dtos;
using NotesApi.Services;

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
            NoteDto result = await service.GetByIdAsync(id);
            return Results.Ok(result);
        });

        app.MapPost("/notes", async (CreateNoteDto input, INoteService service) =>
        {
            NoteDto result = await service.CreateAsync(input);
            return Results.Created($"/notes/{result.Id}", result);
        });

        app.MapPut("/notes/{id}", async (int id, CreateNoteDto input, INoteService service) =>
        {
            await service.UpdateAsync(id, input);
            return Results.NoContent();
        });

        app.MapDelete("/notes/{id}", async (int id, INoteService service) =>
        {
            await service.DeleteAsync(id);
            return Results.NoContent();
        });
    }
}