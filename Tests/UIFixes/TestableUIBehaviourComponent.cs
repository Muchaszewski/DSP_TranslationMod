using System;
using System.Collections.Generic;
using System.ComponentModel;
using TranslationCommon.Fonts;

namespace Tests.UIFixes
{
    public class TestableUIBehaviourComponent : IBehaviourComponent
    {
        public string Type;
        public string Path;

        public string GetComponentType()
        {
            return Type;
        }

        public string GetComponentPath()
        {
            return Path;
        }

        public List<T> GetComponentsByField<T>(string field) 
            where T : UnityEngine.Component
        {
            throw new System.NotImplementedException();
        }

        public List<T> GetComponentsByPath<T>(string path, bool matchChildren) 
            where T : UnityEngine.Component
        {
            throw new System.NotImplementedException();
        }

        public List<T> GetComponentsByType<T>(bool matchAllChildren) 
            where T : UnityEngine.Component
        {
            throw new System.NotImplementedException();
        }
    }
}