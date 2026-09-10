
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;

public class StatusController : BaseController
{
    private readonly StatusSarvice _statusSarvice;
    private readonly ILogger<TaskController> _logger;
    public StatusController(StatusSarvice statusSarvice, ILogger<TaskController> logger)
    {
        _statusSarvice = statusSarvice;
        _logger = logger;
    }



    [HttpGet]
    public async Task<IActionResult> GetAllTasks([FromQuery] GetAllTasksRequest getAllTasksRequest)
    {

        _logger.LogInformation("Getting all tasks.");

        var Status = await _statusSarvice.GetAllAsync();


        var result = Status.OrderBy(t => t.Id)
        .Skip((getAllTasksRequest.pageNumber - 1) * getAllTasksRequest.pageSize)
        .Take(getAllTasksRequest.pageSize).ToList();

        _logger.LogInformation("Successfully retrieved {TaskCount} Status.", Status.Count);

        return Ok(new ApiResponse<List<Status>>
        {
            Success = true,
            Message = "Status retrieved successfully.",
            Data = Status
        });

    }

    [HttpPost]
    public async Task<IActionResult> AddTask([FromBody] CreateStatusDto todo)
    {
        CreateStatusDtoValidator validationRules = new CreateStatusDtoValidator();
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

        var AddedStatus = await _statusSarvice.CreateAsync(todo);

        return Ok(new ApiResponse<Status>
        {
            Success = true,
            Message = "Status retrieved successfully.",
            Data = AddedStatus
        });
    }

    [HttpGet("{StatusID:int}")]
    public async Task<IActionResult> GetTaskById(int StatusID)
    {
        _logger.LogInformation(
        "Getting task with ID {TaskId}.",
        StatusID);


        var status = await _statusSarvice.GetByIdAsync(StatusID);

        if (status is null)
        {
            _logger.LogWarning(
                "status with ID {TaskId} was not found.",
                StatusID);

            return NotFound(new ApiResponse<object>
            {
                Success = false,
                Message = "status not found.",
                Data = null
            });
        }

        _logger.LogInformation(
            "status with ID {TaskId} retrieved successfully.",
            StatusID);

        return Ok(new ApiResponse<Status>
        {
            Success = true,
            Message = "status retrieved successfully.",
            Data = status
        });


    }


    [HttpDelete("{StatusID}")]
    public async Task<IActionResult> DeleteTaskAsync([FromRoute] int StatusID)
    {
        _logger.LogInformation(
        "Deleting status with ID {StatusID}.",
        StatusID);


        var deleted = await _statusSarvice.DeleteAsync(StatusID);

        if (!deleted)
        {
            _logger.LogWarning(
                "status with ID {StatusID} was not found.",
                StatusID);

            return NotFound(new ApiResponse<object>
            {
                Success = false,
                Message = "status not found.",
                Data = null
            });
        }

        _logger.LogInformation(
            "status with ID {StatusID} deleted successfully.",
            StatusID);

        return Ok(new ApiResponse<object>
        {
            Success = true,
            Message = "status deleted successfully.",
            Data = null
        });


    }







}