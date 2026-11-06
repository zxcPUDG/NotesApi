namespace NotesApi.Exceptions;

public class ValidationException : Exception
{
    public ValidationException(string Message) : base(Message) {}
    
}