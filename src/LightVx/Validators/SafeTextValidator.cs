namespace LightVx.Validators
{
    /// <summary>
    ///     Safe Text matches Lower and upper case letters and all digits
    /// </summary>
    public class SafeTextValidator : ValidatorBase
    {
        private const string RegExpression =
            @"^[a-zA-Z0-9\s.\-']+$";

        #region base implementation

        protected override string Expression => RegExpression;

        #endregion

        protected override void Validate()
        {
            if (_Input == null || (string) _Input == string.Empty)
            {
                Succeed();
                return;
            }

            if (SingleMatch(_Input.ToString()))
                Succeed();
            else
                Fail("is not valid.");
        }
    }
}