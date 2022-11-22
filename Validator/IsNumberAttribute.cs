using System.ComponentModel.DataAnnotations;

namespace la_mia_pizzeria_static.Validator
{
    public class IsNumberAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            try
            {
                float fieldValue = (float)value;


                return ValidationResult.Success;
            }catch(Exception ex) { return new ValidationResult("Il campo deve essere numerico"); }
        }
    }
}
