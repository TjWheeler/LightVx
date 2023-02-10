using System;
using System.Collections.Generic;
using System.Text;

namespace LightVx
{
    public class DisplayNameAttribute : System.Attribute
    {
        private string _displayName;

        public DisplayNameAttribute(string displayName)
        {
            _displayName = displayName;
        }
        public string DisplayName
        {
            get { return _displayName; }
        }
    }
}
