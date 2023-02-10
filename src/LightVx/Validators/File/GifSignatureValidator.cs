using System;
using System.Collections.Generic;
using System.Text;

namespace LightVx.Validators.File
{
    public class GifSignatureValidator : AggregatedValidator
    {
        public GifSignatureValidator() 
        {
           
        }

        public override void Validate()
        {
            IValidator validator = new Gif87aSignatureValidator();
            if (validator.Validate(this.Input))
            {
                Succeed();
                return;
            }
            validator = new Gif89aSignatureValidator();
            if (validator.Validate(this.Input))
            {
                Succeed();
                return;
            }
            Fail("is not a valid GIF");
        }
    }
}