using System;

namespace Nrslib.Enums.EnumTexts
{
    [AttributeUsage(AttributeTargets.Field)]
    public class EnumTextAttribute : Attribute
    {
        public EnumTextAttribute(string text)
        {
            Text = text;
        }

        public string Text { get; }
    }
}
