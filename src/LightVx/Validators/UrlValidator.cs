namespace LightVx.Validators
{
    /// <summary>
    ///     Validate Url
    /// </summary>
    public class UrlValidator : ValidatorBase
    {
        private const string RegExpression =
            @"^((((https?|http?|ftps?|gopher|telnet|nntp)://)|(mailto:|news:))(%[0-9A-Fa-f]{2}|" +
            "[-()_.!~*';/?:@&=+$,A-Za-z0-9])+)([).!';/?:,]blank:)?$";

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
                Fail("is not a valid url.");
        }
    }
}