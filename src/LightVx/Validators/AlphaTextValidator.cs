namespace LightVx.Validators
{
    /// <summary>
    ///     Validate text alpha and spaces
    /// </summary>
    [System.AttributeUsage(System.AttributeTargets.Property)]
    public class AlphaTextValidatorAttribute : AttributeValidator
    {
        public AlphaTextValidatorAttribute() : base(new AlphaTextValidator()) { }
    }
    /// <summary>
    ///     Validate text alpha and spaces
    /// </summary>
    public class AlphaTextValidator : ValidatorBase
    {
        private const string RegExpression = "^([a-zA-Z\\s]{1,})$";

        #region base implementation

        protected override string Expression => RegExpression;

        #endregion

        public override void Validate()
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