namespace LightVx
{
    public interface IValidator
    {
        string FieldName { get; set; }
        string FieldDisplayName { get; set; }

        /// <summary>
        ///     Accepts any object as input to be validated
        /// </summary>
        object Input { get; set; }

        /// <summary>
        ///     Indicates if the object is valid.  Must be called after the Validate()
        /// </summary>
        bool IsValid { get; set; }

        /// <summary>
        ///     Error message containing information about why validation has failed.
        /// </summary>
        string ErrorMessage { get; set; }

        /// <summary>
        ///     Performs validation and checks the result. If validation fails it outputs the error
        ///     message.
        /// </summary>
        /// <param name="input">Object to validate</param>
        /// <param name="fieldName">
        ///     Name of the field, used to generate an error that can
        ///     be shown to users
        /// </param>
        /// <param name="errorMessage">Empty string if no error, error message if failure</param>
        /// <returns>True if the input object is valid, false if a validation rule was broken</returns>
        bool Validate(object input, string fieldName, out string errorMessage);
        bool Validate(object input, string fieldName, string fieldDisplayName);
        bool Validate(object input, string fieldName);

        bool Validate(object input);
    }
}