using System;
using System.Collections.Generic;
using System.Text;

namespace LightVx.Validators
{
    [System.AttributeUsage(System.AttributeTargets.Property)]
    public class MinLengthAttribute : AttributeValidator
    {
        public MinLengthAttribute(int minLength) : base(new MinLengthValidator(minLength)) { }
    }
    public class MinLengthValidator : ValidatorBase
    {
        private readonly int _minLength;

        public MinLengthValidator(int minLength)
        {
            _minLength = minLength;
        }
        public override void Validate()
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
