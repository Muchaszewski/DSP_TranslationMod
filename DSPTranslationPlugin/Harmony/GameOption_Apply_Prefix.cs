using HarmonyLib;
using TranslationCommon;

namespace DSPSimpleBuilding
{
    [HarmonyPatch(typeof(GameOption), nameof(GameOption.Apply))]
    public static class GameOption_Apply_Prefix
    {
        [HarmonyPrefix]
        public static void Prefix()
        {
            TranslationManager.LoadCurrentLanguage();
        }
    }
}