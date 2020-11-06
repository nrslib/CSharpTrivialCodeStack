using System;
using System.Collections.Concurrent;
using System.Reflection;

namespace Nrslib.Enums.EnumTexts
{
    public static class EnumTextExtensions
    {
        private static readonly Type enumTextProviderTemplate = typeof(EnumTextProvider<>);

        private static ConcurrentDictionary<Type, object> typeToEnumTextProvider = new ConcurrentDictionary<Type, object>();
        private static ConcurrentDictionary<Type, MethodInfo> typeToToTextMethod = new ConcurrentDictionary<Type, MethodInfo>();
        
        public static string ToText(this Enum value)
        {
            var argType = value.GetType();

            var provider = typeToEnumTextProvider.GetOrAdd(argType, t =>
            {
                var genericType = enumTextProviderTemplate.MakeGenericType(t);
                var constructor = genericType.GetConstructor(Type.EmptyTypes);
                var instance = constructor.Invoke(null);

                return instance;
            });

            var method = typeToToTextMethod.GetOrAdd(argType, t => provider.GetType().GetMethod("ToText"));
            var result = method.Invoke(provider, new object[] {value});

            return (string) result;
        }
    }
}
