namespace LightVx.Validators
{
    /// <summary>
    /// Blacklist approach to detects use of SQL Injection attack characters.
    /// Prevents the use of '#;= and hex equivalents.
    /// Warning: You should not rely on this, ensure other defence factors are in use to protect your system, such as parameterized queries.
    /// </summary>
    [System.AttributeUsage(System.AttributeTargets.Property)]
    public class SqlSafeTextValidatorAttribute : AttributeValidator
    {
        public SqlSafeTextValidatorAttribute() : base(new SqlSafeTextValidator()) { }
    }

    /// <summary>
    /// Blacklist approach to detects use of SQL Injection attack characters.
    /// Prevents the use of '#;= and hex equivalents.
    /// Warning: You should not rely on this, ensure other defence factors are in use to protect your system, such as parameterized queries.
    /// </summary>
    public class SqlSafeTextValidator : ValidatorBase
    {
        public override void Validate()
        {
            if (_Input == null || (string)_Input == string.Empty)
            {
                Succeed();
                return;
            }
            bool msSqlAttack = HasMatch((string)_Input, @"(?i)exec(\s|\+)+(s|x)p\w+");
            bool hasMetachars = HasMatch((string)_Input, @"(?i)(\%27)|(\')|(\-\-)|(\%23)|(\#)|(\%3B)|(;)|(=)"); 
            if (msSqlAttack || hasMetachars)
                Fail("contains invalid characters.");
            else
                Succeed();
        }
    }
}
