public class TodoTask
{
    public int Id { get; set; }

    public required string Name { get; set; }

    public string? Description { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public Priority Priority { get; set; }
    public int UserId { get; set; }

    public User users { get; set; }

    public int StatusId { get; set; }

    public Status Status { get; set; } = null!;
    public List<Comments> Comments { get; set; } = new();

}

public enum Priority
{
    Low = 1,
    Medium = 2,
    Normal = 3,
    High = 4
}

