using NotesApi.Dtos;
namespace NotesApi.Services;
public interface INoteService
{
    Task<List<NoteDto>> GetAllAsync(string? search,string[]? tags);
    Task<NoteDto> GetByIdAsync(int id);
    Task<NoteDto> CreateAsync(CreateNoteDto input);
    Task UpdateAsync(int id, CreateNoteDto input);
    Task DeleteAsync(int id);                      
}