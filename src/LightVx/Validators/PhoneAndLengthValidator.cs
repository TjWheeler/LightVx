namespace LightVx.Validators
{
    public class PhoneAndLengthValidator : AggregatedValidator
    {
        public PhoneAndLengthValidator(int minOccurance, int maxOccurance)
        {
            AddValidator(new LengthValidator(minOccurance, maxOccurance));
            AddValidator(new PhoneNumberValidator());
        }
    }
}