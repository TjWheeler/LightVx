using System.Diagnostics;
using System.Text.RegularExpressions;

namespace LightVx
{
    public abstract class ValidatorBase : IValidator
    {
        #region private functions

        private void ResetFieldName()
        {
            _FieldName = DefaultFieldName;
        }

        #endregion

        #region private and protected variables

        protected object _Input;
        protected string _ErrorMessage = string.Empty;
        protected bool _IsValid;
        public const string DefaultFieldName = "The Field";
        protected string _FieldName = DefaultFieldName;
        

        #endregion

        #region Derived class helper functions

        protected void Fail(string message)
        {
            _IsValid = false;
            _ErrorMessage = $"{_FieldName} {message}";
        }

        protected void Succeed()
        {
            _IsValid = true;
            _ErrorMessage = string.Empty;
        }

        #endregion

        #region Abstract methods

        /// <summary>
        ///     For the derived class to specify its regex expression
        /// </summary>
        protected virtual string Expression { get; }

        protected abstract void Validate();

        #endregion

        #region public methods and properties

        public virtual bool Validate(object input, string fieldName, out string errorMessage)
        {
            _Input = input;
            _FieldName = fieldName;
            Validate();
            ResetFieldName();
            errorMessage = _ErrorMessage;
            return _IsValid;
        }

        public virtual bool Validate(object input, string fieldName)
        {
            _Input = input;
            _FieldName = fieldName;
            Validate();
            ResetFieldName();
            return _IsValid;
        }

        public virtual bool Validate(object input)
        {
            _Input = input;
            Validate();
            return _IsValid;
        }

        public object Input
        {
            get => _Input;
            set
            {
                _Input = value;
                _IsValid = false; //Reset the state
                _ErrorMessage = "Valdate() function not yet called";
            }
        }

        public bool IsValid
        {
            get => _IsValid;
            set => _IsValid = value;
        }

        public string ErrorMessage
        {
            get => _ErrorMessage;
            set => _ErrorMessage = value;
        }

        public string FieldName
        {
            get => _FieldName;
            set
            {
                if (value == null)
                    _FieldName = DefaultFieldName;
                else
                    _FieldName = value;
            }
        }

        #endregion

        #region Regex helper methods

        protected bool HasMatch(string input)
        {
            return Regex.Matches(input, Expression).Count > 0;
        }

        protected bool SingleMatch(string input)
        {
            return Regex.Matches(input, Expression).Count == 1;
        }

        protected int MatchCount(string input)
        {
            return Regex.Matches(input, Expression).Count;
        }

        protected bool HasMatch(string input, string expression)
        {
            return Regex.Matches(input, expression).Count > 0;
        }

        protected bool SingleMatch(string input, string expression)
        {
            return Regex.Matches(input, expression).Count == 1;
        }

        protected int MatchCount(string input, string expression)
        {
            return Regex.Matches(input, expression).Count;
        }

        #endregion
    }
}