using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using API.Models;
using Serilog;
namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]

public class UserController: ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;

    public UserController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }
    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequest request)
    {
        try
        {
            var user = new ApplicationUser
            {
                UserName = request.Email,
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName
            };

            var result = await _userManager.CreateAsync(user, request.Password);

            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, isPersistent: false);
                Log.Information("User registered successfully: {Email}", request.Email);
                return Ok();
            }
            Log.Warning("Registration failed for {Email}: {Errors}", request.Email, string.Join(", ", result.Errors.Select(e => e.Description)));
            return BadRequest();
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error during registration for {Email}", request.Email);
            return StatusCode(500);
        }

    }
    
    [HttpGet("hi")]
    public IActionResult Get()
    {
        var message = "Hello World"; // ← Put breakpoint here
        return Ok(message);
    }
}