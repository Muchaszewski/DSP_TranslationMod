using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

namespace TranslationCommon.Fonts
{
    /// <summary>
    ///     Range value that can 
    /// </summary>
    /// <example>
    ///     Following strings will provide following range
    ///     "4" - match only index equal to 4
    ///     "<4" - match indexes that are larger then 4
    ///     ">4" - match indexes that are smaller then 4
    ///     "<=4" - match indexes that are larger or equal 4
    ///     ">=4" - match indexes that are smaller or equal 4
    ///     "2..4" - match indexes between 2 inclusive and 4 inclusive
    ///     "2;4" - match indexes 2 and 4
    ///     Complex example:
    ///     "3..5; 8; <12" - match indexes 3 to 5 inclusive, index 8 and all above 12
    /// </example>
    public class RangeValue
    {
        /// <summary>
        ///     All tokens used for this range
        /// </summary>
        private readonly List<Token> _rangeTokens = new List<Token>();
        /// <summary>
        ///      Only currently valid tokens for this iteration
        /// </summary>
        private readonly List<Token> _validRangeTokens = new List<Token>();
        
        /// <summary>
        ///     Current index
        /// </summary>
        private int _currentIndex = 0;
        
        /// <summary>
        ///     Parse string to RangeValue
        /// </summary>
        /// <param name="rangeString"></param>
        /// <returns></returns>
        public static RangeValue Parse(string rangeString)
        {
            return new RangeValue(rangeString);
        }

        /// <summary>
        ///     Hide default constructor
        /// </summary>
        [Obsolete]
        private RangeValue()
        {
            
        }

        /// <summary>
        ///     Default constructor
        /// </summary>
        /// <param name="rangeString"></param>
        private RangeValue(string rangeString)
        {
            var split = rangeString.Replace(" ", "").Split(';');
            foreach (var s in split)
            {
                var token = Token.GetMatchingToken(s);
                _rangeTokens.Add(token);
                if (token.TryGetNextValid(_currentIndex, out var nextValid))
                {
                    _validRangeTokens.Add(token);
                }
            }
        }
        
        /// <summary>
        ///     Try get next match, returns next valid index
        /// </summary>
        /// <param name="match"></param>
        /// <returns></returns>
        public bool TryGetNextMatch(out int match)
        {
            match = Int32.MaxValue;
            for (var i = 0; i < _validRangeTokens.Count; i++)
            {
                var rangeToken = _validRangeTokens[i];
                if (rangeToken.TryGetNextValid(_currentIndex, out var nextValid))
                {
                    match = Mathf.Min(match, nextValid);
                }
                else
                {
                    _validRangeTokens.Remove(rangeToken);
                    i--;
                }
            }

            if (match != Int32.MaxValue)
            {
                _currentIndex = match + 1;
                return true;
            }

            match = 0;
            return false;
        }

        /// <summary>
        ///     Try get next match from provided index, skipping range optimizations
        /// </summary>
        /// <param name="currentIndex"></param>
        /// <param name="match"></param>
        /// <returns></returns>
        public bool TryGetMatch(int currentIndex, out int match)
        {
            match = Int32.MaxValue;
            for (var i = 0; i < _rangeTokens.Count; i++)
            {
                var rangeToken = _rangeTokens[i];
                if (rangeToken.TryGetNextValid(currentIndex, out var nextValid))
                {
                    match = Mathf.Min(match, nextValid);
                }
            }

            if (match != Int32.MaxValue)
            {
                return true;
            }

            match = 0;
            return false;
        }

        /// <summary>
        ///     Resets range iteration
        /// </summary>
        /// <param name="startRangeValue"></param>
        public void Reset(int startRangeValue = 0)
        {
            _currentIndex = startRangeValue;
            _validRangeTokens.Clear();
            foreach (var rangeToken in _rangeTokens)
            {
                if (rangeToken.TryGetNextValid(_currentIndex, out var nextValid))
                {
                    _validRangeTokens.Add(rangeToken);
                }
            }
        }

        /// <summary>
        ///     Base class for token
        /// </summary>
        private abstract class Token
        {
            /// <summary>
            ///     Create token that matches provided string, string needs to be a single value
            /// </summary>
            /// <param name="tokenString"></param>
            /// <returns></returns>
            /// <exception cref="Exception"></exception>
            public static Token GetMatchingToken(string tokenString)
            {
                if (Equal.TryParse(tokenString, out var token)) return token;
                if (LessThen.TryParse(tokenString, out token)) return token;
                if (LessThenEqual.TryParse(tokenString, out token)) return token;
                if (GreaterThen.TryParse(tokenString, out token)) return token;
                if (GreaterThenEqual.TryParse(tokenString, out token)) return token;
                if (Between.TryParse(tokenString, out token)) return token;

                throw new Exception($"Provided string was invalid {tokenString}, please provide valid range token");
            }
            
