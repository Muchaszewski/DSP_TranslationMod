using System;
using HarmonyLib;
using TranslationCommon.Translation;
using UnityEngine;

namespace DSPTranslationPlugin.GameHarmony
{
    [HarmonyPatch(typeof(GlobalObject), "Initialize")]
    public static class GlobalObject_Initialize_Harmony
    {
        /// <summary>
        ///     Try settings previously saved language
        /// </summary>
        [HarmonyPrefix]
        public static void Prefix()
        {
            var result = PlayerPrefs.GetString(TranslationManager.PlayerPrefsCode);
            if (!String.IsNullOrEmpty(result))
            {
                TranslationManager.SelectedLanguage = result;
                TranslationManager.LoadCurrentLanguage();
            }
        }
        
    }
}