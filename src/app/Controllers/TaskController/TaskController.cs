
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;

public class TaskController : BaseController
{
    private readonly TaskService _taskService;
    private readonly ILogger<TaskController> _logger;
    public TaskController(TaskService taskService, ILogger<TaskController> logger)
    {
        _taskService = taskService;
        _logger = logger;
    }


    [HttpGet]
    public async Task<IActionResult> GetAllTasks([FromQuery] GetAllTasksRequest getAllTasksRequest)
    {

        _logger.LogInformation("Getting all tasks.");

        var tasks = await _taskService.GetAllAsync();


        var result = tasks.OrderBy(t => t.Id)
        .Skip((getAllTasksRequest.pageNumber - 1) * getAllTasksRequest.pageSize)
        .Take(getAllTasksRequest.pageSize).ToList();

        _logger.LogInformation("Successfully retrieved {TaskCount} tasks.", tasks.Count);

        return Ok(new ApiResponse<List<GetAllTasks>>
        {
            Success = true,
            Message = "tasks retrieved successfully.",
            Data = result
        });

    }

    [HttpPost]
    public async Task<IActionResult> AddTask([FromBody] CreateTodoTaskRequest todo)
    {
        TodoTaskRequestValidator validationRules = new TodoTaskRequestValidator();
        ValidationResult validationResult = validationRules.Validate(todo);
        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors
       .GroupBy(error => error.PropertyName)
       .ToDictionary(
           group => group.Key,
           group => group.Select(error => error.ErrorMessage).ToArray()
       );
            return BadRequest(new ApiResponse<object>
            {
                Success = false,
                Message = "invalid request",
                Errors = errors
            });
        }

        var AddedTask = await _taskService.CreateAsync(todo);

        return Ok(new ApiResponse<TodoTask>
        {
            Success = true,
            Message = "Task retrieved successfully.",
            Data = AddedTask
        });
    }


    [HttpPut]
    public async Task<IActionResult> UpdateAsync([FromBody] CreateTodoTaskRequest todo)
    {
        TodoTaskRequestValidator validationRules = new TodoTaskRequestValidator();
        ValidationResult validationResult = validationRules.Validate(todo);
        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors
       .GroupBy(error => error.PropertyName)
       .ToDictionary(
           group => group.Key,
           group => group.Select(error => error.ErrorMessage).ToArray()
       );
            return BadRequest(new ApiResponse<object>
            {
                Success = false,
                Message = "invalid request",
                Errors = errors
            });
        }


        var updatedTask = await _taskService.UpdateAsync(todo);

        return Ok(new ApiResponse<TodoTask>
        {
            Success = true,
            Message = "Task Updated successfully.",
            Data = updatedTask
        });
    }
    [HttpGet("{TaskId:int}")]
    public async Task<IActionResult> GetTaskById(int TaskId)
    {
        _logger.LogInformation(
        "Getting task with ID {TaskId}.",
        TaskId);


        var task = await _taskService.GetByIdAsync(TaskId);

        if (task is null)
        {
            _logger.LogWarning(
                "Task with ID {TaskId} was not found.",
                TaskId);

            return NotFound(new ApiResponse<object>
            {
                Success = false,
                Message = "Task not found.",
                Data = null
            });
        }

        _logger.LogInformation(
            "Task with ID {TaskId} retrieved successfully.",
            TaskId);

        return Ok(new ApiResponse<TodoTask>
        {
            Success = true,
            Message = "Task retrieved successfully.",
            Data = task
        });


    }


    [HttpDelete("{TaskId}")]
    public async Task<IActionResult> DeleteTaskAsync([FromRoute] int TaskId)
    {
        _logger.LogInformation(
        "Deleting Task with ID {TaskId}.",
        TaskId);


        var deleted = await _taskService.DeleteAsync(TaskId);

        if (!deleted)
        {
            _logger.LogWarning(
                "Task with ID {TaskId} was not found.",
                TaskId);

            return NotFound(new ApiResponse<object>
            {
                Success = false,
                Message = "Task not found.",
                Data = null
            });
        }

        _logger.LogInformation(
            "Task with ID {TaskId} deleted successfully.",
            TaskId);

        return Ok(new ApiResponse<object>
        {
            Success = true,
            Message = "Task deleted successfully.",
            Data = null
        });


    }

    [HttpGet("api/{UserID:int}")]
    public async Task<IActionResult> GetUserTaks([FromRoute] int UserID)
    {

        var tasks = await _taskService.GetUserTaks(UserID);
        if (tasks is null)
        {
            return BadRequest(new ApiResponse<object>
            {
                Success = false,
                Message = $"unebale to get the tasks for user id {UserID}",
                Data = null
            });
        }
        return Ok(new ApiResponse<List<TodoTask>>
        {
            Success = true,
            Message = "Task deleted successfully.",
            Data = tasks
        });
    }

}