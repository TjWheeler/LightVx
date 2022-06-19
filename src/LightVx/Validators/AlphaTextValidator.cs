namespace LightVx.Validators
{
    /// <summary>
    ///     Validate text with length options
    /// </summary>
    public class AlphaTextValidator : ValidatorBase
    {
        private const string RegExpression = "^([a-zA-Z\\s]{1,})$";

        #region base implementation

        protected override string Expression => RegExpression;

        #endregion

        protected override void Validate()
        {
            if (_Input == null || _Input is string && (string)_Input == string.Empty)
            {
                Succeed();
                return;
            }

            if (!SingleMatch(_Input?.ToString()))
            {
                Fail("does not contain correct values");
                return;
            }

            Succeed();
        }
    }
}