using System.Collections.Generic;
using TranslationCommon.SimpleJSON;
using UnityEngine;

namespace TranslationCommon.Fonts
{
    public class UIActionFixes
    {
        [SerializeField]
        [SerializeFirstAsObject]
        private Dictionary<string, List<UIFix>> _serializedFixMap = new Dictionary<string, List<UIFix>>();
        
        private Dictionary<string, List<UIFix>> _typeFixMap;
        private List<UIPathTarget> _pathFixMap;

        public List<UIFix> GetByType(IBehaviourComponent component)
        {
            if (_typeFixMap == null)
            {
                Initialize(_serializedFixMap);
            }
            
            var name = component.GetComponentType();
            return _typeFixMap.ContainsKey(name) ? _typeFixMap[name] : new List<UIFix>();
        }

        public List<UIFix> GetByPath(IBehaviourComponent component)
        {
            if (_pathFixMap == null)
            {
                Initialize(_serializedFixMap);
            }
            
            var name = component.GetComponentPath();
            var fixes = new List<UIFix>();
            for (int i = 0; i < _pathFixMap.Count; i++)
            {
                if (name.EqualsWildcard(_pathFixMap[i].Path))
                {
                    fixes.AddRange(_pathFixMap[i].Fixes);
                }
            }
            return fixes;
        }

        public void Initialize(Dictionary<string, List<UIFix>> serializedFixesMap)
        {
            _serializedFixMap = serializedFixesMap;
            _typeFixMap = new Dictionary<string, List<UIFix>>();
            _pathFixMap = new List<UIPathTarget>();
            foreach (var pair in serializedFixesMap)
            {
                if (pair.Key.StartsWith("t:"))
                {
                    var key = pair.Key.Remove(0,2);
                    _typeFixMap.Add(key, pair.Value);
                }
                else if (pair.Key.StartsWith("p:"))
                {
                    var path = pair.Key.Remove(0,2);
                    _pathFixMap.Add(new UIPathTarget()
                    {
                        Path = path,
                        Fixes = pair.Value,
                    });
                }
            }
        }
    }
}