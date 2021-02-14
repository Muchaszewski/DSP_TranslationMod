using HarmonyLib;
using TranslationCommon.Translation;

namespace DSPTranslationPlugin.GameHarmony
{
    
    [HarmonyPatch(typeof(GameOption), nameof(GameOption.Apply))]
    public static class GameOption_Apply_Harmony
    {
        /// <summary>
        ///     Load current language after pressing "Apply" button
        /// </summary>
        [HarmonyPrefix]
        public static void Prefix(GameOption __instance)
        {
            TranslationManager.LoadCurrentLanguage();
        }
    }
}