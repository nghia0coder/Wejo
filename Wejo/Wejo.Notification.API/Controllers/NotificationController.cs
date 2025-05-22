using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Wejo.Notification.API.Controllers;

using Application.Requests;
using Common.Core.Controllers;
using Common.SeedWork.Responses;

public class NotificationController : BaseController
{
    public NotificationController(IMediator mediator) : base(mediator) { }

    /// <summary>
    /// View all notificaiton.
    /// </summary>
    /// <param name="request">Notification details</param>
    /// <returns>Unseend count</returns>
    [HttpGet("View")]
    [ProducesResponseType(typeof(SingleResponse), (int)HttpStatusCode.Created)]
    public async Task<IActionResult> View([FromBody] NotificationViewR request)
    {
        request.Analyze(HttpContext);

        var response = await _mediator.Send(request);
        response.ReturnUrl = AbsoluteUri;

        return Ok(response);
    }

    /// <summary>
    /// Mark all as seen in notificaiton.
    /// </summary>
    /// <param name="request">Request</param>
    /// <returns>Ok</returns>
    [HttpPatch("mark-all-as-seen")]
    [ProducesResponseType(typeof(SingleResponse), (int)HttpStatusCode.Created)]
    public async Task<IActionResult> MarkAllAsSeen([FromBody] NotiMarkAllAsSeenR request)
    {
        request.Analyze(HttpContext);

        var response = await _mediator.Send(request);
        response.ReturnUrl = AbsoluteUri;

        return Ok(response);
    }
}
