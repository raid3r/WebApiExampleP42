using Microsoft.AspNetCore.Mvc;
using WebApiExampleP42.Models;
using WebApiExampleP42.Models.DTO;
using WebApiExampleP42.Services;

namespace WebApiExampleP42.Controllers;

[ApiController]
[Route("api/[controller]")]
[Controller]
public class MessageController(MessageService messageService, UserService userService) : ControllerBase
{

    // отримати усі повідомлення // 200 401
    [HttpGet]
    public async Task<IActionResult> GetAllMessagesAsync()
    {
        var user = HttpContext.Items["User"] as User; 
        // отримуємо користувача з контексту, який був доданий мідлварою UserAutorizationByToken
        if (user == null)
        {
            return Unauthorized("User is not authorized."); // 401 Unauthorized
        }


        return Ok(await  messageService.GetAllMessagesAsync());
    }

    // додати повідомлення
    [HttpPost]
    public async Task<IActionResult> AddMessageAsync([FromBody] AddMessageRequest request)
    {
        //var tokenFromHeader = Request.Headers["X-My-Authorization"].ToString();
        //var user = await userService.GetUserByTokenAsync(tokenFromHeader);

        //if (user.Email != request.From)
        //{
        //    return Unauthorized("You are not authorized to send messages from this email.");
        //}
        var user = HttpContext.Items["User"] as User; // отримуємо користувача з контексту, який був доданий мідлварою UserAutorizationByToken

        var newMessage = new Message
        {
            To = request.To,
            From = request.From,
            Text = request.Text
        };

        return Ok(await messageService.AddMessageAsync(newMessage));
    }

    // отримати повідомлення для конкретного користувача
    [HttpGet("for")] // GET /api/message/for?email=test@test.com
    public async Task<ICollection<Message>> GetMessagesForEmailAsync([FromQuery] string email)
    {
        return await messageService.GetMessagesForEmailAsync(email);
    }

}

/*
 * Користувача додає до контексту інша мідлвара UserAutorizationByToken, яка перевіряє токен
 * Реалізуте в своєму проєкті мідлвару, яка перевіряє чи співпадає email користувача, який робить запит на відправку повідомлення з полем From у т��лі запиту. Якщо ні - повертає 403 Forbidden.
 * 
 */ 