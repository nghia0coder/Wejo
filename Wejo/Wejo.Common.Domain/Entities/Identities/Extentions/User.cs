namespace Wejo.Common.Domain.Entities;

using SeedWork.Dtos;

partial class User
{
    #region -- Methods --

    /// <summary>
    /// Initialize
    /// </summary>
    public User()
    {
        Id = Guid.NewGuid();
        CreatedOn = DateTime.UtcNow;
    }


    /// <summary>
    /// Convert to data transfer object
    /// </summary>
    /// <param name="numberOfFollowings"></param>
    /// <param name="numberOfFollowers"></param>
    /// <returns>Return the DTO</returns>
    public ViewDto ToViewDto()
    {
        var res = ToBaseDto<ViewDto>();

        return res;
    }

    /// <summary>
    /// Convert to data transfer object
    /// </summary>
    /// <returns>Return the DTO</returns>
    public T ToBaseDto<T>() where T : BaseDto, new()
    {
        return new T
        {
            Id = Id,
            LastName = LastName,
            FirstName = FirstName,
            Avatar = Avatar,
            Email = Email,
            CreatedOn = CreatedOn,
        };
    }


    ///// <summary>
    ///// Create JWT
    ///// </summary>
    ///// <param name="jwt">JWT setting</param>
    ///// <returns>Returns the result</returns>
    //public TokenDto CreateJwt(JwtDto jwt)
    //{
    //    var roles = string.Join(",", Roles);

    //    var payload = new PayloadDto
    //    {
    //        Id = Id,
    //        UserName = UserName + "",
    //        ProfileName = ProfileName + "",
    //        ProfileId = ProfileId + "",
    //        UserFolder = UserFolder,
    //        UserAvatar = Avatar + "",
    //        Roles = Roles,
    //        IsPremium = IsPremium || roles.IsIsAdministrator(),
    //        IsWalletShowing = IsWalletShowing,
    //        SessionId = SessionId,
    //        MinioInstance = MinioInstance,
    //        StorageLimit = StorageLimit
    //    };

    //    var st = new SecurityToken(jwt, payload);
    //    var delay = 30; // time delay between server and client (seconds)

    //    return new TokenDto
    //    {
    //        AccessToken = st.Jwt,
    //        ExpiredDate = st.ExpiredDate.AddSeconds(-delay),
    //        Roles = roles
    //    };
    //}

    #endregion

    #region -- Classes --

    /// <summary>
    /// Base
    /// </summary>
    public class BaseDto : IdDto
    {
        #region -- Properties --

        /// <summary>
        /// LastName
        /// </summary>
        public string? LastName { get; set; }

        /// <summary>
        /// FirstName
        /// </summary>
        public string? FirstName { get; set; }

        /// <summary>
        /// Avatar
        /// </summary>
        public string? Avatar { get; set; }

        /// <summary>
        /// Email
        /// </summary>
        public string? Email { get; set; }

        /// <summary>
        /// CreatedOn
        /// </summary>
        public DateTime? CreatedOn { get; set; }

        #endregion
    }

    /// <summary>
    /// Search
    /// </summary>
    public class SearchDto : BaseDto
    {
    }

    /// <summary>
    /// View
    /// </summary>
    public class ViewDto : BaseDto
    {
    }

    /// <summary>
    /// Profile
    /// </summary>
    public class ProfileDto : IdDto
    {
        #region -- Properties --

        /// <summary>
        /// UserName
        /// </summary>
        public string? UserName { get; set; }

        /// <summary>
        /// ProfileName
        /// </summary>
        public string? ProfileName { get; set; }

        #endregion
    }

    #endregion
}
