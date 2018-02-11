namespace LightVx.Validators
{
    /// <summary>
    ///     Validate Email Addresses
    /// </summary>
    public class EmailValidator : ValidatorBase
    {
        private const string RegExpression =
            @"^([\&\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|" +
            @"(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";

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

            if (SingleMatch((string) _Input))
                Succeed();
            else
                Fail("is not a valid email address.");
        }
    }
}