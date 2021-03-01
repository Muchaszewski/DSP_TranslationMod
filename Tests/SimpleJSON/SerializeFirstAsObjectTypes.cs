using System.Collections.Generic;
using TranslationCommon.SimpleJSON;
using UnityEngine;

namespace Tests
{
    public class SerializeFirstAsObjectTypes
    {
        [SerializeField]
        [SerializeFirstAsObject]
        private Dictionary<string, string> _dictionary = new Dictionary<string, string>();

        public void Add(string key, string value)
        {
            _dictionary.Add(key, value);
        }

        public Dictionary<string, string> Get()
        {
            return _dictionary;
        }
    }
}