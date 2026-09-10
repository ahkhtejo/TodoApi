
using System.ComponentModel.DataAnnotations;
using FluentValidation;


public class CreateTodoTaskRequest
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    [EnumDataType(typeof(Priority))]
    public Priority Priority { get; set; }
    public int UserId { get; set; }
    public int StatusId { get; set; }

}

public class TodoTaskRequestValidator : AbstractValidator<CreateTodoTaskRequest>
{
    public TodoTaskRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(150)
            .WithMessage("Name is required and must not exceed 150 characters.");

        RuleFor(x => x.Description)
            .NotEmpty()
            .WithMessage("Description is required.");

        RuleFor(x => x.StartDate)
            .NotEmpty()
            .WithMessage("Start Date is required.");

        RuleFor(x => x.EndDate)
            .GreaterThan(x => x.StartDate)
            .When(x => x.EndDate.HasValue)
            .WithMessage("End Date must be greater than Start Date.");

        RuleFor(x => x.Priority)
            .IsInEnum()
            .WithMessage("Invalid Priority value.");

        RuleFor(x => x.UserId)
            .GreaterThan(0)
            .WithMessage("UserId must be greater than 0.");
    }
}

public class GetAllTasksRequest
{
    public int pageNumber { get; set; } = 1;
    public int pageSize { get; set; } = 5;

}

public class GetAllTasksRequestValidator : AbstractValidator<GetAllTasksRequest>
{
    public GetAllTasksRequestValidator()
    {
        RuleFor(x => x.pageNumber)
            .GreaterThan(0)
            .WithMessage("Page number must be greater than 0.");

        RuleFor(x => x.pageSize)
            .InclusiveBetween(1, 100)
            .WithMessage("Page size must be between 1 and 100.");
    }
}


public class GetAllTasks
{
    public int Id { get; set; }

    public required string Name { get; set; }

    public string? Description { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }
    public GetUsers Users { get; set; }
    public string Priority { get; set; } = null!;
    public string Status { get; set; } = null!;
    public List<CommentTaskDto> Comments { get; set; } = new();
}

