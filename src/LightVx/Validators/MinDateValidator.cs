using System;
using System.Collections.Generic;
using System.Text;
using LightVx.Attribute;

namespace LightVx.Validators
{
    /// <summary>
    /// Verify that the input is a datetime and greater than or equal to the datetime specified in the constructor.
    /// </summary>
    [System.AttributeUsage(System.AttributeTargets.Property)]
    public class MinDateAttribute : DateAttributeValidator
    {
        public MinDateAttribute(DateTypeEnum dateType, DateOffsetEnum offsetType, int offset = 0)
        {
            DateTime date = CalculateDateOffset(dateType, offsetType, offset);
            Validator = new MinDateValidator(date);
        }
    }
    /// <summary>
    /// Verify that the input is a datetime and greater than or equal to the datetime specified in the constructor.
    /// </summary>
    public class MinDateValidator : ValidatorBase
    {
        private DateTime? _minDate;

        public MinDateValidator(DateTime minDate)
        {
            _minDate = minDate;
        }
        public MinDateValidator(DateTime? minDate)
        {
            _minDate = minDate;
        }
        public override void Validate()
        {
            if (_Input == null)
            {
                Succeed();
                return;
            }
            if (_Input.GetType() != typeof(DateTime) && _Input.GetType() != typeof(DateTime?))
            {
                Fail("is not a date data type.");
                return;
            }
            if (_Input is DateTime && ((DateTime)_Input) < _minDate)
            {
                Fail("does not meet the minimum date range.");
                return;
            }
            if (_Input is DateTime? && ((DateTime?)_Input).HasValue && ((DateTime?)_Input).Value < _minDate)
            {
                Fail("does not meet the minimum date range.");
                return;
            }
            Succeed();
        }
    }
}