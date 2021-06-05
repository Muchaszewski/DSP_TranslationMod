using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;
using UnityEngine.UI;

namespace DSPTranslationPlugin.GameHarmony.TranslationFix
{
    public class UIReplicatorWindow_OnUpdate_Harmony
    {
        [HarmonyPatch(typeof(UIReplicatorWindow), "_OnUpdate")]
        public static class UIReplicatorWindow_OnUpdate_Prefix
        {
            private static bool isPatched = false;
            
            /// <summary>
            ///     Fixed "Replicating Queue" text
            /// </summary>
            /// <param name="__instance"></param>
            [HarmonyPrefix]
            public static void Prefix(UIReplicatorWindow __instance)
            {
                if (!isPatched)
                {
                    var _tmp_text0 = AccessTools.Field(typeof(UIReplicatorWindow), "_tmp_text0");
                    // 制造队列 == "Replicating Queue"
                    _tmp_text0.SetValue(__instance, "制造队列".Translate());
                    isPatched = true;
                }
            }
        }
    }
}