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
        [HarmonyPatch("_OnCreate")]
        public static void Postfix_OnCreate(ManualBehaviour __instance)
        {
            if (!BehaviourComponents.ContainsKey(__instance))
            {
                BehaviourComponents.Add(__instance, new UIBehaviourComponent(__instance));
            }
            BehaviourComponents[__instance].OnCreate();
        }

        [HarmonyPostfix]
        [HarmonyPatch("_OnDestroy")]
        public static void Postfix_OnDestroy(ManualBehaviour __instance)
        {
            BehaviourComponents.Remove(__instance);
        }

        [HarmonyPostfix]
        [HarmonyPatch("_OnInit")]
        public static void Postfix_OnInit(ManualBehaviour __instance)
        {
            if (!BehaviourComponents.ContainsKey(__instance))
            {
                BehaviourComponents.Add(__instance, new UIBehaviourComponent(__instance));
            }
            BehaviourComponents[__instance].OnInit();
        }

        [HarmonyPostfix]
        [HarmonyPatch("_OnOpen")]
        public static void Postfix_OnOpen(ManualBehaviour __instance)
        {
            if (!BehaviourComponents.ContainsKey(__instance))
            {
                BehaviourComponents.Add(__instance, new UIBehaviourComponent(__instance));
            }
            BehaviourComponents[__instance].OnOpen();
        }

        [HarmonyPostfix]
        [HarmonyPatch("_OnUpdate")]
        public static void Postfix_OnUpdate(ManualBehaviour __instance)
        {
            if (!BehaviourComponents.ContainsKey(__instance))
            {
                BehaviourComponents.Add(__instance, new UIBehaviourComponent(__instance));
            }
            BehaviourComponents[__instance].OnUpdate();
        }
        
        [HarmonyPostfix]
        [HarmonyPatch("_OnLateUpdate")]
        public static void Postfix_OnLateUpdate(ManualBehaviour __instance)
        {
            if (!BehaviourComponents.ContainsKey(__instance))
            {
                BehaviourComponents.Add(__instance, new UIBehaviourComponent(__instance));
            }
            BehaviourComponents[__instance].OnLateUpdate();
        }
    }
}