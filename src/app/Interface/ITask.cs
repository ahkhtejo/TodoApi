
public interface ITask
{
    Task<TodoTask?> GetByIdAsync(int TaskId);

    Task<List<GetAllTasks>> GetAllAsync();

    Task<TodoTask> CreateAsync(CreateTodoTaskRequest todo);

    Task<TodoTask?> UpdateAsync(CreateTodoTaskRequest todo);

    Task<bool> DeleteAsync(int TaskId);
}