
using System.ComponentModel.DataAnnotations;
namespace HelperContainer
{
    public class HelperValidation
    {
        /// <summary>
        /// Model Validation
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="instance"></param>
        /// <returns>Return response from Model Validation </returns>
        public static List<string?> ModelValidationResponse<T>(T instance)
        {
            ResponseApi<T> response = new();
            if (instance == null)
            {
                response.isSuccess = false;
                response.Message = "Models are null";
                response.Errors.Add("Models Cannot be Null");
                return response.Errors;
            }
            List<ValidationResult> validationResults = new ();

            var validationContext = new ValidationContext(instance);
            Validator.TryValidateObject(instance, validationContext, validationResults, true);
            return validationResults.Select(x => x.ErrorMessage).ToList();

        }
    }
}
