using NotesApi.Dtos;
namespace NotesApi.Service;
public interface INoteService
{
    Task<List<NoteDto>> GetAllAsync(string? search);
    Task<NoteDto?> GetByIdAsync(int id);
    Task<NoteDto> CreateAsync(CreateNoteDto input);
    Task<bool> UpdateAsync(int id, CreateNoteDto input);
    Task<bool> DeleteAsync(int id);                      
}