            /// <summary>
            ///     Try get next value, returns next valid value and true if possible, else false
            /// </summary>
            /// <param name="current">Current range value</param>
            /// <param name="nextValid">Next valid output</param>
            /// <returns></returns>
            public abstract bool TryGetNextValid(int current, out int nextValid);
        }

        /// <summary>
        ///     Token for Equal operation
        /// </summary>
        private class Equal : Token
        {
            private int _value;
            
            public static bool TryParse(string tokenString, out Token token)
            {
                if (int.TryParse(tokenString, out var result))
                {
                    token = new Equal()
                    {
                        _value = result
                    };
                    return true;
                }
                token = null;
                return false;
            }

            public override bool TryGetNextValid(int current, out int nextValid)
            {
                if (current <= _value)
                {
                    nextValid = _value;
                    return true;
                }

                nextValid = 0;
                return false;
            }
        }
        
        /// <summary>
        ///     Token for LessThen operation
        /// </summary>
        private class LessThen : Token
        {
            private int _value;
            
            public static bool TryParse(string tokenString, out Token token)
            {
                if (tokenString[0] == '<' && 
                    int.TryParse(tokenString.Remove(0, 1), out var result))
                {
                    token = new LessThen()
                    {
                        _value = result
                    };
                    return true;
                }
                token = null;
                return false;
            }

            public override bool TryGetNextValid(int current, out int nextValid)
            {
                if (current < _value)
                {
                    nextValid = current;
                    return true;
                }

                nextValid = 0;
                return false;
            }
        }
        
        /// <summary>
        ///     Token for LessThenEqual operation
        /// </summary>
        private class LessThenEqual : Token
        {
            private int _value;
            
            public static bool TryParse(string tokenString, out Token token)
            {
                if (tokenString[0] == '<' && tokenString[1] == '=' &&
                    int.TryParse(tokenString.Remove(0, 2), out var result))
                {
                    token = new LessThenEqual()
                    {
                        _value = result
                    };
                    return true;
                }
                token = null;
                return false;
            }

            public override bool TryGetNextValid(int current, out int nextValid)
            {
                if (current <= _value)
                {
                    nextValid = current;
                    return true;
                }

                nextValid = 0;
                return false;
            }
        }
        
        /// <summary>
        ///     Token for GreaterThen operation
        /// </summary>
        private class GreaterThen : Token
        {
            private int _value;
            
            public static bool TryParse(string tokenString, out Token token)
            {
                if (tokenString[0] == '>' && 
                    int.TryParse(tokenString.Remove(0, 1), out var result))
                {
                    token = new GreaterThen()
                    {
                        _value = result
                    };
                    return true;
                }
                token = null;
                return false;
            }

            public override bool TryGetNextValid(int current, out int nextValid)
            {
                if (current > _value)
                {
                    nextValid = current;
                    return true;
                }
                else
                {
                    nextValid = _value + 1;
                    return true;
                }
            }
        }

        /// <summary>
        ///     Token for GreaterThenEqual operation
        /// </summary>
        private class GreaterThenEqual : Token
        {
            private int _value;
            
            public static bool TryParse(string tokenString, out Token token)
            {
                if (tokenString[0] == '>' && tokenString[1] == '=' &&
                    int.TryParse(tokenString.Remove(0, 2), out var result))
                {
                    token = new GreaterThenEqual()
                    {
                        _value = result
                    };
                    return true;
                }
                token = null;
                return false;
            }

            public override bool TryGetNextValid(int current, out int nextValid)
            {
                if (current >= _value)
                {
                    nextValid = current;
                    return true;
                }
                else
                {
                    nextValid = _value;
                    return true;
                }
            }
        }
        
        /// <summary>
        ///     Token for Between operation
        /// </summary>
        private class Between : Token
        {
            private int _lessThen;
            private int _greaterThen;
            
            public static bool TryParse(string tokenString, out Token token)
            {
                var split = tokenString.Split(new[] {".."}, StringSplitOptions.RemoveEmptyEntries);

                if (int.TryParse(split[0], out var lessThen) && 
                    int.TryParse(split[1], out var greaterThen))
                {
                    token = new Between()
                    {
                        _lessThen = lessThen < greaterThen ? lessThen : greaterThen,
                        _greaterThen = lessThen < greaterThen ? greaterThen : lessThen,
                    };
                    return true;
                }
                
                token = null;
                return false;
            }

            public override bool TryGetNextValid(int current, out int nextValid)
            {
                if (current <= _lessThen)
                {
                    nextValid = _lessThen;
                    return true;
                }
                else if (current >= _lessThen && current <= _greaterThen)
                {
                    nextValid = current;
                    return true;
                }

                nextValid = 0;
                return false;
            }
        }
    }
}