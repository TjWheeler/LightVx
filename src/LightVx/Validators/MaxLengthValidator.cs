using System;
using System.Collections.Generic;
using System.Text;

namespace LightVx.Validators
{
    public class MaxLengthValidator : ValidatorBase
    {
        private readonly int _maxLength;

        public MaxLengthValidator(int maxLength)
        {
            _maxLength = maxLength;
        }
        protected override void Validate()
        {
            if (_Input == null)
            {
                Succeed();
                return;
            }
            if (Input.ToString().Length <= _maxLength)
            {
                Succeed();
            }
            else
            {
                Fail($"is more than the maximum length of {_maxLength}");
            }
        }
    }
}
