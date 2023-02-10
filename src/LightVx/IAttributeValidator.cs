using System;
using System.Collections.Generic;
using System.Text;

namespace LightVx
{
    public interface IAttributeValidator
    {
        IValidator Validator { get; }
    }
}
