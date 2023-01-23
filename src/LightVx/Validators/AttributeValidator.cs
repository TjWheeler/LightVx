using System;
using System.Collections.Generic;
using System.Text;

namespace LightVx.Validators
{
    [System.AttributeUsage(System.AttributeTargets.Property)]
    public abstract class AttributeValidator : System.Attribute, IAttributeValidator
    {
        private IValidator _validator;
        public AttributeValidator(IValidator validator)
        {
            _validator = validator;
        }

        public IValidator Validator
        {
            get
            {
                return _validator;
            }
        }
    }
}
