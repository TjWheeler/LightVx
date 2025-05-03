namespace LightVx.Validators
{
    /// <summary>
    ///     Validate the input is a string, matches the Iso 8601 date format using the date component only.
    ///     A full Iso 8601 string will fail.
    /// </summary>
    [System.AttributeUsage(System.AttributeTargets.Property)]
    public class IsoDateAttribute : AttributeValidator
    {
        public IsoDateAttribute() : base(new IsoDateValidator()) { }
    }

    /// <summary>
    ///     Validate the input is a string, matches the Iso 8601 date format using the date component only.
    ///     A full Iso 8601 string will fail.
    /// </summary>
    public class IsoDateValidator : AggregatedValidator
    {
        public IsoDateValidator()
        {
            AddValidator(new IsoDateFormatValidator());
            AddValidator(new DateTimeValidator());
        }

        private class IsoDateFormatValidator : ValidatorBase
        {
            private const string RegExpression = @"^(?:\d{4})-(?:0[1-9]|1[0-2])-(?:0[1-9]|[12]\d|3[01])$";

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
                    Fail("is not an Iso8601 Time value, eg '23:59:59Z' or '15:30:45.123+02:00'.");
                    return;
                }

                if (!SingleMatch((string)_Input))
                {
                    Fail("is not an Iso8601 Time value, eg '23:59:59Z' or '15:30:45.123+02:00'.");
                    return;
                }

                Succeed();
            }
        }
    }
  
}