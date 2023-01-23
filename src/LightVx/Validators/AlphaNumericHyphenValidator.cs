namespace LightVx.Validators
{
    /// <summary>
    ///     Validate text with alpha, numeric, hyphen and spaces.
    /// </summary>
    [System.AttributeUsage(System.AttributeTargets.Property)]
    public class AlphaNumericHyphenValidatorAttribute : AttributeValidator
    {
        public AlphaNumericHyphenValidatorAttribute() : base(new AlphaNumericHyphenValidator()) { }
    }

    /// <summary>
    ///     Validate text with alpha, numeric, hyphen and spaces.
    /// </summary>
    public class AlphaNumericHyphenValidator : AlphaTextValidator
    {
        private const string RegExpression = "^([a-zA-Z0-9\\s\\-]{1,})$";

        #region base implementation

        protected override string Expression => RegExpression;

        #endregion
    }
}