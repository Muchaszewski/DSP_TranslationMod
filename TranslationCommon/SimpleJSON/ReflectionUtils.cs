using System;
using System.Linq;
using System.Reflection;

namespace TranslationCommon.SimpleJSON
{
    public static class ReflectionUtils
    {
        public static bool ContainsAttribute<T>(this MemberInfo memberInfo)
            where T : Attribute
        {
            var customAttributes = memberInfo.GetCustomAttributes(true);
            return customAttributes.FirstOrDefault(o => o.GetType().IsAssignableFrom(typeof(T))) != null;
        }
    }
}