using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;

namespace LightVx.Validators
{
    /// <summary>
    ///     Accepts other validators, and acts as an aggregate.  To combine validation techniques.
    ///     Eg a post code validator may be made up of the lengthvalidator and the numericvalidator.
    /// </summary>
    public class AggregatedValidator : ValidatorBase
    {
        #region private variables

        private readonly List<IValidator> _validators = new List<IValidator>();

        #endregion

        #region base implementation

        protected override string Expression => throw new NotImplementedException();

        #endregion

        #region Constructors

        public AggregatedValidator()
        {
        }

        public AggregatedValidator(IValidator[] validators)
        {
            foreach (var validator in validators) _validators.Add(validator);
        }

        #endregion

        #region public methods and properties

        public void AddValidator(IValidator validator)
        {
            _validators.Add(validator);
        }

        public StringCollection ErrorMessages { get; private set; } = new StringCollection();

        public override void Validate()
        {
            var sbErrorMessage = new StringBuilder();
            ErrorMessages = new StringCollection();
            foreach (var validator in _validators)
            {
                validator.Validate(_Input, FieldName, out _);
                if (validator.IsValid) continue;
                ErrorMessages.Add(validator.ErrorMessage);
                sbErrorMessage.AppendLine(validator.ErrorMessage);
            }
            _ErrorMessage = sbErrorMessage.ToString();
            IsValid = _validators.TrueForAll(t => t.IsValid);
        }

        public override bool Validate(object input, string fieldName, out string errorMessage)
        {
            var foundError = false;
            ErrorMessages = new StringCollection();
            errorMessage = string.Empty;
            foreach (var validator in _validators)
            {
                string error;
                var valid = validator.Validate(input, fieldName, out error);
                if (!valid)
                {
                    foundError = true;
                    ErrorMessages.Add(validator.ErrorMessage);
                    errorMessage = errorMessage + " " + error;
                }
            }

            if (foundError)
                return false;
            return true;
        }

        #endregion
    }
}