using Microsoft.EntityFrameworkCore;

[Index(nameof(StatusName), IsUnique = true)]
public class Status
{
    public int Id { get; set; }
    [Unicode]
    public string StatusName { get; set; } = "InProgress";

    public string StatusDescription { get; set; } = string.Empty;
}