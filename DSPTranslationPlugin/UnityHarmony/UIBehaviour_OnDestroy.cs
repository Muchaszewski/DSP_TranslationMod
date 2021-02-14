using HarmonyLib;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace DSPTranslationPlugin.UnityHarmony
{
    [HarmonyPatch(typeof(UIBehaviour), "OnDestroy")]
    public static class UIBehaviour_OnDestroy
    {
        /// <summary>
        ///     OnDestroy method for Text to remove text from TextFontManger
        /// </summary>
        /// <param name="__instance"></param>
        [HarmonyPrefix]
        public static void Prefix(UIBehaviour __instance)
        {
            if (__instance is Text text)
            {
                TextFontManager.Remove(text);
            }
        }
    }
}