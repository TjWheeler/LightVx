using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace LightVx.Validators
{
    /// <summary>
    ///     Validates string is equal or not equal to a specific value
    /// </summary>
    [System.AttributeUsage(System.AttributeTargets.Property)]
    public class StringEqualsAttribute : AttributeValidator
    {
        public StringEqualsAttribute(string expectedString, bool ignoreCase = false) : base(new StringEqualsValidator(expectedString, ignoreCase)) { }
    }
    /// <summary>
    ///     Validates name type text. Allows alpha, spaces, hyphen and apostrophe
    /// </summary>
    public class StringEqualsValidator : AlphaTextValidator
    {
        private bool _ignoreCase;
        private string _expectedString;

        public StringEqualsValidator(string expectedString, bool ignoreCase = false)
        {
            _expectedString = expectedString;
            _ignoreCase = ignoreCase;
        }

        public override void Validate()
        {
            if (Input == null || Input.ToString() == string.Empty)
            {
                Succeed();
                return;
            }

            if (!(Input is string))
            {
                Fail("is not a string");
                return;
            }

            if (!_ignoreCase && string.Equals(Input.ToString(), _expectedString, StringComparison.Ordinal))
            {
                Succeed();
                return;
            }
            else if (_ignoreCase && string.Equals(Input.ToString(), _expectedString, StringComparison.OrdinalIgnoreCase))
            {
                Succeed();
                return;
            }
            else
            {
                Fail($"is not equal to {_expectedString}");
            }
        }
    }
}
