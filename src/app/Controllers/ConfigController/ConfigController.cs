
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

public class ConfigController : BaseController
{
    public ConfigController()
    {

    }

    [HttpGet("priority")]
    public IActionResult priority()
    {
        ;
        var priority = Enum.GetValues(typeof(Priority))
                           .Cast<Priority>()
                           .Select(e => new { Id = (int)e, Name = e.ToString() }).ToList();
        var response = new ApiResponse<object>
        {
            Success = true,
            Data = priority,
            Message = "priority retrived successful"
        };
        return Ok(response);
    }

    [HttpGet("UsersType")]
    public IActionResult UsersType()
    {
        ;
        var priority = Enum.GetValues(typeof(UserType))
                           .Cast<UserType>()
                           .Select(e => new { Id = (int)e, Name = e.ToString() }).ToList();
        var response = new ApiResponse<object>
        {
            Success = true,
            Data = priority,
            Message = "UserType retrived successful"
        };
        return Ok(response);
    }
}