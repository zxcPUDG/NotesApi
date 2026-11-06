using Microsoft.EntityFrameworkCore;
using NotesApi.Dtos;
using NotesApi.Exceptions;


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

    public async Task<NoteDto> GetByIdAsync(int id)
    {   
        ValidateId(id);
        Note note = await _db.Notes.FindAsync(id) 
            ?? throw new NotFoundException($"Note {id} not found");
        return ToDto(note);
    }

    public async Task<NoteDto> CreateAsync(CreateNoteDto input)
    {   
        ValidateTitle(input.Title);

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

    public async Task UpdateAsync(int id, CreateNoteDto input)
    {   
        ValidateTitle(input.Title);
        ValidateId(id);

        Note note = await _db.Notes.FindAsync(id) 
            ?? throw new NotFoundException($"Note {id} not found");

        note.Title = input.Title;
        note.Content = input.Content;
        await _db.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {   
        ValidateId(id);

        Note note = await _db.Notes.FindAsync(id)
            ?? throw new NotFoundException($"Note {id} not found");

        _db.Notes.Remove(note);
        await _db.SaveChangesAsync();
    }

    private static NoteDto ToDto(Note note)
        => new NoteDto(note.Id, note.Title, note.Content, note.CreatedAt);

    private static void ValidateId(int id)
    {
        if (id <= 0) 
            throw new ValidationException("id должен быть больше нуля");   
    }
    private static void ValidateTitle(string title)
    {
        if (string.IsNullOrWhiteSpace(title)) 
                throw new ValidationException("Title не может быть пустым");
    }

}