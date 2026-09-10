
using Microsoft.AspNetCore.Mvc;

public class CommentController : BaseController
{
    private readonly ILogger<TaskController> _logger;
    private readonly CommentsService _commentsService;
    public CommentController(ILogger<TaskController> logger, CommentsService commentsService)
    {
        _logger = logger;
        _commentsService = commentsService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllTasks([FromQuery] GetAllTasksRequest getAllTasksRequest)
    {

        _logger.LogInformation("Getting all tasks.");

        var Comments = await _commentsService.GetAllAsync();


        var result = Comments.OrderBy(t => t.Id)
        .Skip((getAllTasksRequest.pageNumber - 1) * getAllTasksRequest.pageSize)
        .Take(getAllTasksRequest.pageSize).ToList();

        _logger.LogInformation("Successfully retrieved {CommentsCount} Comments.", Comments.Count);

        return Ok(new ApiResponse<List<GetCommentsDto>>
        {
            Success = true,
            Message = "Comments retrieved successfully.",
            Data = Comments
        });

    }

    [HttpPost]
    public async Task<IActionResult> AddTask([FromBody] CreateCommentDto CommentDto)
    {


        var AddedStatus = await _commentsService.CreateAsync(CommentDto);

        return Ok(new ApiResponse<Comments>
        {
            Success = true,
            Message = "Status retrieved successfully.",
            Data = AddedStatus
        });
    }

    [HttpGet("{CommentID:int}")]
    public async Task<IActionResult> GetTaskById(int CommentID)
    {
        _logger.LogInformation(
        "Getting task with ID {CommentID}.",
        CommentID);


        var Comment = await _commentsService.GetByIdAsync(CommentID);

        if (Comment is null)
        {
            _logger.LogWarning(
                "Comment with ID {CommentID} was not found.",
                CommentID);

            return NotFound(new ApiResponse<object>
            {
                Success = false,
                Message = "Comment not found.",
                Data = null
            });
        }

        _logger.LogInformation(
            "Comment with ID {CommentID} retrieved successfully.",
            CommentID);

        return Ok(new ApiResponse<GetCommentsDto>
        {
            Success = true,
            Message = "Comment retrieved successfully.",
            Data = Comment
        });


    }


    [HttpDelete("{CommentID}")]
    public async Task<IActionResult> DeleteTaskAsync([FromRoute] int CommentID)
    {
        _logger.LogInformation(
        "Deleting Comment with ID {CommentID}.",
        CommentID);


        var deleted = await _commentsService.DeleteAsync(CommentID);

        if (!deleted)
        {
            _logger.LogWarning(
                "Comment with ID {CommentID} was not found.",
                CommentID);

            return NotFound(new ApiResponse<object>
            {
                Success = false,
                Message = "Comment not found.",
                Data = null
            });
        }

        _logger.LogInformation(
            "Comment with ID {CommentID} deleted successfully.",
            CommentID);

        return Ok(new ApiResponse<object>
        {
            Success = true,
            Message = "Comment deleted successfully.",
            Data = null
        });


    }



}