using FluentValidation;


public class AddUserReqest
{
    public required string Name { get; set; }
    public DateTime BirthDate { get; set; }
    public required string Email { get; set; }
    public UserType UserType { get; set; }
}

public class AddUserReqestValidator : AbstractValidator<AddUserReqest>
{
    public AddUserReqestValidator()
    {
        RuleFor(AddUserReqest => AddUserReqest.Name)
        .NotEmpty()
        .NotNull()
        .MaximumLength(150)
        .WithMessage("Name is required.");

        RuleFor(AddUserReqest => AddUserReqest.Email)
      .NotEmpty()
      .NotNull()
      .EmailAddress()
      .WithMessage("Email is required.");

        RuleFor(AddUserReqest => AddUserReqest.BirthDate)
        .NotEmpty().WithMessage("Date of birth is required.")
        .LessThanOrEqualTo(DateTime.Today)
        .WithMessage("Date cannot be in the future.");
    }
}

public class GetUsers
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Email { get; set; }
}
public static class ToDtoMapping
{
    public static List<GetUsers> MapToGetUsersList(IEnumerable<User> users)
    {
        return users
        .Select(u => new GetUsers
        {
            Id = u.Id,
            Email = u.Email,
            Name = u.Name
        })
        .ToList();
    }

    public static GetUsers MapToGetUser(User user)
    {
        return new GetUsers
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email
        };
    }
}

