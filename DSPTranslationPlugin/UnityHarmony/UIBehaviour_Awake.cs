using HarmonyLib;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace DSPTranslationPlugin.UnityHarmony
{
    [HarmonyPatch(typeof(UIBehaviour), "Awake")]
    public static class UIBehaviour_Awake
    {
        /// <summary>
        ///     Awake method for Text to add TextFontManger
        /// </summary>
        /// <param name="__instance"></param>
        [HarmonyPrefix]
        public static void Prefix(UIBehaviour __instance)
        {
            if (__instance is Text text)
            {
                TextFontManager.Add(text);
            }
        }
    }
}