using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Text;

namespace Wejo.Identity.Application.Services;

using Common.Extensions;
using Common.SeedWork.Dtos;
using Interfaces;

public class FirebaseAuthService : IFirebaseAuthService
{
    private readonly string _apiKey;
    private readonly HttpClient _httpClient;

    public FirebaseAuthService(IConfiguration configuration)
    {
        _apiKey = configuration["Firebase:WebApiKey"];
        _httpClient = new HttpClient();
    }

    public async Task<FirebaseSessionDto> SendOtpAsync(string phoneNumber)
    {
        var requestBody = new
        {
            phoneNumber,
            recaptchaToken = "RECAPTCHA_TESTING_TOKEN"
        };

        var jsonContent = JsonConvert.SerializeObject(requestBody);
        var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync($"https://identitytoolkit.googleapis.com/v1/accounts:sendVerificationCode?key={_apiKey}", content);
        var responseString = await response.Content.ReadAsStringAsync();

        var result = responseString.FromJson<FirebaseSessionDto>();

        return result;
    }

    public async Task<JwtDto> VerifyOtpAsync(string sessionInfo, string otpCode)
    {
        var requestBody = new
        {
            sessionInfo,
            code = otpCode
        };

        var jsonContent = JsonConvert.SerializeObject(requestBody);
        var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync($"https://identitytoolkit.googleapis.com/v1/accounts:signInWithPhoneNumber?key={_apiKey}", content);
        var responseString = await response.Content.ReadAsStringAsync();

        var result = responseString.FromJson<JwtDto>();

        return result;
    }

    public async Task<JwtDto> SignInWithGoogleAsync(string idToken)
    {
        var requestBody = new
        {
            postBody = $"id_token={idToken}&providerId=google.com",
            requestUri = "http://localhost",
            returnSecureToken = true
        };

        var jsonContent = JsonConvert.SerializeObject(requestBody);
        var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync($"https://identitytoolkit.googleapis.com/v1/accounts:signInWithIdp?key={_apiKey}", content);
        var responseString = await response.Content.ReadAsStringAsync();

        var result = responseString.FromJson<JwtDto>();

        return result;
    }

}

