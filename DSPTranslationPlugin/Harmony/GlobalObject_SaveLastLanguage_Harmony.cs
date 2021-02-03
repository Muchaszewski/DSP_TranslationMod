using HarmonyLib;
using TranslationCommon;
using UnityEngine;

namespace DSPSimpleBuilding
{
    [HarmonyPatch(typeof(GlobalObject), nameof(GlobalObject.SaveLastLanguage))]
    public static class GlobalObject_SaveLastLanguage_Harmony
    {
        [HarmonyPrefix]
        public static void Prefix()
        {
            if (!TranslationManager.IsInitialized)
            {
                TranslationManager.LoadCurrentLanguage();
            }
            else
            {
                if (TranslationManager.CurrentLanguage == null)
                {
                    PlayerPrefs.DeleteKey(TranslationManager.PlayerPrefsCode);
                }
                else
                {
                    PlayerPrefs.SetString(TranslationManager.PlayerPrefsCode, TranslationManager.CurrentLanguage.Settings.LanguageDisplayName);
                }
                PlayerPrefs.Save();
            }
        }
    }
}