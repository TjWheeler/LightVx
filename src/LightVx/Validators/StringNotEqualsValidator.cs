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
    public class StringNotEqualsAttribute : AttributeValidator
    {
        public StringNotEqualsAttribute(string expectedString, bool ignoreCase = false) : base(new StringNotEqualsValidator(expectedString, ignoreCase)) { }
    }
    /// <summary>
    ///     Validates name type text. Allows alpha, spaces, hyphen and apostrophe
    /// </summary>
    public class StringNotEqualsValidator : AlphaTextValidator
    {
        private bool _ignoreCase;
        private string _expectedString;

        public StringNotEqualsValidator(string expectedString, bool ignoreCase = false)
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

            if (!_ignoreCase && !string.Equals(Input.ToString(), _expectedString, StringComparison.Ordinal))
            {
                Succeed();
                return;
            }
            else if (_ignoreCase && !string.Equals(Input.ToString(), _expectedString, StringComparison.OrdinalIgnoreCase))
            {
                Succeed();
                return;
            }
            else
            {
                Fail($"is equal to {_expectedString}");
            }
        }
    }
}
