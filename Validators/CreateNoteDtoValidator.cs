using FluentValidation;
using NotesApi.Dtos;

namespace NotesApi.Validators;

public class CreateNoteDtoValidator : AbstractValidator<CreateNoteDto>
{
    public CreateNoteDtoValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required")
            .MaximumLength(100).WithMessage("Title must be at most 100 characters");

        RuleFor(x => x.Content)
            .NotNull().WithMessage("Content is required")
            .MaximumLength(5000).WithMessage("Content must be at most 5000 characters");
    }   
}