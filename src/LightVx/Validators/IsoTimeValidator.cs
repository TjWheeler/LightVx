namespace LightVx.Validators
{
    /// <summary>
    ///     Validate input is matches a Iso 8601 string format for the time component only.
    ///     A full Iso 8601 string will fail.
    /// </summary>
    [System.AttributeUsage(System.AttributeTargets.Property)]
    public class IsoTimeAttribute : AttributeValidator
    {
        public IsoTimeAttribute() : base(new IsoTimeValidator()) { }
    }
    /// <summary>
    ///     Validate input is matches a Iso 8601 string format for the time component only.
    ///     A full Iso 8601 string will fail. 
    /// </summary>
    public class IsoTimeValidator : ValidatorBase
    {
        private const string RegExpression = @"^(?:[01]\d|2[0-3]):[0-5]\d:[0-5]\d(?:\.\d+)?(?:Z|[\+\-][01]\d:[0-5]\d)?$";

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

            if (!SingleMatch((string) _Input))
            {
                Fail("is not an Iso8601 Time value, eg '23:59:59Z' or '15:30:45.123+02:00'.");
                return;
            }

            Succeed();
        }
    }
}