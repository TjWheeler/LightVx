using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace LightVx.Validators
{
    public class RegExValidator : ValidatorBase
    {
        private string _expression;

        public RegExValidator(string expression)
        {
            _expression = expression;
        }

        protected override void Validate()
        {
            if (_Input == null || (string)_Input == string.Empty)
            {
                Succeed();
                return;
            }
            try
            {
                if (SingleMatch(_Input.ToString()))
                    Succeed();
                else
                    Fail("is not valid.");
            }
            catch
            {
                Fail("could not be validated due to a Regular Expression error.");
            }
        }
        protected new bool SingleMatch(string input)
        {
            return Regex.Matches(input, _expression).Count == 1;

        }
    }
}
