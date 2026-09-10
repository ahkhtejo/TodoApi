
using FluentValidation.Results;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

public class UserController : BaseController
{
    private readonly UserService _userService;
    private readonly ILogger<UserController> _logger;
    public UserController(UserService userService, ILogger<UserController> logger)
    {
        _userService = userService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllUsers()
    {
        _logger.LogInformation("Getting all users.");

        var users = await _userService.GetAllAsync();
        _logger.LogInformation("Successfully retrieved {UserCount} users.", users.Count);
        return Ok(new ApiResponse<List<GetUsers>>
        {
            Success = true,
            Message = "Users retrieved successfully.",
            Data = ToDtoMapping.MapToGetUsersList(users)
        });

    }

    [HttpGet("{userId:int}")]
    public async Task<IActionResult> GetUserById(int userId)
    {
        _logger.LogInformation(
        "Getting user with ID {UserId}.",
        userId);


        var user = await _userService.GetByIdAsync(userId);

        if (user is null)
        {
            _logger.LogWarning(
                "User with ID {UserId} was not found.",
                userId);

            return NotFound(new ApiResponse<User>
            {
                Success = false,
                Message = "User not found.",
                Data = null
            });
        }

        _logger.LogInformation(
            "User with ID {UserId} retrieved successfully.",
            userId);

        return Ok(new ApiResponse<GetUsers>
        {
            Success = true,
            Message = "User retrieved successfully.",
            Data = ToDtoMapping.MapToGetUser(user)
        });


    }

    [HttpPost]
    public async Task<IActionResult> AddUser([FromBody] AddUserReqest user)
    {
        AddUserReqestValidator ReqestValidator = new AddUserReqestValidator();
        ValidationResult validationResult = ReqestValidator.Validate(user);

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

        var AddedUser = await _userService.CreateAsync(user);
        return Ok(new ApiResponse<User>
        {
            Success = true,
            Message = "User retrieved successfully.",
            Data = AddedUser
        });
    }

    [HttpDelete("{userId}")]
    public async Task<IActionResult> DeleteUserAsync([FromRoute] int userId)
    {
        _logger.LogInformation(
        "Deleting user with ID {UserId}.",
        userId);


        var deleted = await _userService.DeleteAsync(userId);

        if (!deleted)
        {
            _logger.LogWarning(
                "User with ID {UserId} was not found.",
                userId);

            return NotFound(new ApiResponse<object>
            {
                Success = false,
                Message = "User not found.",
                Data = null
            });
        }

        _logger.LogInformation(
            "User with ID {UserId} deleted successfully.",
            userId);

        return Ok(new ApiResponse<object>
        {
            Success = true,
            Message = "User deleted successfully.",
            Data = null
        });


    }

}