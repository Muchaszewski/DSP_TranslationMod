using System.Collections.Generic;
using UnityEngine;

namespace TranslationCommon.Fonts
{
    public interface IBehaviourComponent
    {
        string GetComponentType();
        string GetComponentPath();

        List<T> GetComponentsByField<T>(string field) where T : Component;

        List<T> GetComponentsByPath<T>(string path, List<string> except, RangeValue matchChildrenRange) where T : Component;

        List<T> GetComponentsByType<T>(RangeValue matchChildrenRange) where T : Component;
    }
}