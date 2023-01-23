namespace LightVx.Validators
{
    /// <summary>
    ///     Validate numbers only
    /// </summary>
    [System.AttributeUsage(System.AttributeTargets.Property)]
    public class NumericValidatorAttribute : AttributeValidator
    {
        public NumericValidatorAttribute() : base(new NumericValidator()) { }
    }
    /// <summary>
    ///     Validate numbers only
    /// </summary>
    public class NumericValidator : ValidatorBase
    {
        private const string RegExpression = @"^\d+$";

        #region base implementation

        protected override string Expression => RegExpression;

        #endregion

        public override void Validate()
        {
            if (_Input == null || _Input is string && (string) _Input == string.Empty)
            {
                Succeed();
                return;
            }

            if (SingleMatch(_Input.ToString()))
                Succeed();
            else
                Fail("is not a valid number. It can only contain numeric values.");
        }
    }
}