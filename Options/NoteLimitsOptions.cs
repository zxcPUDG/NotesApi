using System.ComponentModel.DataAnnotations;

namespace NotesApi.Settings;

public class NoteLimitsOptions
{
    [Range(1, 1000)]
    public int TitleMaxLength {get;set;}
    [Range(1, 50000)]
    public int ContentMaxLength {get;set;}
}