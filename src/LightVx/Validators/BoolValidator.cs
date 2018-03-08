namespace LightVx.Validators
{
    public class BoolValidator : ValidatorBase
    {
        protected override void Validate()
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
