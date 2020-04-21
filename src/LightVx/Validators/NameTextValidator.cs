using System;
using System.Collections.Generic;
using System.Text;

namespace LightVx.Validators
{
    
    /// <summary>
    ///     Validates name type text. Allows alpha, spaces, hyphen and apostrophe
    /// </summary>
    public class NameTextValidator : AlphaTextValidator
    {
        private const string RegExpression = "^([a-zA-Z\\s\\-']{1,})$";

        #region base implementation

        protected override string Expression => RegExpression;

        #endregion
    }
}
