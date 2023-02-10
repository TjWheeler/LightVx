namespace LightVx.Validators
{
    [System.AttributeUsage(System.AttributeTargets.Property)]
    public class BoolAttribute : AttributeValidator
    {
        public BoolAttribute() : base(new BoolValidator()) { }
    }
    public class BoolValidator : ValidatorBase
    {
        public override void Validate()
        {
            if (_Input == null || _Input is string && (string) _Input == string.Empty)
            {
                Succeed();
                return;
            }
            if (_Input is bool)
            {
                Succeed();
            }
            else if (bool.TryParse(_Input.ToString(), out var _))
            {
                Succeed();
            }
            else
            {
                Fail("is not valid.");
            }
        }
    }
}
