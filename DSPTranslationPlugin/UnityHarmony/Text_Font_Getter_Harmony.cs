using HarmonyLib;
using UnityEngine.UI;

namespace DSPTranslationPlugin.UnityHarmony
{
    /// <summary>
    ///     Text harmony patcher to apply font to all requesting elements
    /// </summary>
    [HarmonyPatch(typeof(Text), nameof(Text.font), MethodType.Getter)]
    public static class Text_Font_Getter_Harmony
    {
        [HarmonyPrefix]
        public static void Prefix(Text __instance)
        {
            TextFontManager.Get(__instance)?.OnGetFont();
        }
    }
}