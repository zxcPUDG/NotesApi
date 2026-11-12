namespace NotesApi.Exceptions;

public class BadRequestException : Exception
{
    public BadRequestException(string Message) : base(Message) {}
    
}