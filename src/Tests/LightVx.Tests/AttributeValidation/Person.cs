using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LightVx.Attribute;
using LightVx.Validators;

namespace LightVx.Tests.AttributeValidation
{
    public class Person
    {
        public string Id { get; set; }
        [MaxLength(10), NameText, Required]
        public string FirstName { get; set; }
        [MaxLength(15), NameText]
        public string LastName { get; set; }
        [MaxDate(DateTypeEnum.Now, DateOffsetEnum.PlusDays, -1)]
        public DateTime DOB { get; set; }

        [BetweenDate(DateTypeEnum.Now, DateOffsetEnum.PlusDays, -5, DateOffsetEnum.PlusDays, 5)]
        public DateTime ActivityDate { get; set; }

    }
}
