namespace NotesApi.Dtos;

public record CreateNoteDto(string Title, string Content, List<string>? Tags);
public record NoteDto(int Id, string Title, string Content, DateTime CreatedAt, List<string> Tags);

