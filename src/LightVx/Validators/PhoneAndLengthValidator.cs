namespace LightVx.Validators
{
    [System.AttributeUsage(System.AttributeTargets.Property)]
    public class PhoneAndLengthAttribute : AttributeValidator
    {
        public PhoneAndLengthAttribute(int minOccurance, int maxOccurance) : base(new PhoneAndLengthValidator(minOccurance, maxOccurance)) { }
    }
    public class PhoneAndLengthValidator : AggregatedValidator
    {
        public PhoneAndLengthValidator(int minOccurance, int maxOccurance)
        {
            AddValidator(new LengthValidator(minOccurance, maxOccurance));
            AddValidator(new PhoneNumberValidator());
        }
    }
}