using System;

namespace Breeze
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class Editable : Attribute
    {
        public string Title;

        public Editable()
        {
        }

        public Editable(string title)
        {
            Title = title;
        }
    }
}

