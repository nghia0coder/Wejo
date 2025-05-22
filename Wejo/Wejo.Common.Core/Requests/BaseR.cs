using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Swashbuckle.AspNetCore.Annotations;
using System.Text.Json;


namespace Wejo.Common.Core.Requests;

using Enums;
using SeedWork.Constants;
using SeedWork.Responses;

/// <summary>
/// Base request
/// </summary>
public class BaseR : IRequest<SingleResponse>
{
    #region -- Methods --

    /// <summary>
    /// Initialize
    /// </summary>
    public BaseR() { }

    /// <summary>
    /// Initialize
    /// </summary>
    /// <param name="hc">HTTP context</param>
    public BaseR(HttpContext? hc)
    {
        _hc = hc;
    }

    /// <summary>
    /// Analyze
    /// </summary>
    /// <param name="hc">HTTP context</param>
    public void Analyze(HttpContext? hc)
    {
        _hc = hc;
    }

    /// <summary>
    /// Get absolute URI
    /// </summary>
    public string GetAbsoluteUri(string domain)
    {
        if (_hc == null)
        {
            return string.Empty;
        }

        var request = _hc.Request;
        if (string.IsNullOrWhiteSpace(domain))
        {
            return request.GetDisplayUrl();
        }

        string? rewriteUrl;

        var key = "X-Original-URL";
        if (request.Headers.ContainsKey(key))
        {
            rewriteUrl = request.Headers[key].ToString();
        }
        else
        {
            var path = request.Path.ToUriComponent();
            var query = request.QueryString.ToUriComponent();
            rewriteUrl = string.Concat(path, query);
        }

        return domain + rewriteUrl;
    }

    /// <summary>
    /// Set the cookie
    /// </summary>
    /// <param name="key">Key (unique indentifier)</param>
    /// <param name="value">Value to store in cookie object</param>
    /// <param name="timeout">Timeout</param>
    /// <param name="type">Time type</param>
    /// <param name="httpOnly">true if a cookie must not be accessible by client-side script; otherwise, false</param>
    public void SetCookie(string key, string value, double timeout, TimeType type, bool httpOnly)
    {
        if (_hc == null)
        {
            return;
        }

        if (timeout < 0)
        {
            timeout = 1;
        }

        TimeSpan? age = null;
        if (type == TimeType.Minute)
        {
            age = TimeSpan.FromMinutes(timeout);
        }
        else if (type == TimeType.Hour)
        {
            age = TimeSpan.FromHours(timeout);
        }
        else if (type == TimeType.Day)
        {
            age = TimeSpan.FromDays(timeout);
        }

        var option = new CookieOptions { HttpOnly = httpOnly, MaxAge = age };
        _hc.Response.Cookies.Append(key, value, option);
    }

    /// <summary>
    /// Delete the cookie
    /// </summary>
    /// <param name="key">Key (unique indentifier)</param>
    public void DelCookie(string key)
    {
        if (_hc == null)
        {
            return;
        }

        _hc.Response.Cookies.Delete(key);
    }

    #endregion

    #region -- Properties --

    /// <summary>
    /// UserName logged in
    /// </summary>
    [SwaggerSchema(ReadOnly = true)]
    public string? UserName => _hc?.User.Identity?.Name;

    /// <summary>
    /// UserId logged in
    /// </summary>
    [SwaggerSchema(ReadOnly = true)]
    public string? UserId => _hc?.User?.Claims.FirstOrDefault(c => c.Type == "user_id")?.Value;

    /// <summary>
    /// UserAgent
    /// </summary>
    [SwaggerSchema(ReadOnly = true)]
    public string? UserAgent
    {
        get
        {
            var headers = _hc?.Request.Headers;

            var key = "User-Agent";
            if (headers != null && headers.ContainsKey(key))
            {
                return headers[key].ToString();
            }

            return null;
        }
    }

    /// <summary>
    /// From mobile
    /// </summary>
    [SwaggerSchema(ReadOnly = true)]
    public bool FromMobile => FromIos || FromAndroid;

    /// <summary>
    /// From iOS
    /// </summary>
    [SwaggerSchema(ReadOnly = true)]
    public bool FromIos => "ios".Equals(DeviceType, StringComparison.OrdinalIgnoreCase);

    /// <summary>
    /// From Android
    /// </summary>
    [SwaggerSchema(ReadOnly = true)]
    public bool FromAndroid => "android".Equals(DeviceType, StringComparison.OrdinalIgnoreCase);

    /// <summary>
    /// Platform
    /// </summary>
    [SwaggerSchema(ReadOnly = true)]
    public string Platform => FromAndroid ? "Android" : FromIos ? "iOS" : "Web";

    /// <summary>
    /// Timezone offset (minute)
    /// </summary>
    [SwaggerSchema(ReadOnly = true)]
    public int TimezoneOffset
    {
        get
        {
            var headers = _hc?.Request.Headers;

            var key = nameof(TimezoneOffset);
            if (headers != null && headers.ContainsKey(key))
            {
                return Convert.ToInt32(headers[key]);
            }

            return 0;
        }
    }

    /// <summary>
    /// Device type
    /// </summary>
    [SwaggerSchema(ReadOnly = true)]
    public string? DeviceType
    {
        get
        {
            var headers = _hc?.Request.Headers;

            var key = nameof(DeviceType);
            if (headers != null && headers.ContainsKey(key))
            {
                return headers[key];
            }

            return null;
        }
    }

    /// <summary>
    /// Origin address
    /// </summary>
    [SwaggerSchema(ReadOnly = true)]
    public string? OriginAddress
    {
        get
        {
            var headers = _hc?.Request.Headers;

            var key = "Origin";
            if (headers != null && headers.ContainsKey(key))
            {
                return headers[key].ToString();
            }

            return null;
        }
    }

    /// <summary>
    /// Action time
    /// </summary>
    [SwaggerSchema(ReadOnly = true)]
    public DateTime ActionTime => DateTime.UtcNow.AddMinutes(-TimezoneOffset);

    /// <summary>
    /// Timezone
    /// </summary>
    [SwaggerSchema(ReadOnly = true)]
    public string Timezone
    {
        get
        {
            var t = TimeSpan.FromMinutes(TimezoneOffset);
            var sign = TimezoneOffset < 0 ? "+" : "-";
            return sign + t.ToString("hh':'mm");
        }
    }

    /// <summary>
    /// Payload
    /// </summary>
    private JsonDocument? Payload
    {
        get
        {
            var user = _hc?.User;
            var payload = user?.Claims.Where(p => p.Type == Setting.Payload).Select(p => p.Value).FirstOrDefault();
            if (payload != null)
            {
                return JsonDocument.Parse(payload);
            }

            return null;
        }
    }

    #endregion

    #region -- Fields --

    /// <summary>
    /// HTTP context
    /// </summary>
    protected HttpContext? _hc;

    #endregion
}
