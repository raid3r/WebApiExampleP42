using Microsoft.AspNetCore.Mvc;
using WebApiExampleP42.Models;
using WebApiExampleP42.Models.DTO;
using WebApiExampleP42.Services;
using static System.Net.Mime.MediaTypeNames;
using System.ComponentModel;
using AutoMapper;

namespace WebApiExampleP42.Controllers;


[ApiController]
[Route("api/[controller]")] // api/user
[Tags("User authentication")]
public class UserController : ControllerBase
{
    private readonly UserService _userService;
    private readonly IMapper _mapper;

    public UserController(UserService userService, IMapper mapper)
    {
        _userService = userService;
        _mapper = mapper;
    }


    // реєстрація користувача

    [HttpPost("register")]
    [EndpointSummary("Register a new user")]
    [EndpointDescription("This endpoint allows you to register a new user by providing their name, email, and password. Upon successful registration, a token will be returned.")]
    [ProducesResponseType(typeof(UserToken), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<UserToken>> RegisterUserAsync([FromBody] UserRegisterRequest request)
    {
        try
        {
            var newUser = _mapper.Map<User>(request);   
            var createdUser = await _userService.RegisterAsync(newUser);
            return Ok(_mapper.Map<UserToken>(createdUser));
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
        return Ok(_mapper.Map<UserToken>(user));
    }

    // по ід користувача отримати дані користувача


    // оновлення користувача


    // перегляд всіх користувачів
    [HttpGet("all")]
    public async Task<ActionResult<ICollection<UserViewDto>>> GetAllUsersAsync()
    {
        var users = await _userService.GetAllAsync();
        var userDtos = users.Select(u => _mapper.Map<UserViewDto>(u)).ToList();
        return Ok(userDtos);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<UserViewDto>> GetByIdAsync(int id)
    {
        var users = await _userService.GetAllAsync();
        var user = users.FirstOrDefault(u => u.Id == id);
        if (user == null)
        {
            return NotFound();
        }
        var userDto = _mapper.Map<UserViewDto>(user);
        return Ok(userDto);
    }

    [HttpGet]
    public async Task<ActionResult<ICollection<UserViewDto>>> GetAllAsync()
    {
        var users = await _userService.GetAllAsync();
        var userDtos = users.Select(u => _mapper.Map<UserViewDto>(u)).ToList();
        return Ok(userDtos);
    }

}
