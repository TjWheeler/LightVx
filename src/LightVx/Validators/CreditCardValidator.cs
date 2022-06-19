using System;

namespace LightVx.Validators
{
    [Obsolete("This validator is not working")]
    /// <summary>
    /// Validate Credit Card
    /// Note: This is not tested yet
    /// Validates null or empty string as Successful.  
    /// </summary>
    public class CreditCardValidator : ValidatorBase
    {
        private const string RegExpression =
            @"^((4\d{3})|(5[1-5]\d{2})|(6011)|(7\d{3}))-?\d{4}-?\d{4}-?\d{4}|3[4,7]\d{13}$";

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

            if (SingleMatch(_Input.ToString()))
                Succeed();
            else
                Fail("is not a valid credit card number.");
        }
    }
}