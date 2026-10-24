public class Note
{
    public int Id {get; set;}
    public string Title {get; set;} = "";
    public string Content {get; set;} = "";
    public DateTime CreatedAt {get; set;}
} 

public record CreateNoteDto(string Title, string Content);
public record NoteDto(int Id, string Title, string Content, DateTime CreatedAt);

