namespace NoteApi.Dtos;

public record CreateNoteDto(string Title, string Content);
public record NoteDto(int Id, string Title, string Content, DateTime CreatedAt);

