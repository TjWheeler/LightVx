using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace LightVx.Validators
{
    /// <summary>
    /// Checks if the input is within the items of a collection, optionally ignore case.
    /// </summary>
    [System.AttributeUsage(System.AttributeTargets.Property)]
    public class InCollectionValidatorAttribute : AttributeValidator
    {
        public InCollectionValidatorAttribute(ICollection items, bool ignoreCase = false) : base(new InCollectionValidator(items, ignoreCase)) { }
    }
    /// <summary>
    /// Checks if the input is within the items of a collection, optionally ignore case.
    /// </summary>
    public class InCollectionValidator : ValidatorBase
    {
        private bool _ignoreCase;
        private ICollection _items;

        public InCollectionValidator(ICollection items, bool ignoreCase = false)
        {
            _items = items;
            _ignoreCase = ignoreCase;
        }
        public override void Validate()
        {
            if (_Input == null || _Input.ToString() == string.Empty)
            {
                Succeed();
                return;
            }
            if (Input is Array || Input is ICollection)
            {
                if(Input is Array && ((Array)Input).Length == 0) { Succeed(); return; }
                if (Input is ICollection && ((ICollection)Input).Count == 0) { Succeed(); return; }
                bool isValid = true;
                foreach (var input in (IEnumerable) _Input)
                {
                    if (!ValidateItem(input))
                    {
                        isValid = false;
                        break;
                    };
                }
                if (isValid)
                {
                    return;
                }
            }
            else
            {
                if (ValidateItem(_Input)) return;
            }
            Fail("is not a valid selection.");
        }

        private bool ValidateItem(object input)
        {
            foreach (var item in _items)
            {
                if (!_ignoreCase && String.Equals(item?.ToString(), input.ToString(), StringComparison.CurrentCulture))
                {
                    Succeed();
                    return true;
                }

                if (_ignoreCase && String.Equals(item?.ToString(), input.ToString(), StringComparison.CurrentCultureIgnoreCase))
                {
                    Succeed();
                    return true;
                }
            }

            return false;
        }
    }
}
