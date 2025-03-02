using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Wejo.Common.Core.Controllers;
using Wejo.Common.SeedWork.Responses;
using Wejo.GameService.Application.Request.Games;

namespace Wejo.GameService.API.Controllers
{
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


}
