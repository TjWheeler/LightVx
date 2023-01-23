using System;
using System.Collections.Generic;
using System.Text;

namespace LightVx.Attribute
{
    [System.AttributeUsage(System.AttributeTargets.Property)]
    public abstract class DateAttributeValidator : System.Attribute, IAttributeValidator
    {
        protected IValidator _validator;
        public DateAttributeValidator()
        {
        }

        public IValidator Validator
        {
            get
            {
                return _validator;
            }
            protected set
            {
                _validator = value;
            }
        }
        protected static DateTime CalculateDateOffset(DateTypeEnum dateType, DateOffsetEnum offsetType, int offset)
        {
            DateTime date;
            switch (dateType)
            {
                case DateTypeEnum.Now:
                    date = DateTime.Now;
                    break;
                case DateTypeEnum.UtcNow:
                    date = DateTime.UtcNow;
                    break;
                case DateTypeEnum.Today:
                    date = DateTime.Today;
                    break;
                default:
                    throw new NotImplementedException();
            }

            switch (offsetType)
            {
                case DateOffsetEnum.PlusSeconds:
                    date = date.AddSeconds(offset);
                    break;
                case DateOffsetEnum.PlusMinutes:
                    date = date.AddMinutes(offset);
                    break;
                case DateOffsetEnum.PlusHours:
                    date = date.AddHours(offset);
                    break;
                case DateOffsetEnum.PlusDays:
                    date = date.AddDays(offset);
                    break;
                case DateOffsetEnum.PlusMonths:
                    date = date.AddMonths(offset);
                    break;
                case DateOffsetEnum.PlusYears:
                    date = date.AddYears(offset);
                    break;
            }

            return date;
        }
    }
}
