using System;
using System.Collections.Generic;
using System.Text;

namespace LightVx.Validators.File
{
    public class PngSignatureValidator : SignatureValidator
    {
        public PngSignatureValidator() : base("PNG", new byte[] { 0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A })
        {
           
        }

    }
}