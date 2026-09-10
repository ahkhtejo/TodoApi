using Microsoft.AspNetCore.Mvc;

public class HealthController : BaseController
{
    private readonly ILogger<HealthController> _logger;


    public HealthController(ILogger<HealthController> logger)
    {
        _logger = logger;
    }

    [HttpGet("api/hi")]
    public IActionResult Hello()
    {
        _logger.LogInformation("Welcome endpoint was called.");

        var response = new ApiResponse<string>
        {
            Success = true,
            Data = "Welcome to Todo API",
            Message = "Welcome"
        };

        _logger.LogInformation("Welcome endpoint completed successfully.");

        return Ok(response);
    }

    [HttpGet("api/ping")]
    public IActionResult Ping()
    {
        _logger.LogInformation("Ping endpoint was called.");

        var response = new ApiResponse<string>
        {
            Success = true,
            Data = "pong",
            Message = "Ping successful"
        };

        _logger.LogInformation("Ping endpoint completed successfully.");

        return Ok(response);
    }


}
