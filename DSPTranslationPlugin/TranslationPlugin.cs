using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using BepInEx;
using DSPTranslationPlugin.UnityHarmony;
using HarmonyLib;
using UnityEngine;

namespace DSPTranslationPlugin
{
    [BepInPlugin("com.muchaszewski.dsp_translationPlugin", "DSP Community Translation", "0.4.0")]
    public class TranslationPlugin : BaseUnityPlugin
    {
        public static MonoBehaviour StaticMonoBehaviour { get; private set; }
        
        private void Awake()
        {
            StaticMonoBehaviour = this;
            Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly());
        }
    }
}