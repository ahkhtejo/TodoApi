using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;

public class TaskService : ITask
{
    // private readonly List<TodoTask> _tasksList = new();
    private readonly UserService _userService;
    private readonly AppDbContext _dbContext;

    public TaskService(UserService userService, AppDbContext appContext)
    {
        _userService = userService;
        _dbContext = appContext;

    }

    public async Task<TodoTask?> CreateAsync(CreateTodoTaskRequest todo)
    {



        var user = await _userService.GetByIdAsync(todo.UserId);

        if (user is null)
        {
            return null;
        }

        TodoTask task = new TodoTask
        {
            Name = todo.Name,
            Description = todo.Description,
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now,
            StartDate = todo.StartDate,
            EndDate = todo.EndDate,
            Priority = todo.Priority,
            UserId = user.Id,
            users = user,
            StatusId = todo.StatusId
        };

        _dbContext.Tasks.Add(task);
        await _dbContext.SaveChangesAsync();
        return task;
    }

    public async Task<bool> DeleteAsync(int TaskId)
    {
        var task = _dbContext.Tasks.SingleOrDefault(t => t.Id == TaskId);
        if (task is null)
        {
            return false;

        }
        _dbContext.Tasks.Remove(task);
        await _dbContext.SaveChangesAsync();
        return true;

    }

    public async Task<List<GetAllTasks>> GetAllAsync()
    {
        return _dbContext.Tasks.Select(t => new GetAllTasks
        {
            Id = t.Id,
            Name = t.Name,
            Description = t.Description,
            StartDate = t.StartDate,
            EndDate = t.EndDate,
            Priority = t.Priority.GetDisplayName(),
            Users = new GetUsers { Id = t.UserId, Email = t.users.Email, Name = t.users.Name },
            Status = t.Status.StatusName,
            Comments = t.Comments
                .Select(c => new CommentTaskDto
                {
                    Id = c.Id,
                    Body = c.Body,
                    CreatedAt = c.CreatedAt,
                    UpdatedAt = c.UpdatedAt
                })
                .ToList()


        }).ToList();
    }

    public async Task<List<TodoTask?>> GetUserTaks(int UserID)
    {
        var user = await _userService.GetByIdAsync(UserID);
        if (user is not null)
        {
            var Tasks = await _dbContext.Tasks.Where(t => t.UserId == UserID).ToListAsync();
            return Tasks;
        }
        return null;
    }

    public async Task<TodoTask?> GetByIdAsync(int TaskId)
    {
        var task = _dbContext.Tasks.SingleOrDefault(t => t.Id == TaskId);
        if (task is null)
        {
            return null;
        }
        return task;
    }



    public async Task<TodoTask?> UpdateAsync(CreateTodoTaskRequest todo)
    {


        var user = await _userService.GetByIdAsync(todo.UserId);
        var DbTask = await GetByIdAsync(todo.Id);

        if (user is null)
        {
            return null;
        }



        DbTask.Name = todo.Name;
        DbTask.Description = todo.Description;
        DbTask.UpdatedAt = DateTime.Now;
        DbTask.StartDate = todo.StartDate;
        DbTask.EndDate = todo.EndDate;
        DbTask.Priority = todo.Priority;
        DbTask.UserId = user.Id;
        DbTask.users = user;
        DbTask.StatusId = todo.StatusId;

        _dbContext.Entry(DbTask).State = EntityState.Modified;
        await _dbContext.SaveChangesAsync();
        return DbTask;

    }
}
