namespace LightVx.Validators
{
    /// <summary>
    ///     Validate text with length options
    /// </summary>
    [System.AttributeUsage(System.AttributeTargets.Property)]
    public class HexColorValidatorAttribute : AttributeValidator
    {
        public HexColorValidatorAttribute() : base(new HexColorValidator()) { }
    }
    /// <summary>
    ///     Validate text with length options
    /// </summary>
    public class HexColorValidator : ValidatorBase
    {
        private const string RegExpression = @"^\#{1}[A-Fa-f0-9]{3}([A-Fa-f0-9]{3})?$";

        #region base implementation

        protected override string Expression => RegExpression;

        #endregion

        public override void Validate()
        {
            if (_Input == null)
            {
                Succeed();
                return;
            }

            if (!(_Input is string))
            {
                Fail("is not a hex colour value, eg #FFFFFF.");
                return;
            }

            if (!SingleMatch((string) _Input))
            {
                Fail("is not a hex colour value, eg #FFFFFF.");
                return;
            }

            Succeed();
        }
    }
}