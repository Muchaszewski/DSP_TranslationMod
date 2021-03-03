using System.Collections.Generic;
using HarmonyLib;
using TranslationCommon.Fonts;

namespace DSPTranslationPlugin.GameHarmony
{
    [HarmonyPatch(typeof(ManualBehaviour))]
    public static class ManualBehaviour_All_Harmony
    {
        public static Dictionary<ManualBehaviour, UIBehaviourComponent> BehaviourComponents =
            new Dictionary<ManualBehaviour, UIBehaviourComponent>();
        
        [HarmonyPostfix]
        [HarmonyPatch("_Create")]
        public static void Postfix_OnCreate(ManualBehaviour __instance)
        {
            if (!BehaviourComponents.ContainsKey(__instance))
            {
                BehaviourComponents.Add(__instance, new UIBehaviourComponent(__instance));
            }
            BehaviourComponents[__instance].OnCreate();
        }

        [HarmonyPostfix]
        [HarmonyPatch("_Destroy")]
        public static void Postfix_OnDestroy(ManualBehaviour __instance)
        {
            BehaviourComponents.Remove(__instance);
        }

        [HarmonyPostfix]
        [HarmonyPatch("_Init")]
        public static void Postfix_OnInit(ManualBehaviour __instance)
        {
            if (!BehaviourComponents.ContainsKey(__instance))
            {
                BehaviourComponents.Add(__instance, new UIBehaviourComponent(__instance));
            }
            BehaviourComponents[__instance].OnInit();
        }

        [HarmonyPostfix]
        [HarmonyPatch("_Open")]
        public static void Postfix_OnOpen(ManualBehaviour __instance)
        {
            if (!BehaviourComponents.ContainsKey(__instance))
            {
                BehaviourComponents.Add(__instance, new UIBehaviourComponent(__instance));
            }

            BehaviourComponents[__instance].OnOpen();
        }

        [HarmonyPostfix]
        [HarmonyPatch("_Update")]
        public static void Postfix_OnUpdate(ManualBehaviour __instance)
        {
            if (__instance.isActiveAndEnabled)
            {
                if (!BehaviourComponents.ContainsKey(__instance))
                {
                    BehaviourComponents.Add(__instance, new UIBehaviourComponent(__instance));
                }

                BehaviourComponents[__instance].OnUpdate();
            }
        }
        
        [HarmonyPostfix]
        [HarmonyPatch("_LateUpdate")]
        public static void Postfix_OnLateUpdate(ManualBehaviour __instance)
        {
            if (__instance.isActiveAndEnabled)
            {
                if (!BehaviourComponents.ContainsKey(__instance))
                {
                    BehaviourComponents.Add(__instance, new UIBehaviourComponent(__instance));
                }
                BehaviourComponents[__instance].OnLateUpdate();
            }
        }
    }
}