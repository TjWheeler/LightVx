namespace LightVx.Validators
{
    public class DoubleValidator : ValidatorBase
    {
        protected override void Validate()
        {
            if (_Input == null || _Input is string && (string)_Input == string.Empty)
            {
                Succeed();
                return;
            }

            if (double.TryParse(_Input.ToString(), out var _))
            {
                Succeed();
            }
            else
            {
                Fail("is not a valid Decimal.");
            }
        }
    }
}
