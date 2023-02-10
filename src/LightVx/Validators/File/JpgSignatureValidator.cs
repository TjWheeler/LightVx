using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace LightVx.Validators.File
{
    public class JpgSignatureValidator : SignatureValidator
    {
        public JpgSignatureValidator() : base("JPG", new byte[] { 0xff, 0xd8, 0xff, 0xe0 },new byte[] { 0xff, 0xd9 } )
        {
        }
    }
}
