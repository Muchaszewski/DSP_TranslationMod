using HarmonyLib;
using UnityEngine;

namespace DSPTranslationPlugin.GameHarmony
{
    /// <summary>
    ///     Localizer postfix for expanding translation box to match new credits text
    /// </summary>
    [HarmonyPatch(typeof(Localizer), "Refresh")]
    public static class Localizer_Refresh_Harmony
    {
        /// <summary>
        ///     Expand in game credits box
        /// </summary>
        /// <param name="__instance"></param>
        [HarmonyPostfix]
        public static void Postfix(Localizer __instance)
        {
            if (__instance.name == "tip" && __instance.transform.parent.name == "language")
            {
                var rect = __instance.GetComponent<RectTransform>();
                var sizeDelta = rect.sizeDelta;
                sizeDelta.x = 600;
                sizeDelta.y = 90;
                rect.sizeDelta = sizeDelta;
            }
        }
    }
}