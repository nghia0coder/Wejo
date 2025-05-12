using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Wejo.Identity.API.Controllers;

using Application.Request;
using Application.Requests;
using Common.Core.Controllers;
using Common.SeedWork.Responses;

public class UserChatController : BaseController
{
    public UserChatController(IMediator mediator) : base(mediator) { }

    /// <summary>
    /// Send message in User room.
    /// </summary>
    /// <param name="request">Send Message to User room</param>
    /// <returns>Send message success</returns>
    [HttpPost("send-message")]
    [ProducesResponseType(typeof(SingleResponse), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> SendMessage([FromBody] UserChatSendMessageR request)
    {
        request.Analyze(HttpContext);

        var response = await _mediator.Send(request);
        response.ReturnUrl = AbsoluteUri;

        return Ok(response);
    }

    /// <summary>
    /// Get message in User room.
    /// </summary>
    /// <param name="request">Get Message to User room</param>
    /// <returns>Message in the room</returns>
    [HttpGet("get-messages")]
    [ProducesResponseType(typeof(SingleResponse), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetMessage([FromQuery] UserChatGetMessageR request)
    {
        request.Analyze(HttpContext);

        var response = await _mediator.Send(request);
        response.ReturnUrl = AbsoluteUri;

        return Ok(response);
    }

    /// <summary>
    /// Get message in User room.
    /// </summary>
    /// <param name="request">Get Message to User room</param>
    /// <returns>Message in the room</returns>
    [HttpPatch("mark-as-read")]
    [ProducesResponseType(typeof(SingleResponse), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> MarkAsRead([FromBody] UserChatMarkAsReadR request)
    {
        request.Analyze(HttpContext);

        var response = await _mediator.Send(request);
        response.ReturnUrl = AbsoluteUri;

        return Ok(response);
    }
}
