using System;
using System.Collections;

namespace LightVx.Validators
{
    [System.AttributeUsage(System.AttributeTargets.Property)]
    public class MaxAttribute : AttributeValidator
    {
        public MaxAttribute(int max) : base(new MaxValidator(max)) { }
        public MaxAttribute(decimal max) : base(new MaxValidator(max)) { }
        public MaxAttribute(double max) : base(new MaxValidator(max)) { }
    }
    public class MaxValidator : ValidatorBase
    {
        private readonly int? _intMax;
        private decimal? _decimalMax;
        private double? _doubleMax;

        public MaxValidator(int intMax)
        {
            _intMax = intMax;
        }

        public MaxValidator(decimal decimalMax)
        {
            _decimalMax = decimalMax;
        }

        public MaxValidator(double doubleMax)
        {
            _doubleMax = doubleMax;
        }

        public override void Validate()
        {
            if (Input == null || Input.ToString() == string.Empty)
            {
                Succeed();
                return;
            }

            if (Input is decimal)
            {
                ValidateAsDecimal((decimal) Input, $"is more than {_decimalMax}");
                return;
            }

            if (Input is double)
            {
                ValidateAsDouble((double) Input, $"is more than {_doubleMax}");
                return;
            }

            if (Input is int)
            {
                ValidateAsInt((int) Input, $"is more than {_intMax}");
                return;
            }

            if (Input is string)
            {
                ValidateString();
                return;
            }

            Fail($"contains more than {_intMax} items");
            if (Input is Array)
            {
                ValidateAsInt(((Array) Input).Length, $"contains more than {_intMax} items");
            }
            else if (Input is ICollection)
            {
                ValidateAsInt(((ICollection) Input).Count, $"contains more than {_intMax} items");
            }
            else
            {
                Fail("cannot be validated using the Max method");
            }
        }

        private void ValidateString()
        {
            if (Input.ToString().Contains(".") && decimal.TryParse(Input.ToString(), out var decimalValue))
            {
                ValidateAsDecimal(decimalValue, $"is not less than {_decimalMax}");
            }
            else if (Input.ToString().Contains(".") && double.TryParse(Input.ToString(), out var doubleValue))
            {
                ValidateAsDouble(doubleValue, $"is not less than {_doubleMax}");
            }
            else if (int.TryParse(Input.ToString(), out var intValue))
            {
                ValidateAsInt(intValue, $"is not less than {_intMax}");
            }
            else
            {
                Fail("is not a valid number");
            }
        }

        private void ValidateAsDecimal(decimal value, string failMessage)
        {
            if (!_decimalMax.HasValue && _intMax.HasValue)
            {
                _decimalMax = _intMax.Value;
            }

            if (value <= _decimalMax)
            {
                Succeed();
            }
            else
            {
                Fail($"{failMessage}");
            }
        }

        private void ValidateAsDouble(double value, string failMessage)
        {
            if (!_doubleMax.HasValue && _intMax.HasValue)
            {
                _doubleMax = _intMax.Value;
            }

            if (value <= _doubleMax)
            {
                Succeed();
            }
            else
            {
                Fail($"{failMessage}");
            }
        }

        private void ValidateAsInt(int value, string failMessage)
        {
            if (value <= _intMax)
            {
                Succeed();
            }
            else
            {
                Fail($"{failMessage}");
            }
        }
    }
}