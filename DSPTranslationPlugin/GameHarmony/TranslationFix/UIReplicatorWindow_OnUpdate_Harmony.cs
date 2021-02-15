using System.Collections.Generic;
using System.IO;
using System.Linq;
using HarmonyLib;
using Mono.Cecil.Cil;
using TranslationCommon;
using TranslationCommon.Translation;
using UnityEngine.UI;
using OpCodes = System.Reflection.Emit.OpCodes;

namespace DSPTranslationPlugin.GameHarmony.TranslationFix
{
    public class UIReplicatorWindow_OnUpdate_Harmony
    {
        [HarmonyPatch(typeof(UIReplicatorWindow), "_OnUpdate")]
        [HarmonyDebug]
        public static class UIReplicatorWindow_OnUpdate_Prefix
        {
            private static bool isPatched = false;
            
            /// <summary>
            ///     Load current language after pressing "Apply" button
            /// </summary>
            [HarmonyPrefix]
            public static void Prefix(UIReplicatorWindow __instance)
            {
                if (!isPatched)
                {
                    var _tmp_text0 = AccessTools.Field(typeof(UIReplicatorWindow), "_tmp_text0");
                    _tmp_text0.SetValue(__instance, "制造队列".Translate());
                    isPatched = true;
                }
            }
        }
    }
}