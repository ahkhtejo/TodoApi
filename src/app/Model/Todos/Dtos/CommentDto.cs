
public class CreateCommentDto
{
    public string Body { get; set; } = "";
    public int UserId { get; set; }
    public int TodoTaskID { get; set; }
}

public class GetCommentsDto
{
    public int Id { get; set; }

    public string Body { get; set; } = "";

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public required GetUsers users { get; set; }
    public int TaskID { get; set; }
    public string TaskName { get; set; }
}

public class CommentTaskDto
{
    public int Id { get; set; }

    public string Body { get; set; } = "";

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }
}

public static class CommentDtoMapping
{
    public static GetCommentsDto MapCommetToDto(Comments _comments)
    {
        return new GetCommentsDto
        {
            Id = _comments.Id,
            Body = _comments.Body,
            CreatedAt = _comments.CreatedAt,
            UpdatedAt = _comments.UpdatedAt,
            TaskID = _comments.todoTask.Id,
            TaskName = _comments.todoTask.Name,
            users = new GetUsers
            {
                Id = _comments.users.Id,
                Email = _comments.users.Email,
                Name = _comments.users.Name
            },

        };
    }
}
