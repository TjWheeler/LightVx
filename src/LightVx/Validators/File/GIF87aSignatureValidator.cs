using System;
using System.Collections.Generic;
using System.Text;

namespace LightVx.Validators.File
{
    public class Gif87aSignatureValidator :  SignatureValidator
    {
        public Gif87aSignatureValidator() : base("GIF", new byte[] { 0x47, 0x49, 0x46, 0x38, 0x37, 0x61 })
        {
        }
    }
}
