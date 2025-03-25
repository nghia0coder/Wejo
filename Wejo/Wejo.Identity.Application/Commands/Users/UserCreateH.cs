using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Wejo.Identity.Application.Commands;

using Azure.Storage.Blobs;
using Common.Core.Extensions;
using Common.Domain.Entities;
using Common.Domain.Interfaces;
using Common.SeedWork.Dtos;
using Common.SeedWork.Extensions;
using Common.SeedWork.Responses;
using Interfaces;
using Microsoft.AspNetCore.Http;
using Requests;
using Validators;
using static Common.SeedWork.Constants.Error;

/// <summary>
/// Handler
/// </summary>
public class UserCreateH : BaseSettingH, IRequestHandler<UserCreateR, SingleResponse>
{
    #region --Fieds--

    private readonly IFirebaseAuthService _firebaseAuth;

    private readonly BlobServiceClient _blobServiceClient;

    #endregion

    #region -- Methods --

    /// <summary>
    /// Initialize
    /// </summary>
    /// <param name="context">DB context</param>
    /// 
    public UserCreateH(IWejoContext context, ISetting setting, IFirebaseAuthService firebaseAuth, BlobServiceClient blobServiceClient) : base(context, setting)
    {
        _firebaseAuth = firebaseAuth;
        _blobServiceClient = blobServiceClient;
    }

    /// <summary>
    /// Handle
    /// </summary>
    /// <param name="request">Request</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Return the result</returns>
    public async Task<SingleResponse> Handle(UserCreateR request, CancellationToken cancellationToken)
    {
        var res = new SingleResponse();

        var vr = new UserCreateV().Validate(request);
        if (!vr.IsValid)
        {
            var t = vr.Errors.ToDic();
            return res.SetError(nameof(E000), E000, t);
        }

        var userId = request.Id;
        if (userId == null)
        {
            return res.SetError(nameof(E119), E119);
        }

        #region -- Validate on server --
        var hasUser = await _context.Users.AnyAsync(p => p.Id == userId, cancellationToken);
        if (hasUser)
        {
            var t = new List<DicDto> { new() { Key = nameof(userId).ToCamelCase(), Value = userId } };
            return res.SetError(nameof(E002), E002, t);
        }
        #endregion

        var ett = User.Create(userId, request.FirstName, request.LastName, request.PhoneNumber, request.PhoneNumberConfirmed, request.Email, request.EmailConfirmed);
        if (request.Image != null)
        {
            var oldFileName = ett.Avatar;
            request.ImageUrl = await UploadUserImageAsync(userId, request.Image, oldFileName);
            ett.Avatar = request.ImageUrl;
        }

        await _context.Users.AddAsync(ett, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return res.SetSuccess(ett.ToViewDto());
    }

    /// <summary>
    /// Uploads a user's image to Azure Blob Storage, deletes the old image if it exists, and returns the new image URL.
    /// </summary>
    /// <param name="userId">The ID of the user.</param>
    /// <param name="image">The new image file to upload.</param>
    /// <param name="oldFileName">The filename of the user's previous image (optional).</param>
    /// <returns>URL of the uploaded image.</returns>
    private async Task<string> UploadUserImageAsync(string userId, IFormFile image, string? oldFileName = null)
    {
        const string containerName = "userimg";
        var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);

        // Delete the previous image from Blob Storage if it exists
        if (!string.IsNullOrEmpty(oldFileName))
        {
            var oldFileUrl = Path.GetFileName(new Uri(oldFileName).AbsolutePath);
            var oldBlobClient = containerClient.GetBlobClient(oldFileUrl);
            await oldBlobClient.DeleteIfExistsAsync();
        }

        // If no new image is provided, return a default placeholder image URL
        if (image is null)
            return "https://placehold.co/600x400";

        // Generate a new unique filename for the uploaded image
        var newFileName = $"{userId}-{Guid.NewGuid()}{Path.GetExtension(image.FileName)}";
        var newBlobClient = containerClient.GetBlobClient(newFileName);

        using var stream = image.OpenReadStream();
        await newBlobClient.UploadAsync(stream, overwrite: true);

        return newBlobClient.Uri.ToString();
    }

    #endregion
}
