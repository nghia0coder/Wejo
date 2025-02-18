using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Wejo.Identity.Application.Requests;

using Common.Core.Controllers;
using Common.SeedWork.Responses;
using Interfaces;

/// <summary>
/// User controller
/// </summary>
public class UsersController : BaseController
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
    public UsersController(IMediator mediator, ISetting setting) : base(mediator) { _setting = setting; }

    /// <summary>
    /// View
    /// </summary>
    /// <returns>Return the result</returns>
    [HttpPatch("View")]
    [ProducesResponseType(typeof(SingleResponse), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> View([FromBody] UserViewR request)
    {
        request.Analyze(HttpContext);

        var response = await _mediator.Send(request);
        response.ReturnUrl = AbsoluteUri;

        return Ok(response);
    }

    #endregion
}

