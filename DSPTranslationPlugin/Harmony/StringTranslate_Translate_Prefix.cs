using System.Linq;
using HarmonyLib;
using TranslationCommon;

namespace DSPSimpleBuilding
{
    [HarmonyPatch(typeof(StringTranslate), nameof(StringTranslate.Translate), typeof(string))]
    public static class StringTranslate_Translate_Prefix
    {
        [HarmonyPrefix]
        public static bool Prefix(ref string __result, string s)
        {
            if (s == null)
            {
                return true;
            }

            if (TranslationManager.CurrentLanguage != null)
            {
                if (TranslationManager.TranslationDictionary.ContainsKey(s))
                {
                    __result = TranslationManager.TranslationDictionary[s].Translation;
                    return false;
                }

                return true;
            }
            
            return true;
        }
    }
}