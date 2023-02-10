using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LightVx
{
    public class ValidationResult
    {
        private readonly List<ValidatorResult> _validatorResults = new List<ValidatorResult>();

        public bool IsValid
        {
            get
            {
                return ValidatorResults.All(t => t.IsValid);
            }
        }

        public List<string> ErrorMessages
        {
            get
            {
                var errors = new List<string>();
                foreach (ValidatorResult result in ValidatorResults)
                {
                    errors.AddRange(result.ErrorMessages);
                }
                return errors;
            }
        }
        public Dictionary<string, List<string>> FieldErrorMessages
        {
            get
            {
                var dictionary = new Dictionary<string, List<string>>();
                foreach (ValidatorResult result in ValidatorResults)
                {
                    if(result.IsValid) continue;
                    if (dictionary.ContainsKey(result.FieldName))
                    {
                        dictionary[result.FieldName].AddRange(result.ErrorMessages.Where(t => !string.IsNullOrEmpty(t)).ToArray());
                    }
                    else
                    {
                        dictionary.Add(result.FieldName, result.ErrorMessages.Where(t => !string.IsNullOrEmpty(t)).ToList());
                    }
                }
                return dictionary;
            }
        }

        public ValidatorResult GetFieldResult(string fieldName)
        {
            return ValidatorResults.FirstOrDefault(t => t.FieldName == fieldName);
        }
        public Dictionary<string, ValidatorResult> FieldResults { get; } = new Dictionary<string, ValidatorResult>();
        public List<ValidatorResult> ValidatorResults
        {
            get
            {
                return FieldResults.Select(t => t.Value).ToList();
            }
        }
    }

    public class ValidatorResult
    {
        public bool IsValid
        {
            get
            {
                return Validators.All(t => t.IsValid != false);
            } 
        }

        public List<string> ErrorMessages
        {
            get
            {
                return Validators.Select(t => t.ErrorMessage).ToList();
            }
        }

        public string FieldName { get; set; }
        public string FieldDisplayName { get; set; }

        public List<IValidator> FailedValidators
        {
            get
            {
                return Validators.Where(t => !t.IsValid).ToList();
            }
        }

        public List<IValidator> Validators { get; set; }
    }
}
