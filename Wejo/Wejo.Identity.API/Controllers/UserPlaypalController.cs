using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Wejo.Identity.API.Controllers;

using Application.Interfaces;
using Application.Requests;
using Common.Core.Controllers;
using Common.SeedWork.Responses;

/// <summary>
/// User controller
/// </summary>
public class UserPlaypalController : BaseController
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
    public UserPlaypalController(IMediator mediator, ISetting setting) : base(mediator) { _setting = setting; }

    /// <summary>
    /// View
    /// </summary>
    /// <returns>Return the result</returns>
    [HttpGet("View")]
    [ProducesResponseType(typeof(SingleResponse), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> View([FromBody] UserPlaypalViewR request)
    {
        request.Analyze(HttpContext);

        var response = await _mediator.Send(request);
        response.ReturnUrl = AbsoluteUri;

        return Ok(response);
    }

    #endregion
}

