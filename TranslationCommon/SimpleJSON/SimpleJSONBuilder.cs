using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace TranslationCommon.SimpleJSON
{
    public class SimpleJSONBuilder
    {
        public T Deserialize<T>(string serializationStream)
            where T : new()
        {
            var jsonNode = JSON.Parse(serializationStream);
            var settings = new SimpleJSONParserSettings();
            return (T) SimpleJSONStaticParser.FromJsonNode(jsonNode, typeof(T), settings);
        }

        public string Serialize(object graph, bool isIndented)
        {
            var settings = new SimpleJSONParserSettings
            {
                IsIndented = isIndented
            };
            var jsonNode = SimpleJSONStaticParser.ToJsonNode(graph, settings);

            var sb = new StringBuilder();
            jsonNode.WriteToStringBuilder(sb, 0, settings.IsIndented ? 2 : 0,
                settings.IsIndented ? JSONTextMode.Indent : JSONTextMode.Compact);
            return sb.ToString();
        }
    }

    public struct SimpleJSONParserSettings
    {
        public static SimpleJSONParserSettings Default()
        {
            return new SimpleJSONParserSettings
            {
                IsIndented = true
            };
        }

        public bool IsIndented;
    }

    public static class SimpleJSONStaticParser
    {
        public static Type[] AppDomainTypes;

        public static object FromJsonNode(JSONNode value, Type type, SimpleJSONParserSettings settings)
        {
            if (value == null)
            {
                return null;
            }

            if (type == typeof(string))
            {
                return (string) value;
            }

            if (type == typeof(char))
            {
                return (char) value;
            }

            if (type == typeof(bool))
            {
                return (bool) value;
            }

            if (typeof(IList).IsAssignableFrom(type))
            {
                var listType = typeof(List<>);
                var elementType = type.GetGenericArguments();
                var constructedListType = listType.MakeGenericType(elementType);
                var list = (IList) Activator.CreateInstance(constructedListType);
                foreach (var array in value.AsArray.Children)
                {
                    list.Add(FromJsonNode(array, elementType[0], settings));
                }

                return list;
            }

            if (typeof(IDictionary).IsAssignableFrom(type))
            {
                var dictType = typeof(Dictionary<,>);
                var elementType = type.GetGenericArguments();
                var constructedListType = dictType.MakeGenericType(elementType);
                var dictionary = (IDictionary) Activator.CreateInstance(constructedListType);
                foreach (var dict in value)
                {
                    dictionary.Add(
                        FromJsonNode(dict.Key, elementType[0], settings),
                        FromJsonNode(dict.Value, elementType[1], settings)
                    );
                }

                return dictionary;
            }

            if (type == typeof(Vector2))
            {
                return (Vector2) value;
            }

            if (type == typeof(Vector3))
            {
                return (Vector3) value;
            }

            if (type == typeof(Vector4))
            {
                return (Vector4) value;
            }

            if (type == typeof(Quaternion))
            {
                return (Quaternion) value;
            }

            if (type == typeof(Rect))
            {
                return (Rect) value;
            }

            if (type == typeof(RectOffset))
            {
                return (RectOffset) value;
            }

            if (type == typeof(Matrix4x4))
            {
                return (Matrix4x4) value;
            }

            if (type == typeof(Color))
            {
                return (Color) value;
            }

            if (type == typeof(Color32))
            {
                return (Color32) value;
            }

            if (type == typeof(byte))
            {
                return (byte) value;
            }

            if (type == typeof(sbyte))
            {
                return (sbyte) value;
            }

            if (type == typeof(int))
            {
                return (int) value;
            }

            if (type == typeof(uint))
            {
                return (uint) value;
            }

            if (type == typeof(short))
            {
                return (short) value;
            }

            if (type == typeof(ushort))
            {
                return (ushort) value;
            }

            if (type == typeof(char))
            {
                return (char) value;
            }

            if (type == typeof(float))
            {
                return (float) value;
            }

            if (type == typeof(double))
            {
                return (double) value;
            }

            if (type == typeof(decimal))
            {
                return (decimal) value;
            }

            if (type == typeof(long))
            {
                return (long) value;
            }

            if (type == typeof(ulong))
            {
                return (ulong) value;
            }

            var obj = CreateMatchingType(value, type, out var resultType);
            var fields = resultType.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            foreach (var fieldInfo in fields)
            {
                if (fieldInfo.IsPublic || fieldInfo.ContainsAttribute<SerializeField>())
                {
                    if (fieldInfo.ContainsAttribute<SerializeFirstAsObjectAttribute>())
                    {
                        fieldInfo.SetValue(obj, FromJsonNode(value, fieldInfo.FieldType, settings));
                        return obj;
                    }

                    fieldInfo.SetValue(obj, FromJsonNode(value[fieldInfo.Name], fieldInfo.FieldType, settings));
                }
            }

            return obj;
        }

        private static object CreateMatchingType(JSONNode value, Type type, out Type resultType)
        {
            if (type.IsInterface)
            {
                bool ContainsAllItems<T>(IEnumerable<T> a, IEnumerable<T> b)
                {
                    return !b.Except(a).Any();
                }

                if (AppDomainTypes == null)
                {
                    var assemblies = AppDomain.CurrentDomain.GetAssemblies();
                    var matchingTypes = new List<Type>();
                    foreach (var assembly in assemblies)
                    {
                        try
                        {
                            matchingTypes.AddRange(assembly.GetTypes()
                                .Where(type.IsAssignableFrom)
                                .Where(t => !t.IsInterface && !t.IsAbstract));
                        }
                        catch //(ReflectionTypeLoadException e)
                        {
                            /*ConsoleLogger.LogDebug(assembly.FullName + " " + e.Message + "\n" + e.StackTrace + "\n" + 
                            e.LoaderExceptions.Select(exception => exception.Message).Aggregate((x,y) => x + " " + y));*/
                        }
                    }
                    AppDomainTypes = matchingTypes.ToArray();
                }
                
                foreach (var matchingType in AppDomainTypes)
                {
                    var fields =
                        matchingType.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                    if (ContainsAllItems(fields.Select(info => info.Name), value.GetEnumerator().ToKeyEnumerable()))
                    {
                        resultType = matchingType;
                        return Activator.CreateInstance(matchingType);
                    }
                }

                throw new KeyNotFoundException(
                    $"No matching member was not found with signature {value.GetEnumerator().ToKeyEnumerable().Aggregate((x, y) => x + " " + y)}");
            }

            resultType = type;
            return Activator.CreateInstance(type);
        }

        public static JSONNode ToJsonNode(object value, SimpleJSONParserSettings settings)
        {
            switch (value)
            {
                case null:
                    return JSONNull.CreateOrGet();
                case JSONNode jsonValue:
                    return jsonValue;
                case string strValue:
                    return new JSONString(strValue);
                case char charValue:
                    return new JSONString(new string(charValue, 1));
                case bool boolValue:
                    return new JSONBool(boolValue);
                case IList listValue:
                    return ToJsonNode(listValue, settings);
                case IDictionary dictValue:
                    return ToJsonNode(dictValue, settings);

                case Vector2 v2Value:
                    return v2Value;
                case Vector3 v3Value:
                    return v3Value;
                case Vector4 v4Value:
                    return v4Value;
                case Quaternion quatValue:
                    return quatValue;
                case Rect rectValue:
                    return rectValue;
                case RectOffset rectOffsetValue:
                    return rectOffsetValue;
                case Matrix4x4 matrixValue:
                    return matrixValue;
                case Color colorValue:
                    return colorValue;
                case Color32 color32Value:
                    return color32Value;

                case float floatValue:
                    return new JSONNumber(floatValue.ToString("R", CultureInfo.InvariantCulture));
                default:
                    if (JSONNumber.IsNumeric(value))
                    {
                        return new JSONNumber(Convert.ToDouble(value));
                    }
                    else
                    {
                        var jsonObject = new JSONObject();
                        var fields = value.GetType()
                            .GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                        foreach (var fieldInfo in fields)
                        {
                            if (fieldInfo.IsPublic || fieldInfo.ContainsAttribute<SerializeField>())
                            {
                                var fieldValue = fieldInfo.GetValue(value);
                                if (fieldValue == default)
                                {
                                    continue;
                                }

                                var jsonNode = ToJsonNode(fieldValue, settings);
                                if (fieldInfo.ContainsAttribute<SerializeFirstAsObjectAttribute>())
                                {
                                    return jsonNode;
                                }

                                jsonObject.Add(fieldInfo.Name, jsonNode);
                            }
                        }

                        return jsonObject;
                    }
            }
        }

        private static JSONArray ToJsonNode(IList list, SimpleJSONParserSettings settings)
        {
            var jsonArray = new JSONArray();

            for (var i = 0; i < list.Count; i++)
            {
                jsonArray.Add(ToJsonNode(list[i], settings));
            }

            return jsonArray;
        }

        private static JSONObject ToJsonNode(IDictionary dict, SimpleJSONParserSettings settings)
        {
            var jsonObject = new JSONObject();

            foreach (var key in dict.Keys)
            {
                jsonObject.Add(key.ToString(), ToJsonNode(dict[key], settings));
            }

            return jsonObject;
        }
    }
}