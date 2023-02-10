using System;

namespace LightVx.Validators
{
    /// <summary>
    ///     Validate text or arrays with length options
    /// </summary>
    [System.AttributeUsage(System.AttributeTargets.Property)]
    public class LengthAttribute : AttributeValidator
    {
        public LengthAttribute(int minOccurance, int? maxOccurance) : base(new LengthValidator(minOccurance, maxOccurance)) { }
    }
    [System.AttributeUsage(System.AttributeTargets.Property)]
    public class RequiredAttribute : AttributeValidator
    {
        public RequiredAttribute() : base(new LengthValidator(1)) { }
    }
    /// <summary>
    ///     Validate text or arrays with length options
    /// </summary>
    public class LengthValidator : ValidatorBase
    {
        private readonly int? _maxOccurance;
        private readonly int _minOccurance;

        public LengthValidator(int minOccurance)
        {
            _minOccurance = minOccurance;
        }

        public LengthValidator(int minOccurance, int? maxOccurance)
        {
            _minOccurance = minOccurance;
            _maxOccurance = maxOccurance;
        }

        #region base implementation

        protected override string Expression { get; }

        #endregion

        public override void Validate()
        {
            if (Input != null && Input is Array)
            {
                ValidateArray();
                return;
            }

            if (_Input == null && _minOccurance == 0)
            {
                Succeed();
                return;
            }

            if (_Input == null && _minOccurance > 0 && _maxOccurance.HasValue)
            {
                Fail(
                    string.Format("has no data (null) and is not valid. Must have a length of between {0} and {1}.",
                        _minOccurance, _maxOccurance));
                return;
            }

            if (_Input == null && _minOccurance > 0 && !_maxOccurance.HasValue)
            {
                Fail(
                    string.Format(_minOccurance == 1 ? "is required" : "has no data (null) and is not valid. Must have at least a length of {0}.",
                        _minOccurance));
                return;
            }

            if (_Input != null && (_Input.ToString().Length >= _minOccurance && !_maxOccurance.HasValue))
            {
                Succeed();
            }
            else if (_Input != null && (_Input.ToString().Length >= _minOccurance && _Input.ToString().Length <= _maxOccurance))
            {
                Succeed();
            }
            else
            {
                string message;
                if (_minOccurance == _maxOccurance)
                    message =
                        string.Format("is not a valid length. Must have a length of {0}.", _minOccurance);
                else
                    message =
                        string.Format("is not a valid length. Must have a length of between {0} and {1}.",
                            _minOccurance,
                            _maxOccurance);
                Fail(message);
            }
        }

        private void ValidateArray()
        {
            if(((Array)Input).Length < _minOccurance)
            {
                Fail(
                    string.Format("is not valid. Must have a length greater than or equal to {0}.",
                        _minOccurance));
                return;
            }

            if (_maxOccurance.HasValue)
            {
                if (_maxOccurance.Value < ((Array)Input).Length)
                {
                    Fail(
                        string.Format("is not valid. Must have a length of between {0} and {1}.",
                            _minOccurance, _maxOccurance));
                    return;
                }
            }
            Succeed();
        }
    }
}