using FluentValidation;
using GTBack.Core.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTBack.Service.Validation.Tool
{
    public static class FluentValidationTool
    {
        public static void Validate<T>(IValidator<T> validator, T entity)
        {
            var result = validator.Validate(entity);
            if (!result.IsValid)
            {
                throw new ValidationException("Validation failed", result.Errors);
            }
        }

        public static ModelStateValidationResult ValidateModelWithKeyResult<T>(IValidator<T> validator, T entity)
        {
            ModelStateValidationResult validationResult = new ModelStateValidationResult { Success = true };

            if (entity == null)
            {
                validationResult.Success = false;
                validationResult.Errors.Add("", "Entity was null.");
                return validationResult;
            }

            var result = validator.Validate(entity);
            if (!result.IsValid)
            {
                validationResult.Success = false;
                foreach (var item in result.Errors)
                {
                    if (validationResult?.Errors.ContainsKey(item.PropertyName) == false)
                        validationResult.Errors.Add(item.PropertyName, item.ErrorMessage);
                }

                return validationResult;
            }

            return validationResult;
        }
    }
}
