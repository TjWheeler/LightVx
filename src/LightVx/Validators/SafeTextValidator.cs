namespace LightVx.Validators
{
    /// <summary>
    /// Safe Text that uses both the <see cref="XssSafeTextValidator"/> and the <see cref="SqlSafeTextValidator"/> validiators
    /// </summary>
    [System.AttributeUsage(System.AttributeTargets.Property)]
    public class SafeTextAttribute : AttributeValidator
    {
        public SafeTextAttribute() : base(new SafeTextValidator()) { }
    }
    /// <summary>
    /// Safe Text that uses both the <see cref="XssSafeTextValidator"/> and the <see cref="SqlSafeTextValidator"/> validiators
    /// </summary>
    public class SafeTextValidator : AggregatedValidator
    {
        public SafeTextValidator()
        {
            AddValidator(new XssSafeTextValidator());
            AddValidator(new SqlSafeTextValidator());
        }
        
    }
}