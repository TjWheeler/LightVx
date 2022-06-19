using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LightVx.Tests.ObjectValidator
{
    public class CustomerValidator : ObjectValidator<Customer>
    {
        public CustomerValidator(Customer customer ) : base(customer)
        {
            Eval(Input.Name, "Name")
                .IsNotEmpty()
                .HasMinLength(3)
                .HasMaxLength(120)
                .IsNameText(); //Allows alpha, hyphen, space, apostrophie

            Eval(Input.DOB, "Date of Birth")
                .IsNotNull()
                .IsBefore(DateTime.Now);
        }
    }
}
