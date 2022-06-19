namespace LightVx.Validators
{
    public class IntValidator : ValidatorBase
    {
        protected override void Validate()
        {
            if (_Input == null || _Input is string && (string)_Input == string.Empty)
            {
                Succeed();
                return;
            }

            if (_Input is int)
            {
                Succeed();
            }
            else if (int.TryParse(_Input.ToString(), out var _))
            {
                Succeed();
            }
            else
            {
                Fail("is not a valid Integer.");
            }
        }
    }
}
