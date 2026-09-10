using FluentValidation;
using Microsoft.EntityFrameworkCore;

public class CreateStatusDto
{
    [Unicode]
    public string StatusName { get; set; } = "InProgress";

    public string StatusDescription { get; set; } = string.Empty;
}

public class CreateStatusDtoValidator : AbstractValidator<CreateStatusDto>
{
    public CreateStatusDtoValidator()
    {
        RuleFor(x => x.StatusName)
            .NotEmpty()
            .MaximumLength(100)
            .WithMessage("Status name is required.");

        RuleFor(x => x.StatusDescription)
            .MaximumLength(600)
            .WithMessage("Status description cannot exceed 500 characters.");
    }
}

public class GetStatusDto
{
    public int Id { get; set; }

    public string StatusName { get; set; } = string.Empty;

    public string StatusDescription { get; set; } = string.Empty;
}