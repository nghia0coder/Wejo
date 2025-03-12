using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Wejo.Game.API.Controllers;

using Common.Core.Controllers;
using Common.SeedWork.Responses;
using Game.Application.Interfaces;
using Game.Application.Request;

/// <summary>
/// User controller
/// </summary>
public class GameParticipantController : BaseController
{
    #region -- Fields --

    /// <summary>
    /// Setting
    /// </summary>
    private readonly ISetting _setting;

    #endregion

    #region -- Methods --

    /// <summary>
    /// Initialize
    /// </summary>
    /// <param name="mediator">Mediator</param>
    public GameParticipantController(IMediator mediator, ISetting setting) : base(mediator) { _setting = setting; }

    /// <summary>
    /// Create
    /// </summary>
    /// <returns>Return the result</returns>
    [HttpPost("Create"), Authorize]
    [ProducesResponseType(typeof(SingleResponse), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> Create([FromBody] GameParticipantCreateR request)
    {
        request.Analyze(HttpContext);

        var response = await _mediator.Send(request);
        response.ReturnUrl = AbsoluteUri;

        return Ok(response);
    }

    /// <summary>
    /// Update
    /// </summary>
    /// <returns>Return the result</returns>
    [HttpPatch("Update"), Authorize]
    [ProducesResponseType(typeof(SingleResponse), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> Update([FromBody] GameParticipantUpdateR request)
    {
        request.Analyze(HttpContext);

        var response = await _mediator.Send(request);
        response.ReturnUrl = AbsoluteUri;

        return Ok(response);
    }

    /// <summary>
    /// View
    /// </summary>
    /// <returns>Return the result</returns>
    [HttpPatch("View")]
    [ProducesResponseType(typeof(SingleResponse), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> View([FromBody] GameParticipantViewR request)
    {
        request.Analyze(HttpContext);

        var response = await _mediator.Send(request);
        response.ReturnUrl = AbsoluteUri;

        return Ok(response);
    }

    #endregion
}

