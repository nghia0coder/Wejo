namespace Wejo.Common.SeedWork.Constants;

public class Validator
{
    /// <summary>
    /// NotEmpty message
    /// </summary>
    public const string NotEmpty = "must be specifed";

    /// <summary>
    /// MinimumLength message
    /// </summary>
    public const string MinimumLength = "does not exceed the authorized size";

    /// <summary>
    /// MaximumLength message
    /// </summary>
    public const string MaximumLength = "exceeds the authorized size";

    /// <summary>
    /// Invalid Length
    /// </summary>
    public const string InvalidLength = "Invalid Length";

    /// <summary>
    /// uid Length
    /// </summary>
    public const ushort uid = 28;

    /// <summary>
    /// PhoneNumber 
    /// </summary>
    public class PhoneNumber
    {
        /// <summary>
        /// Minimum length
        /// </summary>
        public const ushort Min = 10;

        /// <summary>
        /// Maximum length
        /// </summary>
        public const ushort Max = 15;

        /// <summary>
        /// PhoneNumber must contain only digits
        /// </summary>
        public const string Regex = @"^\+?\d+$";

        /// <summary>
        /// Regex message for validating.
        /// </summary>
        public const string Message = "must contain only digits";
    }

    /// <summary>
    /// OtpCode 
    /// </summary>
    public class OtpCode
    {
        /// <summary>
        /// Minimum length
        /// </summary>
        public const ushort Min = 6;

        /// <summary>
        /// Maximum length
        /// </summary>
        public const ushort Max = 6;

        /// <summary>
        /// PhoneNumber must contain only digits
        /// </summary>
        public const string Regex = "@\"^\\d+$\"";

        /// <summary>
        /// Regex message for validating.
        /// </summary>
        public const string Message = "must contain only digits";
    }
}
