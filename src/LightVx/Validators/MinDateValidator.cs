using System;
using System.Collections.Generic;
using System.Text;

namespace LightVx.Validators
{
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
        protected override void Validate()
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