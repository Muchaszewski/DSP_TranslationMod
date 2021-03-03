using System.Collections.Generic;
using System.Linq;
using TranslationCommon.Fonts;
using UnityEngine;

namespace TranslationCommon
{
    public static class GameObjectsUtils
    {
        public static IEnumerable<GameObject> GetChildrenWithSelf(this Component component, RangeValue childLayers)
        {
            return GetChildrenWithSelf(component.transform, childLayers);
        }
        
        public static IEnumerable<GameObject> GetChildrenWithSelf(this GameObject gameObject, RangeValue childLayers)
        {
            return GetChildrenWithSelf(gameObject.transform, childLayers);
        }
        
        public static IEnumerable<GameObject> GetChildrenWithSelf(this Transform transform, RangeValue childLayers)
        {
            return GetChildrenWithSelf_Impl(transform, childLayers, 0);
        }

        private static IEnumerable<GameObject> GetChildrenWithSelf_Impl(this Transform transform,
            RangeValue childLayers, int index)
        {
            if (childLayers == null)
            {
                yield return transform.gameObject;
                yield break;
            }
            
            if (!childLayers.TryGetMatch(index, out var match))
            {
                yield break;
            }

            if (index == match)
            {
                yield return transform.gameObject;
            }

            for (var i = 0; i < transform.childCount; i++)
            {
                var child = transform.GetChild(i);
                foreach (var gameObject in GetChildrenWithSelf_Impl(child, childLayers, index + 1))
                {
                    yield return gameObject;
                }
            }
        }
        
        public static IEnumerable<T> GetComponentsInChildrenWithSelf<T>(this Component component, RangeValue childLayers)
            where T : Component
        {
            return GetComponentsInChildrenWithSelf<T>(component.transform, childLayers);
        }
        
        public static IEnumerable<T> GetComponentsInChildrenWithSelf<T>(this GameObject gameObject, RangeValue childLayers)
            where T : Component
        {
            return GetComponentsInChildrenWithSelf<T>(gameObject.transform, childLayers);
        }

        public static IEnumerable<T> GetComponentsInChildrenWithSelf<T>(this Transform transform, RangeValue childLayers)
            where T : Component
        {
            return GetChildrenWithSelf(transform, childLayers).SelectMany(gameObject => gameObject.GetComponents<T>());
        }
    }
}