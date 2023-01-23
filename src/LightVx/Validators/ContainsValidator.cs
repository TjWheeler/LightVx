using System;
using System.Collections.Generic;
using System.Text;

namespace LightVx.Validators
{
    /// <summary>
    /// Checks to ensure the specified content exists within the input
    /// </summary>
    [System.AttributeUsage(System.AttributeTargets.Property)]
    public class ContainsValidatorAttribute : AttributeValidator
    {
        public ContainsValidatorAttribute(string content, bool ignoreCase = false) : base(new ContainsValidator(content, ignoreCase)) { }
        public ContainsValidatorAttribute(string[] content, bool ignoreCase = false) : base(new ContainsValidator(content, ignoreCase)) { }
    }
    /// <summary>
    /// Checks to ensure the specified content exists within the input
    /// </summary>
    public class ContainsValidator : ValidatorBase
    {
        private string[] _content;
        private bool _ignoreCase;

        public ContainsValidator(string[] content, bool ignoreCase = false)
        {
            _ignoreCase = ignoreCase;
            _content = content;
        }
        public ContainsValidator(string content, bool ignoreCase = false)
        {
            _ignoreCase = ignoreCase;
            _content = new [] { content};
        }
        public override void Validate()
        {
            if (_Input == null || _Input.ToString() == string.Empty)
            {
                Succeed();
                return;
            }

            foreach (var content in _content)
            {
                if (_ignoreCase && !_Input.ToString().ToLower().Contains(content.ToLower()))
                {
                    Fail($"must contain {content}");
                    return;
                }

                if (!_ignoreCase && !_Input.ToString().Contains(content))
                {
                    Fail($"must contain {content}");
                    return;
                }
            }
            Succeed();
        }
    }
}
