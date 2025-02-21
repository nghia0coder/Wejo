using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Wejo.Identity.Application.Controllers;

using Common.Core.Controllers;
using Common.SeedWork.Responses;
using Interfaces;
using Requests;

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
    [HttpPatch("View"), Authorize]
    [ProducesResponseType(typeof(SingleResponse), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> View([FromBody] UserViewR request)
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
    public async Task<IActionResult> Update([FromForm] UserUpdateR request)
    {
        request.Analyze(HttpContext);

        var response = await _mediator.Send(request);
        response.ReturnUrl = AbsoluteUri;

        return Ok(response);
    }

    /// <summary>
    /// SendOtp
    /// </summary>
    /// <returns>Return the result</returns>
    [HttpPatch("SendOtp")]
    [ProducesResponseType(typeof(SingleResponse), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> SendOtp([FromBody] UserSendOtpR request)
    {
        request.Analyze(HttpContext);

        var response = await _mediator.Send(request);
        response.ReturnUrl = AbsoluteUri;

        return Ok(response);
    }

    /// <summary>
    /// VerifyOtp
    /// </summary>
    /// <returns>Return the result</returns>
    [HttpPatch("VerifyOtp")]
    [ProducesResponseType(typeof(SingleResponse), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> VerifyOtp([FromBody] UserVerifyOtpR request)
    {
        request.Analyze(HttpContext);

        var response = await _mediator.Send(request);
        response.ReturnUrl = AbsoluteUri;

        return Ok(response);
    }

    /// <summary>
    /// LoginSocial
    /// </summary>
    /// <returns>Return the result</returns>
    [HttpPatch("login-social")]
    [ProducesResponseType(typeof(SingleResponse), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> LoginSocial([FromBody] UserLoginSocialR request)
    {
        request.Analyze(HttpContext);

        var response = await _mediator.Send(request);
        response.ReturnUrl = AbsoluteUri;

        return Ok(response);
    }

    #endregion
}

