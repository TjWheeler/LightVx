namespace LightVx.Validators
{
    /// <summary>
    ///     Validate decimal numbers.  digits and 1 period.  Also has maxium decimal
    ///     places restriction.
    /// </summary>
    public class DecimalValidator : ValidatorBase
    {
        #region private variables and protected properties

        protected override string Expression { get; }

        #endregion

        #region Implementation

        protected override void Validate()
        {
            decimal result;
            ErrorMessage = string.Empty;
            if (_Input != null && _Input.ToString().Length > 0)
            {
                IsValid = decimal.TryParse(_Input.ToString(), out result);
                if (!IsValid) ErrorMessage = FieldName + " is not a valid decimal value";
            }
            else
            {
                Succeed();
            }
        }

        #endregion
    }
}