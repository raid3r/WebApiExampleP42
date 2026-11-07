using Microsoft.AspNetCore.Mvc;
using WebApiExampleP42.Models;
using WebApiExampleP42.Models.DTO;
using WebApiExampleP42.Services;
using static System.Net.Mime.MediaTypeNames;
using System.ComponentModel;

namespace WebApiExampleP42.Controllers;


[ApiController]
[Route("api/[controller]")] // api/user
[Tags("User authentication")]
public class UserController : ControllerBase
{
    private readonly UserService _userService;

    public UserController(UserService userService)
    {
        _userService = userService;
    }


    // реєстрація користувача

    [HttpPost("register")]
    [EndpointSummary("Register a new user")]
    [EndpointDescription("This endpoint allows you to register a new user by providing their name, email, and password. Upon successful registration, a token will be returned.")]
    [ProducesResponseType(typeof(UserToken), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<UserToken>> RegisterUserAsync([FromBody] UserRegisterRequest newUser)
    {
        try
        {
            var createdUser = await _userService.RegisterAsync(newUser);
            return Ok(new UserToken { Token = createdUser.Token });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    // авторизація користувача

    /// <summary>
    /// Login a user and return a token for authentication.
    /// </summary>
    /// <param name="loginUser">The login request containing the user's email and password.</param>
    /// <returns>A UserToken if authentication is successful; otherwise, an Unauthorized response.</returns>
    /// <response code="200">Returns the user token upon successful authentication.</response>
    /// <response code="401">Returns Unauthorized if the email or password is invalid.</response>
    [ProducesResponseType(typeof(UserToken), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
    [HttpPost("login")]
    public async Task<ActionResult<UserToken>> AuthenticateUserAsync([FromBody] UserLoginRequest loginUser)
    {
        var user = await _userService.AuthenticateAsync(loginUser.Email, loginUser.Password);
        if (user == null)
        {
            return Unauthorized("Invalid email or password.");
        }
        return Ok(new UserToken { Token = user.Token });
    }

    // по ід користувача отримати дані користувача


    // оновлення користувача


    // перегляд всіх користувачів
    [HttpGet("all")]
    public async Task<ActionResult<ICollection<UserViewDto>>> GetAllUsersAsync()
    {
        var users = await _userService.GetAllAsync();
        var userDtos = users.Select(u => new UserViewDto
        {
            Id = u.Id,
            Name = u.Name,
            Email = u.Email,
            IsActive = u.IsActive,
            RoleId = u.RoleId
        }).ToList();
        return Ok(userDtos);
    }



}
