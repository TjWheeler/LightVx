using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Text;

namespace LightVx.Validators
{
    /// <summary>
    /// Ensures a date value is within valid SQL range
    /// </summary>
    [System.AttributeUsage(System.AttributeTargets.Property)]
    public class SqlSafeDateValidatorAttribute : AttributeValidator
    {
        public SqlSafeDateValidatorAttribute() : base(new SqlSafeDateValidator()) { }
    }
    /// <summary>
    /// Ensures a date value is within valid SQL range
    /// </summary>
    public class SqlSafeDateValidator : AggregatedValidator
    {
        public SqlSafeDateValidator()
        {
            AddValidator(new MinDateValidator(SqlDateTime.MinValue.Value));
            AddValidator(new MaxDateValidator(SqlDateTime.MaxValue.Value));
        }
    }
}
