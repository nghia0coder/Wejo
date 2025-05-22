namespace Wejo.Common.Domain.Entities;

using Core.Enums;
using SeedWork.Dtos;

partial class User
{
    #region -- Methods --

    /// <summary>
    /// Initialize
    /// </summary>
    public User()
    {
        CreatedOn = DateTime.UtcNow;
    }

    /// <summary>
    /// Update
    /// </summary>
    /// <param name="id"></param>
    /// <param name="firstName"></param>
    /// <param name="lastName"></param>
    /// <param name="phoneNumber"></param>
    /// <param name="phoneNumberConfirmed"></param>
    /// <param name="email"></param>
    /// <param name="emailConfirmed"></param>
    public static User Create(string id, string? firstName, string? lastName, string? phoneNumber, bool? phoneNumberConfirmed, string? email, bool? emailConfirmed)
    {
        var res = new User
        {
            Id = id,
            FirstName = firstName,
            LastName = lastName,
            PhoneNumber = phoneNumber,
            PhoneNumberConfirmed = phoneNumberConfirmed,
            Email = email,
            EmailConfirmed = emailConfirmed
        };

        return res;
    }

    /// <summary>
    /// Update
    /// </summary>
    /// <param name="firstName"></param>
    /// <param name="lastName"></param>
    /// <param name="bio"></param>
    /// <param name="gender"></param>
    public void Update(string? firstName, string? lastName, string? bio, Gender gender, string? imgUrl)
    {
        FirstName = firstName;
        LastName = lastName;
        Bio = bio;
        Gender = gender;
        Avatar = imgUrl;

        ModifiedOn = DateTime.UtcNow;
    }


    /// <summary>
    /// Convert to data transfer object
    /// </summary>
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
            Bio = Bio,
            Gender = Gender.ToString(),
            Avatar = Avatar
        };
    }

    #endregion

    #region -- Classes --

    /// <summary>
    /// Base
    /// </summary>
    public class BaseDto
    {
        #region -- Properties --

        /// <summary>
        /// Id
        /// </summary>
        public string? Id { get; set; }

        /// <summary>
        /// FirstName
        /// </summary>
        public string? FirstName { get; set; }

        /// <summary>
        /// LastName
        /// </summary>
        public string? LastName { get; set; }

        /// <summary>
        /// Bio
        /// </summary>
        public string? Bio { get; set; }

        /// <summary>
        /// Avatar
        /// </summary>
        public string? Avatar { get; set; }

        /// <summary>
        /// Gender
        /// </summary>
        public string? Gender { get; set; }

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
