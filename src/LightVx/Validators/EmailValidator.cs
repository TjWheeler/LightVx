namespace LightVx.Validators
{
    /// <summary>
    ///     Validate Email Addresses
    /// </summary>
    [System.AttributeUsage(System.AttributeTargets.Property)]
    public class EmailValidatorAttribute : AttributeValidator
    {
        public EmailValidatorAttribute() : base(new EmailValidator()) { }
    }
    /// <summary>
    ///     Validate Email Addresses
    /// </summary>
    public class EmailValidator : ValidatorBase
    {
        protected override string Expression => "^[A-Za-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\\.[A-Za-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[A-Za-z0-9](?:[A-Za-z0-9-]*[A-Za-z0-9])?\\.)+[A-Za-z0-9](?:[A-Za-z0-9-]*[A-Za-z0-9])?$";
        public override void Validate()
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