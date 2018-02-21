namespace LightVx.Validators.US
{
    public static class USFluientExtensions
    {
        public static ValidatorFluent IsUSState(this ValidatorFluent fluentApi)
        {
            fluentApi.AddValidator(new USStateValidator());
            return fluentApi;
        }
    }
}
