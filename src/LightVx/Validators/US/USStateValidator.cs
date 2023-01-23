namespace LightVx.Validators.US
{
    [System.AttributeUsage(System.AttributeTargets.Property)]
    public class USStateValidatorAttribute : AttributeValidator
    {
        public USStateValidatorAttribute() : base(new USStateValidator()) { }
    }
    public class USStateValidator : ValidatorBase
    {
        protected override string Expression => @"^(AA|AE|AP|AL|AK|AS|AZ|AR|CA|CO|CT|DE|DC|FM|FL|GA|GU|HI|ID|IL|IN|IA|KS|KY|LA|ME|MH|MD|MA|MI|MN|MS|MO|MT|NE|NV|NH|NJ|NM|NY|NC|ND|MP|OH|OK|OR|PW|PA|PR|RI|SC|SD|TN|TX|UT|VT|VI|VA|WA|WV|WI|WY)$";

        public override void Validate()
        {
            if (_Input == null || (string)_Input == string.Empty)
            {
                Succeed();
                return;
            }

            if (SingleMatch((string)_Input))
                Succeed();
            else
                Fail("is not a valid State.");
        }
    }
}
