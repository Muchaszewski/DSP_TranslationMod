using System;
using System.Collections.Generic;
using UnityEngine;

namespace TranslationCommon.Fonts
{
    public class UIFix
    {
        public string Path;
        public string Field;

        public bool MatchChildren;
        
        public IFix Fix;

        public List<T> GetComponents<T>(IBehaviourComponent component) 
            where T : Component
        {
            if (!String.IsNullOrEmpty(Path))
            {
                return component.GetComponentsByPath<T>(Path, MatchChildren);
            }
            else if (!String.IsNullOrEmpty(Field))
            {
                return component.GetComponentsByField<T>(Field);
            }
            else
            {
                return component.GetComponentsByType<T>(MatchChildren);
            }
        }
    }
}