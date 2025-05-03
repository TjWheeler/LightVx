namespace LightVx.Validators
{
    /// <summary>
    ///     Validate the input is a string, matches the Iso 8601 format and can be converted to a DateTime.
    /// </summary>
    [System.AttributeUsage(System.AttributeTargets.Property)]
    public class IsoDateTimeAttribute : AttributeValidator
    {
        public IsoDateTimeAttribute() : base(new IsoDateTimeValidator()) { }
    }

    /// <summary>
    ///     Validate the input is a string, matches the Iso 8601 format and can be converted to a DateTime.
    /// </summary>
    public class IsoDateTimeValidator : AggregatedValidator
    {
        public IsoDateTimeValidator()
        {
            AddValidator(new IsoDateTimeFormatValidator());
            AddValidator(new DateTimeValidator());
        }

        private class IsoDateTimeFormatValidator : ValidatorBase
        {
            private const string RegExpression = @"^(?:\d{4})-(?:0[1-9]|1[0-2])-(?:0[1-9]|[12]\d|3[01])T(?:[01]\d|2[0-3]):[0-5]\d:[0-5]\d(?:\.\d+)?(?:Z|[\+\-][01]\d:[0-5]\d)?$";

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
                    Fail("is not an Iso8601 DateTime value, eg '2025-05-03T14:30:15Z' or '1999-12-31T23:59:59.999+02:00'.");
                    return;
                }

                if (!SingleMatch((string)_Input))
                {
                    Fail("is not an Iso8601 DateTime value, eg '2025-05-03T14:30:15Z' or '1999-12-31T23:59:59.999+02:00'.");
                    return;
                }

                Succeed();
            }
        }
    }
  
}