using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using SimpleJSON;
using UnityEngine;

namespace TranslationCommon
{
    public class SimpleJSONBuilder
    {
        public T Deserialize<T>(string serializationStream)
            where T : new()
        {
            var jsonNode = JSON.Parse(serializationStream);
            var value = new T();
            var fields = typeof(T).GetFields(BindingFlags.Instance | BindingFlags.Public);
            var settings = new SimpleJSONParserSettings()
            {
            };
            foreach (var fieldInfo in fields)
            {
                fieldInfo.SetValue(value, SimpleJSONStaticParser.FromJsonNode(jsonNode[fieldInfo.Name], fieldInfo.FieldType, settings));
            }
            return value;
        }

        public string Serialize(object graph, bool isIndented)
        {
            var settings = new SimpleJSONParserSettings()
            {
                IsIndented = isIndented
            };
            var jsonNode = SimpleJSONStaticParser.ToJsonNode(graph, settings);

            var sb = new StringBuilder();
            jsonNode.WriteToStringBuilder(sb, 0, settings.IsIndented ? 2 : 0, settings.IsIndented ? JSONTextMode.Indent : JSONTextMode.Compact);
            return sb.ToString();
        }
    }

    public struct SimpleJSONParserSettings
    {
        public static SimpleJSONParserSettings Default()
        {
            return new SimpleJSONParserSettings()
            {
                IsIndented = true,
            };
        }

        public bool IsIndented;
    }

    public static class SimpleJSONStaticParser
    {
        public static object FromJsonNodeImplicitSlow(JSONNode node, Type type)
        {
            var nodeType = typeof(JSONNode);
            
            var converter = nodeType.GetMethods(BindingFlags.Static | BindingFlags.Public)
                .FirstOrDefault(info => info.Name == "op_Implicit" && info.ReturnType == type);
            if (converter != null)
            {
                var result = converter.Invoke(null, new[] {(JSONNode)node});
                return result;
            }
            
            return node;
        }
        
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
                var list = (IList)Activator.CreateInstance(constructedListType);
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
                var dictionary = (IDictionary)Activator.CreateInstance(constructedListType);
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
            else
            {
                var obj = Activator.CreateInstance(type);
                var fields = type.GetFields(BindingFlags.Instance | BindingFlags.Public);
                foreach (var fieldInfo in fields)
                {
                    fieldInfo.SetValue(obj, FromJsonNode(value[fieldInfo.Name], fieldInfo.FieldType, settings));
                }

                return obj;
            }
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
                        return new JSONNumber(System.Convert.ToDouble(value));
                    }
                    else
                    {
                        var jsonObject = new JSONObject();
                        var fields = value.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public);
                        foreach (var fieldInfo in fields)
                        {
                            var fieldValue = fieldInfo.GetValue(value);
                            var jsonNode = ToJsonNode(fieldValue, settings);
                            jsonObject.Add(fieldInfo.Name, jsonNode);
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