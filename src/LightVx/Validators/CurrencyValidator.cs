using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace LightVx.Validators
{
    [System.AttributeUsage(System.AttributeTargets.Property)]
    public class CurrencyAttribute : AttributeValidator
    {
        public CurrencyAttribute() : base(new CurrencyValidator()) { }
    }
    /// <summary>
    /// Validates if the input is a valid currency value based on at least 1 culture matching.
    /// If requireCurrencySymbol is set to false then only a double is validated.
    /// </summary>
    public class CurrencyValidator : ValidatorBase
    {
        private bool _requireCurrencySymbol;

        public CurrencyValidator(bool requireCurrencySymbol = true) : base()
        {
            _requireCurrencySymbol = requireCurrencySymbol;
        }
        private static readonly Lazy<Dictionary<string, List<CultureInfo>>> _allCulturesCurrencyMap = new Lazy<Dictionary<string, List<CultureInfo>>>(LoadCurrencyMap);
        public override void Validate()
        {
            if (_Input == null || _Input is string && (string)_Input == string.Empty)
            {
                Succeed();
                return;
            }
            if (IsValidCurrency(_Input.ToString()))
            {
                Succeed();
            }
            else
            {
                Fail("is not a valid currency value.");
            }
        }

        private static Dictionary<string, List<CultureInfo>> LoadCurrencyMap()
        {
            Dictionary<string, List<CultureInfo>> map = new Dictionary<string, List<CultureInfo>>();
            foreach (CultureInfo cultureInfo in CultureInfo.GetCultures(CultureTypes.AllCultures))
            {
                List<CultureInfo> cultures;
                if (!map.TryGetValue(cultureInfo.NumberFormat.CurrencySymbol, out var value))
                {
                    cultures = new List<CultureInfo>();
                    map.Add(cultureInfo.NumberFormat.CurrencySymbol, cultures);
                }
                else
                {
                    cultures = value;
                }
                cultures.Add(cultureInfo);
            }
            return map;
        }
        bool IsValidCurrency(string input)
        {
            string symbol = ExtractCurrencySymbol(input);
            if (string.IsNullOrEmpty(symbol) && _requireCurrencySymbol)
            {
                return false;
            }
            if (string.IsNullOrEmpty(symbol))
            {
                return double.TryParse(_Input.ToString(), out var _);
            }
            _allCulturesCurrencyMap.Value.TryGetValue(symbol, out var cultures);

            foreach (var culture in cultures ?? new List<CultureInfo>())
            {
                if (double.TryParse(input, NumberStyles.Currency, culture, out _))
                {
                    return true; // Valid in one mapped culture
                }
            }

            return false;
        }

       
        static string ExtractCurrencySymbol(string input)
        {
            var match = Regex.Match(input, @"^[^\d]+"); // Extract leading non-numeric characters
            return match.Success ? match.Value.Trim() : string.Empty;
        }
    }
}
