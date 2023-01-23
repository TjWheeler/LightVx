using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LightVx.Validators;

namespace LightVx.Tests.AttributeValidation
{
    public class Person
    {
        public string Id { get; set; }
        [MaxLengthValidator(10), NameTextValidator, RequiredValidator]
        public string FirstName { get; set; }
        [MaxLengthValidator(15), NameTextValidator]
        public string LastName { get; set; }
    }
}
