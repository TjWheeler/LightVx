using System;
using System.Collections.Generic;
using System.Text;
using LightVx.Attribute;

namespace LightVx.Validators
{
    [System.AttributeUsage(System.AttributeTargets.Property)]
    public class BetweenDateAttribute : DateAttributeValidator
    {
        public BetweenDateAttribute(DateTypeEnum dateType, DateOffsetEnum minDateOffsetType, int minDateOffset, DateOffsetEnum maxDateOffsetType, int maxDateOffset)
        {
            DateTime date1 = CalculateDateOffset(dateType, minDateOffsetType, minDateOffset);
            DateTime date2 = CalculateDateOffset(dateType, maxDateOffsetType, maxDateOffset);
            Validator = new BetweenDateValidator(date1, date2);
        }
    }

    public class BetweenDateValidator : AggregatedValidator
    {
        public BetweenDateValidator(DateTime minDate, DateTime maxDate)
        {
            AddValidator(new MinDateValidator(minDate));
            AddValidator(new MaxDateValidator(maxDate));
        }
    }
}