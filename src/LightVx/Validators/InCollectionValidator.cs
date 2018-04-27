using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace LightVx.Validators
{
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
        protected override void Validate()
        {
            if (_Input == null || _Input.ToString() == string.Empty)
            {
                Succeed();
                return;
            }

            foreach (var item in _items)
            {
                if (!_ignoreCase && String.Equals(item?.ToString(), _Input.ToString(), StringComparison.CurrentCulture))
                {
                    Succeed();
                    return;
                }
                if (_ignoreCase && String.Equals(item?.ToString(), _Input.ToString(), StringComparison.CurrentCultureIgnoreCase))
                {
                    Succeed();
                    return;
                }
            }
            Fail("is not a valid selection.");
        }
    }
}
