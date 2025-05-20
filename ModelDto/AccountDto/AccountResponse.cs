using Models;
using Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelDto.AccountDto
{
     /// <summary>
     /// Data Transfer Object (DTO) for returning account information.
     /// </summary>
    public class AccountResponse : IEquatable<Account>
    {
        public string AccountNumber { get; set; }
        public string CostumerName { get; set; }
        public string CostumerEmail { get; set; }
        public string? Gender { get; set; }
        public DateTime? BirthDay { get; set; }
        public int Age { get; set; }
        public decimal? CurrentBalance { get; set; }
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Determines whether the specified Account is equal to the current AccountResponse.
        /// </summary>
        /// <param name="other">The Account to compare with the current AccountResponse.</param>
        /// <returns>true if the specified Account is equal to the current AccountResponse; otherwise, false.</returns>
        public bool Equals(Account? other)
        {
            if (other == null)
                return false;

            // Compare all relevant properties for equality
            if (AccountNumber == other.AccountId &&
                CostumerName == other.CostumerName &&
                CostumerEmail == other.CostumerEmail &&
                Gender == other.Gender &&
                BirthDay == other.BirthDay &&
                CurrentBalance == other.CurrentBalance &&
                CreatedAt == other.CreatedAt)
                return true;

            return false;
        }

        /// <summary>
        /// Determines whether the specified object is equal to the current AccountResponse.
        /// </summary>
        /// <param name="obj">The object to compare with the current AccountResponse.</param>
        /// <returns>true if the specified object is equal to the current AccountResponse; otherwise, false.</returns>
        public override bool Equals(object? obj) => Equals(obj as AccountResponse);

        /// <summary>
        /// Serves as the default hash function.
        /// </summary>
        /// <returns>A hash code for the current object.</returns>
        public override int GetHashCode()
        {
            return HashCode.Combine(AccountNumber, CostumerName, CostumerEmail, Gender, BirthDay, CurrentBalance,CreatedAt);
        }

        /// <summary>
        /// Returns a string that represents the current AccountResponse.
        /// </summary>
        /// <returns>A string representation of the AccountResponse.</returns>
        public override string ToString()
        {
            return $"AccountNumber: {AccountNumber}, CostumerName: {CostumerName}, CostumerEmail: {CostumerEmail}, Gender: {Gender}, BirthDay: {BirthDay}, Age: {Age}, CurrentBalance: {CurrentBalance}, CreatedAt: {CreatedAt:O}";
        }
        /// <summary>
        /// Maps the AccountResponse to an AccountUpdateRequest.
        /// </summary>
        /// <returns></returns>
        public AccountUpdateRequest AccountUpdateRequest()
        {
            return new AccountUpdateRequest
            {
                AccountName = this.CostumerName,
                AccountEmail = this.CostumerEmail,
                Gender = !string.IsNullOrEmpty(this.Gender)
                    ? (GenderOptions)Enum.Parse(typeof(GenderOptions), this.Gender, true)
                    : default,
                BirthDate = this.BirthDay ?? DateTime.MinValue
            };
        }
    }
    /// <summary>
    /// Extension methods for Account to AccountResponse conversion and age calculation.
    /// </summary>
    public static class AccountResponseExtension
    {
        /// <summary>
        /// Converts an Account entity to an AccountResponse DTO.
        /// </summary>
        /// <param name="account">The Account entity to convert.</param>
        /// <returns>An AccountResponse DTO with mapped properties.</returns>
        public static AccountResponse GetAccountResponse(this Account account)
        {
            return new AccountResponse
            {
                AccountNumber = account.AccountId,
                CostumerName = account.CostumerName,
                CostumerEmail = account.CostumerEmail,
                BirthDay = account.BirthDay,
                Gender = account.Gender,
                Age = CalculateAge(account.BirthDay),
                CurrentBalance = account.CurrentBalance,
                CreatedAt = account.CreatedAt
            };
        }

        /// <summary>
        /// Calculates the age based on the given birth date and optional reference date.
        /// </summary>
        /// <param name="birthDate">The birth date to calculate age from.</param>
        /// <param name="referenceDate">The date to calculate age relative to (defaults to today).</param>
        /// <returns>The calculated age, or 0 if birthDate is null.</returns>
        public static int CalculateAge(DateTime? birthDate, DateTime? referenceDate = null)
        {
            if (birthDate == null)
                return 0;

            var today = referenceDate ?? DateTime.Today;
            var age = today.Year - birthDate.Value.Year;
            if (birthDate.Value.Date > today.AddYears(-age)) age--;
            return age;
        }
    }
    
}
