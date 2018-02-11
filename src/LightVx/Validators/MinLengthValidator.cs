using System;
using System.Collections.Generic;
using System.Text;

namespace LightVx.Validators
{
    public class MinLengthValidator : ValidatorBase
    {
        private readonly int _minLength;

        public MinLengthValidator(int minLength)
        {
            _minLength = minLength;
        }
        protected override void Validate()
        {
            if (_Input == null)
            {
                Succeed();
                return;
            }
            if (Input.ToString().Length >= _minLength)
            {
                Succeed();
            }
            else
            {
                Fail($"does not meet the minimum length requirement of {_minLength}");
            }
        }
    }
}
