public class User
{
    public int Id { get; set; }

    public required string Name { get; set; }

    public required string Email { get; set; }

    public DateTime BirthDate { get; set; }

    public UserType UserType { get; set; }

    public List<TodoTask> Tasks { get; set; } = new();
}

public enum UserType
{
    Male,
    Female,
    Other
}
