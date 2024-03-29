namespace LightVx.Validators
{
    /// <summary>
    ///     Validate text with alpha, numeric and spaces.
    /// </summary>
    [System.AttributeUsage(System.AttributeTargets.Property)]
    public class AlphaNumericAttribute : AttributeValidator
    {
        public AlphaNumericAttribute() : base(new AlphaNumericValidator()) { }
    }
    /// <summary>
    ///     Validate text with alpha, numeric and spaces.
    /// </summary>
    public class AlphaNumericValidator : ValidatorBase
    {
        private const string RegExpression = "^([a-zA-Z0-9\\s]{1,})$";

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