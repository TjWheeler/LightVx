using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;using LightVx.Validators;

namespace LightVx.Tests.CustomValidator
{
    /// <summary>
    /// This example validates a post code which is exactly 4 numbers.
    /// </summary>
    public class PostCodeValidator : AggregatedValidator
    {
        public PostCodeValidator()
        {
            AddValidator(new LengthValidator(4, 4));
            AddValidator(new NumericValidator());
        }
    }
    /// <summary>
    /// This extension example adds the IsPostCode method to the fluent api
    /// </summary>
    public static class PostCodeValidatorExtension
    {
        public static ValidatorFluent IsPostCode(this ValidatorFluent fluentApi)
        {
            fluentApi.AddValidator(new PostCodeValidator());
            return fluentApi;
        }
    }
}
