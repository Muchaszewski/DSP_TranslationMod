using System;
using HarmonyLib;
using TranslationCommon;
using UnityEngine;

namespace DSPSimpleBuilding
{
    [HarmonyPatch(typeof(GlobalObject), "Initialize")]
    public static class GlobalObject_Initialize_Harmony
    {
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