using Models;
using Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ModelDto.AccountDto
{
    /// <summary>
    /// Data Transfer Object (DTO) for creating a new account.
    /// </summary>
    public class AccountRequest
    {
        [Required(ErrorMessage = "Account name is required.")]
        [StringLength(250, ErrorMessage = "Account name cannot exceed 250 characters.")]
        public string AccountName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Account email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address format.")]
        public string AccountEmail { get; set; } = string.Empty;

        [Required(ErrorMessage = "Gender is required.")]
        [EnumDataType(typeof(GenderOptions), ErrorMessage = "Invalid Gender Options.")]
        public GenderOptions Gender { get; set; }

        [Required(ErrorMessage = "Birth date is required.")]
        [JsonConverter(typeof(Newtonsoft.Json.Converters.IsoDateTimeConverter))]
        [ValidateBirthDate] //validate birthdate
        public DateTime BirthDate { get; set; }

        public Account MapAccountRequest()
        {
            return new Account
            {
                CostumerName = this.AccountName,
                CostumerEmail = this.AccountEmail,
                Gender = this.Gender.ToString(),
                BirthDay = this.BirthDate,
            };
        }
    }

    /// <summary>
    /// Validate BirthDate
    /// </summary>
    public class ValidateBirthDate : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null)
                return ValidationResult.Success; // let required handle

            DateTime dateNow = DateTime.UtcNow;

            if (value is DateTime birthDate)
            {
                if (birthDate > dateNow)
                {
                    return new ValidationResult(ErrorMessage ?? "Birth Date must not exceed the current date and time.");
                }
            }

            return ValidationResult.Success;
        }

    }
    
}
