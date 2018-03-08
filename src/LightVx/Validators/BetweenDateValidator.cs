using System;
using System.Collections.Generic;
using System.Text;

namespace LightVx.Validators
{
    public class BetweenDateValidator : AggregatedValidator
    {
        public BetweenDateValidator(DateTime minDate, DateTime maxDate)
        {
            AddValidator(new MinDateValidator(minDate));
            AddValidator(new MaxDateValidator(maxDate));
        }
    }
}