using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Wejo.Game.API.Controllers;

using Application.Request;
using Common.Core.Controllers;
using Common.SeedWork.Responses;
using Wejo.Game.Application.Request.Games;

public class GameController : BaseController
{
    public GameController(IMediator mediator) : base(mediator) { }

    /// <summary>
    /// Creates a new game.
    /// </summary>
    /// <param name="request">Game details</param>
    /// <returns>Created game ID</returns>
    [HttpPost("Create")]
    [ProducesResponseType(typeof(SingleResponse), (int)HttpStatusCode.Created)]
    [ProducesResponseType(typeof(SingleResponse), (int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> Create([FromBody] GameCreateR request)
    {
        request.Analyze(HttpContext);

        var response = await _mediator.Send(request);
        response.ReturnUrl = AbsoluteUri;

        return Ok(response);
    }

    [HttpPatch("Search")]
    [ProducesResponseType(typeof(SingleResponse), (int)HttpStatusCode.Created)]
    [ProducesResponseType(typeof(SingleResponse), (int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> Search([FromBody] GameListInfoR request)
    {
        request.Analyze(HttpContext);

        var response = await _mediator.Send(request);
        response.ReturnUrl = AbsoluteUri;

        return Ok(response);
    }
}
