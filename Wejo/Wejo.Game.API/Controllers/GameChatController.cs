using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Wejo.Game.API.Controllers;

using Application.Request;
using Common.Core.Controllers;
using Common.SeedWork.Responses;

public class GameChatController : BaseController
{
    public GameChatController(IMediator mediator) : base(mediator) { }

    /// <summary>
    /// Send message in game room.
    /// </summary>
    /// <param name="request">Send Message to game room</param>
    /// <returns>Send message success</returns>
    [HttpPost("send-message")]
    [ProducesResponseType(typeof(SingleResponse), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> SendMessage([FromBody] GameChatSendMessageR request)
    {
        request.Analyze(HttpContext);

        var response = await _mediator.Send(request);
        response.ReturnUrl = AbsoluteUri;

        return Ok(response);
    }

    /// <summary>
    /// Get message in game room.
    /// </summary>
    /// <param name="request">Get Message to game room</param>
    /// <returns>Message in the room</returns>
    [HttpGet("get-messages")]
    [ProducesResponseType(typeof(SingleResponse), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetMessage([FromQuery] GameChatGetMessageR request)
    {
        request.Analyze(HttpContext);

        var response = await _mediator.Send(request);
        response.ReturnUrl = AbsoluteUri;

        return Ok(response);
    }

    /// <summary>
    /// Get message in game room.
    /// </summary>
    /// <param name="request">Get Message to game room</param>
    /// <returns>Message in the room</returns>
    [HttpPatch("mark-as-read")]
    [ProducesResponseType(typeof(SingleResponse), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> MarkAsRead([FromBody] GameChatMarkAsReadR request)
    {
        request.Analyze(HttpContext);

        var response = await _mediator.Send(request);
        response.ReturnUrl = AbsoluteUri;

        return Ok(response);
    }
}
