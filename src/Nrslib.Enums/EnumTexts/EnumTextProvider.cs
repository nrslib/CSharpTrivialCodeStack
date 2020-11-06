using System.Collections.Generic;
using System.Linq;

namespace Nrslib.Enums.EnumTexts
{
    public class EnumTextProvider<T>
    {
        private readonly Dictionary<T, EnumTextAttribute> typeToEnumTextAttribute;

        public EnumTextProvider()
        {
            var type = typeof(T);
            var lookup = type.GetFields()
                .Where(fi => fi.FieldType == type)
                .SelectMany(fi => fi.GetCustomAttributes(false),
                    (fi, Attribute) => new { code = (T)fi.GetValue(null), Attribute })
                .ToLookup(a => a.Attribute.GetType());

            typeToEnumTextAttribute = lookup[typeof(EnumTextAttribute)].ToDictionary(p => p.code, p => (EnumTextAttribute)p.Attribute);
        }

        public string ToText(T key)
        {
            if (typeToEnumTextAttribute.TryGetValue(key, out var attribute))
            {
                return attribute.Text;
            }
            else
            {
                return key.ToString();
            }
        }
    }
}
