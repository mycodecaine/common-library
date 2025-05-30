﻿using Codecaine.Common.Primitives.Errors;
using FluentValidation;

namespace Codecaine.Common.Extensions
{
    /// <summary>
    /// Provides extension methods for FluentValidation to support custom error handling.
    /// </summary>
    public static class FluentValidationExtensions
    {
        /// <summary>
        /// Specifies a custom error to use if validation fails.
        /// </summary>
        /// <typeparam name="T">The type being validated.</typeparam>
        /// <typeparam name="TProperty">The property being validated.</typeparam>
        /// <param name="rule">The current rule.</param>
        /// <param name="error">The error to use.</param>
        /// <returns>The same rule builder.</returns>
        public static IRuleBuilderOptions<T, TProperty> WithError<T, TProperty>(
            this IRuleBuilderOptions<T, TProperty> rule, Error error)
        {
            if (error is null)
            {
                throw new ArgumentNullException(nameof(error), "The error is required");
            }

            return rule.WithErrorCode(error.Code).WithMessage(error.Message);
        }
    }
}
