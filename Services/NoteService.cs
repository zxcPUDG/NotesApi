using Microsoft.EntityFrameworkCore;
using NotesApi.Dtos;


namespace NotesApi.Service;

public class NoteService : INoteService
{
    private readonly AppDbContext _db;

    public NoteService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<List<NoteDto>> GetAllAsync(string? search)
    {
        string normalizedSearch = search?.Trim().ToLower() ?? "";

        return await _db.Notes
            .Where(note => note.Title.ToLower().Contains(normalizedSearch))
            .Select(note => new NoteDto(note.Id, note.Title, note.Content, note.CreatedAt))
            .ToListAsync();
    }

    public async Task<NoteDto?> GetByIdAsync(int id)
    {
        Note? note = await _db.Notes.FindAsync(id);
        return note is null ? null : ToDto(note);
    }

    public async Task<NoteDto> CreateAsync(CreateNoteDto input)
    {
        Note note = new Note
        {
            Title = input.Title,
            Content = input.Content,
            CreatedAt = DateTime.UtcNow
        };

        _db.Notes.Add(note);
        await _db.SaveChangesAsync();

        return ToDto(note);
    }

    public async Task<bool> UpdateAsync(int id, CreateNoteDto input)
    {
        Note? note = await _db.Notes.FindAsync(id);
        if (note is null) return false;

        note.Title = input.Title;
        note.Content = input.Content;
        await _db.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        Note? note = await _db.Notes.FindAsync(id);
        if (note is null) return false;

        _db.Notes.Remove(note);
        await _db.SaveChangesAsync();

        return true;
    }

    private static NoteDto ToDto(Note note)
        => new NoteDto(note.Id, note.Title, note.Content, note.CreatedAt);
}