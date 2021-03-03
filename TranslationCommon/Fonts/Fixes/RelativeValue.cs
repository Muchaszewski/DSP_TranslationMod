using System;
using System.Linq;
using UnityEngine;

namespace TranslationCommon.Fonts
{
    public static class RelativeValue
    {
        public static int SetRelative(this int value, string relative)
        {
            var sign = relative[0];
            var relVal = int.Parse(relative.Substring(1));
            switch (sign)
            {
                case '@':
                    return relVal;
                case '+':
                    return value + relVal;
                case '-':
                    return value - relVal;
                case '*':
                    return value * relVal;
                case '/':
                    return value / relVal;
                case '%':
                    return value % relVal;
                case '^':
                    return (int)Mathf.Pow(value, relVal);
            }

            return relVal;
        }
        
        public static float SetRelative(this float value, string relative)
        {
            var sign = relative[0];
            var relVal = float.Parse(relative.Substring(1));
            switch (sign)
            {
                case '@':
                    return relVal;
                case '+':
                    return value + relVal;
                case '-':
                    return value - relVal;
                case '*':
                    return value * relVal;
                case '/':
                    return value / relVal;
                case '%':
                    return value % relVal;
                case '^':
                    return Mathf.Pow(value, relVal);
            }

            return relVal;
        }
        
        public static double SetRelative(this double value, string relative)
        {
            var sign = relative[0];
            var relVal = double.Parse(relative.Substring(1));
            switch (sign)
            {
                case '@':
                    return relVal;
                case '+':
                    return value + relVal;
                case '-':
                    return value - relVal;
                case '*':
                    return value * relVal;
                case '/':
                    return value / relVal;
                case '%':
                    return value % relVal;
                case '^':
                    return Math.Pow(value, relVal);
            }

            return relVal;
        }

        public static Vector2 SetRelative(this Vector2 value, string relative)
        {
            if (relative.StartsWith("(") && relative.EndsWith(")") && relative.Count(c => c == ',') == 1)
            {
                relative = relative.Substring(1, relative.Length - 2);
                relative = relative.Replace(" ", "");
                var split = relative.Split(',');
                return new Vector2(
                    value[0].SetRelative(split[0]), 
                    value[1].SetRelative(split[1])
                    );
            }
            throw new Exception($"Expected \"(value, value)\" got \"{relative}\"");
        }
        
        public static Vector3 SetRelative(this Vector3 value, string relative)
        {
            if (relative.StartsWith("(") && relative.EndsWith(")") && relative.Count(c => c == ',') == 2)
            {
                relative = relative.Substring(1, relative.Length - 2);
                relative = relative.Replace(" ", "");
                var split = relative.Split(',');
                return new Vector3(
                    value[0].SetRelative(split[0]), 
                    value[1].SetRelative(split[1]),
                    value[2].SetRelative(split[2])
                );
            }
            throw new Exception($"Expected \"(value, value, value)\" got \"{relative}\"");
        }
        
        public static Rect SetRelative(this Rect value, string relative)
        {
            if (relative.StartsWith("(") && relative.EndsWith(")") && relative.Count(c => c == ',') == 2)
            {
                relative = relative.Substring(1, relative.Length - 2);
                relative = relative.Replace(" ", "");
                var split = relative.Split(',');
                return new Rect(
                    value.x.SetRelative(split[0]), 
                    value.y.SetRelative(split[1]),
                    value.width.SetRelative(split[2]),
                    value.height.SetRelative(split[3])
                );
            }
            throw new Exception($"Expected \"(x, y, width, height)\" got \"{relative}\"");
        }
    }
}