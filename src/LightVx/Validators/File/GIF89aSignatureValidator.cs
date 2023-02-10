using System;
using System.Collections.Generic;
using System.Text;

namespace LightVx.Validators.File
{
    public class Gif89aSignatureValidator :  SignatureValidator
    {
        public Gif89aSignatureValidator() : base("GIF", new byte[] { 0x47, 0x49, 0x46, 0x38, 0x39, 0x61 })
        {
        }
    }
}
