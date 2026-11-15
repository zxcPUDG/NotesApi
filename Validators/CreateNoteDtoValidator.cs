using FluentValidation;
using Microsoft.Extensions.Options;
using NotesApi.Dtos;
using NotesApi.Settings;

namespace NotesApi.Validators;

public class CreateNoteDtoValidator : AbstractValidator<CreateNoteDto>
{
    public CreateNoteDtoValidator(IOptions<NoteLimitsOptions> options)
    {   
        var limits = options.Value;

        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required")
            .MaximumLength(limits.TitleMaxLength)
            .WithMessage($"Title must be at most {limits.TitleMaxLength} characters");

        RuleFor(x => x.Content)
            .NotNull().WithMessage("Content is required")
            .MaximumLength(limits.ContentMaxLength)
            .WithMessage($"Content must be at most {limits.ContentMaxLength} characters");
    }   
}