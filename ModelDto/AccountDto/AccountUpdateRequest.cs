using Models.Enums;
using Models;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;


namespace ModelDto.AccountDto
{
    /// <summary>
    /// Data Transfer Object (DTO) for updating an existing account.
    /// </summary>
    public class AccountUpdateRequest
    {
        [StringLength(250, ErrorMessage = "Account name cannot exceed 250 characters.")]
        public string AccountName { get; set; } = string.Empty;

        [EmailAddress(ErrorMessage = "Invalid email address format.")]
        public string AccountEmail { get; set; } = string.Empty;

        [EnumDataType(typeof(GenderOptions), ErrorMessage = "Invalid Gender Options.")]
        public GenderOptions Gender { get; set; }

        [ValidateBirthDate]
        [JsonConverter(typeof(Newtonsoft.Json.Converters.IsoDateTimeConverter))]
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
}
