using System;
using System.Collections.Generic;
using System.Text;

namespace LightVx.Validators
{
    /// <summary>
    /// Checks to ensure the specified content does not exist within the input
    /// </summary>
    public class NotContainsValidator : ValidatorBase
    {
        private string[] _content;
        private bool _ignoreCase;

        public NotContainsValidator(string[] content, bool ignoreCase = false)
        {
            _ignoreCase = ignoreCase;
            _content = content;
        }
        public NotContainsValidator(string content, bool ignoreCase = false)
        {
            _ignoreCase = ignoreCase;
            _content = new[] { content };
        }
        protected override void Validate()
        {
            if (_Input == null || _Input.ToString() == string.Empty)
            {
                Succeed();
                return;
            }

            foreach (var content in _content)
            {
                if (_ignoreCase && _Input.ToString().ToLower().Contains(content.ToLower()))
                {
                    Fail($"cannot contain {content}");
                    return;
                }

                if (!_ignoreCase && _Input.ToString().Contains(content))
                {
                    Fail($"cannot contain {content}");
                    return;
                }
            }

            Succeed();
        }
    }
}
