
public class Comments
{
    public int Id { get; set; }

    public required string Body { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }
    public required int UserId { get; set; }
    public User users { get; set; }
    public required int TodoTaskID { get; set; }
    public TodoTask todoTask { get; set; }

}