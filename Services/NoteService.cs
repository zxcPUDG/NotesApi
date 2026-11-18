using Microsoft.EntityFrameworkCore;
using NotesApi.Dtos;
using NotesApi.Exceptions;
using FluentValidation;

namespace NotesApi.Services;

public class NoteService : INoteService
{
    private readonly AppDbContext _db;
    private readonly ILogger<NoteService> _logger;
    private readonly IValidator<CreateNoteDto> _validator;

    public NoteService(AppDbContext db, ILogger<NoteService> logger, IValidator<CreateNoteDto> validator)
    {
        _db = db;
        _logger = logger;
        _validator = validator;
    }

    public async Task<List<NoteDto>> GetAllAsync(string? search,string[]? tags)
    {   
        IQueryable<Note> query = _db.Notes.AsNoTracking();
        
        if(!string.IsNullOrWhiteSpace(search))
        {
            string normalizedSearch = search.Trim().ToLower();
            query = query.Where(note => note.Title.ToLower().Contains(normalizedSearch));
        }

        List<string> normalizedTags = new List<string>();
        if (tags is not null)
        {
            foreach(var name in tags)
            {
                if (!string.IsNullOrWhiteSpace(name))
                {
                    normalizedTags.Add(name.Trim().ToLower());
                }
            }
            
            if (normalizedTags.Count > 0)
            {
                query = query.Where(note => note.Tags.Any(g => normalizedTags.Contains(g.Name)));
            }
        }

        var notes = await query.Select(note => 
        new NoteDto(
            note.Id, 
            note.Title, 
            note.Content, 
            note.CreatedAt, 
            note.Tags.Select(g => g.Name).ToList())).ToListAsync();

        _logger.LogInformation("Received {Count} notes", notes.Count);

        return notes;
    }

    public async Task<NoteDto> GetByIdAsync(int id)
    {   
        ValidateId(id);
        Note note = await _db.Notes.Include(n => n.Tags).FirstOrDefaultAsync(n =>n.Id == id) 
            ?? throw new NotFoundException($"Note {id} not found");
        
        return ToDto(note);
    }

    public async Task<NoteDto> CreateAsync(CreateNoteDto input)
    {   
        await _validator.ValidateAndThrowAsync(input);
       
        List<Tag> tags = await ResolveTagsAsync(input.Tags);

        Note note = new Note
        {
            Title = input.Title,
            Content = input.Content,
            CreatedAt = DateTime.UtcNow,
            Tags = tags
        };

        _db.Notes.Add(note);
        await _db.SaveChangesAsync();

        _logger.LogInformation("Created note with Id {NoteId}", note.Id);
        
        return ToDto(note);
    }

    public async Task UpdateAsync(int id, CreateNoteDto input)
    {
        await _validator.ValidateAndThrowAsync(input);
        ValidateId(id);

        Note note = await _db.Notes.Include(n => n.Tags).FirstOrDefaultAsync(n =>n.Id == id)
            ?? throw new NotFoundException($"Note {id} not found");

        note.Title = input.Title;
        note.Content = input.Content;
        note.Tags = await ResolveTagsAsync(input.Tags);

        await _db.SaveChangesAsync();
        _logger.LogInformation("Updated note with Id {Id} ",id);

    }

    public async Task DeleteAsync(int id)
    {   
        ValidateId(id);

        Note note = await _db.Notes.FindAsync(id)
            ?? throw new NotFoundException($"Note {id} not found");

        _db.Notes.Remove(note);
        await _db.SaveChangesAsync();
        _logger.LogInformation("Deleted note with Id {Id}",id);
    }

    private static NoteDto ToDto(Note note)
        => new NoteDto(note.Id, note.Title, note.Content, note.CreatedAt, note.Tags.Select(g => g.Name).ToList());

    private static void ValidateId(int id)
    {
        if (id <= 0) 
            
            throw new BadRequestException("Id must more than 0");   
    }

    private async Task<List<Tag>> ResolveTagsAsync(List<string>? inputTags)
    {
        List<Tag> tags = new();
        if (inputTags is null || inputTags.Count == 0)
        return tags;

        
        HashSet<string> normalizedNames = inputTags
            .Where(g => !string.IsNullOrWhiteSpace(g))
            .Select(g => g.Trim().ToLower()).ToHashSet();

        List<Tag> findedTags = await _db.Tags
            .Where(g => normalizedNames.Contains(g.Name)).ToListAsync();

        foreach(var tag in findedTags)
        {
            tags.Add(tag);
            normalizedNames.Remove(tag.Name);
        }

        foreach(var name in normalizedNames)
        {
            tags.Add(new Tag(){Name = name});
        }
        
        return tags;
    }
    
}