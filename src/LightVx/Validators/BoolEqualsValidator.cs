namespace LightVx.Validators
{
    [System.AttributeUsage(System.AttributeTargets.Property)]
    public class BoolEqualsAttribute : AttributeValidator
    {
        public BoolEqualsAttribute(bool expected) : base(new BoolEqualsValidator(expected)) { }
    }
    public class BoolEqualsValidator : ValidatorBase
    {
        private bool _expected;

        public BoolEqualsValidator(bool expected) 
        {
            _expected = expected;
        }
        public override void Validate()
        {
            if (_Input == null || _Input is string && (string) _Input == string.Empty)
            {
                Succeed();
                return;
            }
            if (_Input is bool && (bool) _Input == _expected)
            {
                Succeed();
            }
            else if (bool.TryParse(_Input.ToString(), out var result) && result == _expected)
            {
                Succeed();
            }
            else
            {
                Fail($"is not valid and must be {_expected}.");
            }
        }
    }
}
