using System;

namespace LightVx.Validators
{
    /// <summary>
    ///     Validate Australian Business Numbers
    /// </summary>
    [System.AttributeUsage(System.AttributeTargets.Property)]
    public class AbnAttribute : AttributeValidator
    {
        public AbnAttribute(): base(new AbnValidator()) { }
    }
    /// <summary>
    ///     Validate Australian Business Numbers
    /// </summary>
    public class AbnValidator : AggregatedValidator
    {
        public AbnValidator()
        {
            AddValidator(new NumericValidator());
            AddValidator(new LengthValidator(11, 11));
            AddValidator(new AbnCheckSumValidator());
        }
        private class AbnCheckSumValidator : ValidatorBase
        {
            #region base implementation

            protected override string Expression => string.Empty;

            #endregion

            public override void Validate()
            {
                if (_Input == null || _Input is string && (string)_Input == string.Empty)
                {
                    Succeed();
                    return;
                }

                if (IsValidAbnCheckSum())
                    Succeed();
                else
                    Fail("has failed the ABN Checksum Validation.");
            }

            private bool IsValidAbnCheckSum()
            {
                try
                {
                    //1 Subtract 1 from the first (left) digit to give a new eleven digit number 
                    //2 Multiply each of the digits in this new number by its weighting factor 
                    //3 Sum the resulting 11 products 
                    //4 Divide the total by 89, noting the remainder 
                    //5 If the remainder is zero the number is valid
                    var abnString = _Input.ToString();
                    //1  Subtract 1 from the first (left) digit to give a new eleven digit number 
                    abnString = ReduceFirstDigit(abnString);
                    //2 and 3 - Multiply each of the digits in this new number by its weighting factor 
                    //10,1,3,5,7,9,11,13,15,17,19
                    int runningTotal;
                    if (!SumWeightings(abnString, out runningTotal)) return false;
                    //4 Divide by 89 - if remainder is 0 then its a valid ABN
                    return decimal.Remainder(runningTotal, 89) == 0;
                }
                catch (Exception)
                {
                    return false;
                }
            }

            private static bool SumWeightings(string abnString, out int runningTotal)
            {
                runningTotal = 0;
                for (var count = 0; count <= 10; count++)
                {
                    var currentDigit = int.Parse(abnString.Substring(count, 1));
                    switch (count + 1)
                    {
                        case 1:
                            runningTotal = currentDigit * 10;
                            break;
                        case 2:
                            runningTotal += currentDigit * 1;
                            break;
                        case 3:
                            runningTotal += currentDigit * 3;
                            break;
                        case 4:
                            runningTotal += currentDigit * 5;
                            break;
                        case 5:
                            runningTotal += currentDigit * 7;
                            break;
                        case 6:
                            runningTotal += currentDigit * 9;
                            break;
                        case 7:
                            runningTotal += currentDigit * 11;
                            break;
                        case 8:
                            runningTotal += currentDigit * 13;
                            break;
                        case 9:
                            runningTotal += currentDigit * 15;
                            break;
                        case 10:
                            runningTotal += currentDigit * 17;
                            break;
                        case 11:
                            runningTotal += currentDigit * 19;
                            break;
                        default:
                            return false;
                    }
                }

                return true;
            }

            private static string ReduceFirstDigit(string abnString)
            {
                var firstDigit = int.Parse(abnString.Substring(0, 1));
                if (firstDigit == 0)
                    firstDigit = 9;
                else
                    firstDigit = firstDigit - 1;
                abnString = firstDigit + abnString.Substring(1);
                return abnString;
            }
        }
    }
}