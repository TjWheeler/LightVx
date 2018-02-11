namespace LightVx.Validators
{
    /// <summary>
    ///     Validate numeric numbers
    /// </summary>
    public class NumericValidator : ValidatorBase
    {
        private const string RegExpression = @"^\d+$";

        #region base implementation

        protected override string Expression => RegExpression;

        #endregion

        protected override void Validate()
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