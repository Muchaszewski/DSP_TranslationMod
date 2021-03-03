using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using TranslationCommon.SimpleJSON;
using UnityEngine;

namespace TranslationCommon.Fonts
{
    /*
     * ActionFixMap
     * {
     *      "OnCreate" : {
     *          "t:UIAction":{...           
     *                      }
     *          "p:*dasdas":{...}
     *          "p:*dasdas":{...}
     *          }
     * }
     *
     * {
     *      Type: "Text",
     *      Path: "content1",
     *      Field: "_dspText",
     *      Text: [],
     *      RectTransform: [],
     * }
     */

    public class UIBehaviourComponent : IBehaviourComponent
    {
        private static UIBehaviourCache _behaviourCache;

        private readonly ManualBehaviour _behaviour;

        public UIBehaviourComponent(ManualBehaviour behaviour)
        {
            _behaviour = behaviour;
        }

        public static UIBehaviourCache BehaviourCache
        {
            get
            {
                if (_behaviourCache == null)
                {
                    var path = $"{Utils.ConfigPath}/Translation/fontFix.json";
                    if (File.Exists(path))
                    {
                        _behaviourCache = new UIBehaviourCache();
                        var json = File.ReadAllText(path);
                        _behaviourCache.UIFixes = JSON.FromJson<UIFixesData>(json);
                    }
                }

                return _behaviourCache;
            }
        }

        public string GetComponentType()
        {
            return _behaviour.GetType().Name;
        }

        public string GetComponentPath()
        {
            return GetComponentPath(_behaviour);
        }

        public List<T> GetComponentsByField<T>(string field) where T : Component
        {
            var fieldInfos = _behaviour.GetType()
                .GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            var limitedFieldInfos = fieldInfos
                .Where(info => info.FieldType.IsAssignableFrom(typeof(T)))
                .Where(info => info.FieldType.Name.EqualsWildcard(field));
            return limitedFieldInfos.Select(info => (T) info.GetValue(_behaviour)).ToList();
        }

        public List<T> GetComponentsByPath<T>(string path, List<string> except, RangeValue matchChildrenRange) where T : Component
        {
            var components = _behaviour.GetComponentsInChildrenWithSelf<T>(matchChildrenRange);
            return components
                .Where(arg =>
                {
                    var componentPath = GetComponentPath(arg);
                    if (except != null && except.Any(ex => componentPath.EqualsWildcard(ex)))
                    {
                        return false;
                    }
                    return componentPath.EqualsWildcard(path);
                })
                .ToList();
        }

        public List<T> GetComponentsByType<T>(RangeValue matchChildrenRange) where T : Component
        {
            return _behaviour.GetComponentsInChildrenWithSelf<T>(matchChildrenRange).ToList();
        }

        public static string GetComponentPath(Component component)
        {
            var pathList = new List<string>();
            var current = component.gameObject.transform;
            while (current != null)
            {
                pathList.Add(current.name);
                current = current.parent;
            }

            pathList.Reverse();
            var join = string.Join(".", pathList.ToArray());
            return join;
        }

        public void OnCreate()
        {
            BehaviourCache?.GetFixes("OnCreate", this)?
                .ForEach(fix => fix.Fix.Evaluate(fix, this));
        }

        public void OnInit()
        {
            BehaviourCache?.GetFixes("OnInit", this)?
                .ForEach(fix => fix.Fix.Evaluate(fix, this));
        }

        public void OnOpen()
        {
            BehaviourCache?.GetFixes("OnOpen", this)?
                .ForEach(fix => fix.Fix.Evaluate(fix, this));
        }

        public void OnUpdate()
        {
            BehaviourCache?.GetFixes("OnUpdate", this)?
                .ForEach(fix => fix.Fix.Evaluate(fix, this));
        }

        public void OnLateUpdate()
        {
            BehaviourCache?.GetFixes("OnLateUpdate", this)?
                .ForEach(fix => fix.Fix.Evaluate(fix, this));
        }
    }
}