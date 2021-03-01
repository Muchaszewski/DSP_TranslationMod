using System.Collections.Generic;

namespace TranslationCommon.SimpleJSON
{
    public static class SimpleJSONUtils
    {
        public static IEnumerable<T> ToEnumerable<T>(this IEnumerator<T> enumerator) 
        {
            while ( enumerator.MoveNext() ) {
                yield return enumerator.Current;
            }
        }
        
        public static IEnumerable<string> ToKeyEnumerable(this JSONNode.Enumerator enumerator) 
        {
            while ( enumerator.MoveNext() ) {
                yield return enumerator.Current.Key;
            }
        }
    }
}