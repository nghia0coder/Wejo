using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;


namespace Wejo.Game.API.Controllers;

using Application.Request;
using Common.Core.Controllers;
using Common.SeedWork.Responses;

[Route("api/games")]
[ApiController]
public class GameController : BaseController
{
    public GameController(IMediator mediator) : base(mediator) { }
    /// <summary>
    /// Creates a new game.
    /// </summary>
    /// <param name="gameDto">Game details</param>
    /// <returns>Created game ID</returns>
    [HttpPost]
    [ProducesResponseType(typeof(SingleResponse), (int)HttpStatusCode.Created)]
    [ProducesResponseType(typeof(SingleResponse), (int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> CreateGame([FromBody] GameCreateR request)
    {
        if (request == null)
        {
            return BadRequest(new SingleResponse().SetError("InvalidRequest", "Invalid game data"));
        }

        var response = await _mediator.Send(request);

        return CreatedAtAction(nameof(CreateGame), new { id = response.Data }, response);
    }
}
