using System;
using System.Collections;

namespace LightVx.Validators
{
    [System.AttributeUsage(System.AttributeTargets.Property)]
    public class MinAttribute : AttributeValidator
    {
        public MinAttribute(int min) : base(new MinValidator(min)) { }
        public MinAttribute(decimal min) : base(new MinValidator(min)) { }
        public MinAttribute(double min) : base(new MinValidator(min)) { }
    }
    public class MinValidator : ValidatorBase
    {
        private decimal? _decimalMin;
        private double? _doubleMin;
        private readonly int? _intMin;

        public MinValidator(int intMin)
        {
            _intMin = intMin;
        }

        public MinValidator(decimal decimalMin)
        {
            _decimalMin = decimalMin;
        }

        public MinValidator(double doubleMin)
        {
            _doubleMin = doubleMin;
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
                ValidateAsDecimal((decimal) Input, $"is not greater than {_decimalMin}");
                return;
            }

            if (Input is double)
            {
                ValidateAsDouble((double) Input, $"is not greater than {_doubleMin}");
                return;
            }

            if (Input is int)
            {
                ValidateAsInt((int) Input, $"is not greater than {_intMin}");
                return;
            }

            if (Input is string)
            {
                ValidateString();
                return;
            }

            Fail($"does not contain at least {_intMin} items");
            if (Input is Array)
            {
                ValidateAsInt(((Array) Input).Length, $"does not contain at least {_intMin} items");
            }
            else if (Input is ICollection)
            {
                ValidateAsInt(((ICollection) Input).Count, $"does not contain at least {_intMin} items");
            }
            else
            {
                Fail("cannot be validated using the Min method");
            }
        }

        private void ValidateString()
        {
            if (Input.ToString().Contains(".") && decimal.TryParse(Input.ToString(), out var decimalValue))
            {

                ValidateAsDecimal(decimalValue, $"is not greater than {_decimalMin}");
            }
            else if (Input.ToString().Contains(".") && double.TryParse(Input.ToString(), out var doubleValue))
            {
                ValidateAsDouble(doubleValue, $"is not greater than {_doubleMin}");
            }
            else if (int.TryParse(Input.ToString(), out var intValue))
            {
                ValidateAsInt(intValue, $"is not greater than {_intMin}");
            }
            else
            {
                Fail("is not a valid number");
            }
        }

        private void ValidateAsDecimal(decimal value, string failMessage)
        {
            if (!_decimalMin.HasValue && _intMin.HasValue)
            {
                _decimalMin = (decimal) _intMin.Value;
            }
            else if (!_decimalMin.HasValue && _doubleMin.HasValue)
            {
                _decimalMin = (decimal) _doubleMin.Value;
            }
            if (value >= _decimalMin)
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
            if (!_doubleMin.HasValue && _intMin.HasValue)
            {
                _doubleMin = (double)_intMin.Value; 
            }
            else if (!_doubleMin.HasValue && _decimalMin.HasValue)
            {
                _doubleMin = (double)_decimalMin.Value;
            }
            if (value >= _doubleMin)
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
            if (value >= _intMin)
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