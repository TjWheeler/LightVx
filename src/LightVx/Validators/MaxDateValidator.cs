using System;
using System.Collections.Generic;
using System.Text;

namespace LightVx.Validators
{
    /// <summary>
    /// Verify that the input is a datetime and less or equal to the datetime specified in the constructor.
    /// </summary>
    public class MaxDateValidator : ValidatorBase
    {
        private DateTime? _maxDate;

        public MaxDateValidator(DateTime maxDate)
        {
            _maxDate = maxDate;
        }
        public MaxDateValidator(DateTime? maxDate)
        {
            _maxDate = maxDate;
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
            if (_Input is DateTime && ((DateTime)_Input) > _maxDate)
            {
                Fail("does not meet the maximum date range.");
                return;
            }
            if (_Input is DateTime? && ((DateTime?)_Input).HasValue && ((DateTime?)_Input).Value > _maxDate)
            {
                Fail("does not meet the maximum date range.");
                return;
            }
            Succeed();
        }
    }
}