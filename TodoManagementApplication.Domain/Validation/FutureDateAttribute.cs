using System;
using System.ComponentModel.DataAnnotations;

namespace TodoManagementApplication.Domain.Validation
{
    public class FutureDateAttribute : ValidationAttribute
    {
        private readonly string _comparisonProperty;

        public FutureDateAttribute(string comparisonProperty)
        {
            _comparisonProperty = comparisonProperty;
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null) return new ValidationResult("Due date is required.");

            var currentValue = (DateTime)value;

            var property = validationContext.ObjectType.GetProperty(_comparisonProperty);
            if (property == null)
                return new ValidationResult($"Unknown property: {_comparisonProperty}");

            var comparisonValue = property.GetValue(validationContext.ObjectInstance);

            if (comparisonValue == null || !(comparisonValue is DateTime createdDate))
                return new ValidationResult("Created date must be a valid date.");

            if (currentValue < createdDate)
                return new ValidationResult("Due date must be after the created date.");

            return ValidationResult.Success;
        }
    }
}
