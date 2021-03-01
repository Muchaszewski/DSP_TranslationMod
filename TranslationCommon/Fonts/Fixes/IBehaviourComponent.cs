using System.Collections.Generic;
using UnityEngine;

namespace TranslationCommon.Fonts
{
    public interface IBehaviourComponent
    {
        string GetComponentType();
        string GetComponentPath();

        List<T> GetComponentsByField<T>(string field) where T : Component;

        List<T> GetComponentsByPath<T>(string path, bool matchChildren) where T : Component;

        List<T> GetComponentsByType<T>(bool matchAllChildren) where T : Component;
    }
}