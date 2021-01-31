using System;
using System.IO;
using UnityEngine.Serialization;

namespace TranslationCommon
{
    [Serializable]
    public class LanguageSettings
    {
        public string Version;
        public string GameVersion;
        
        public string LanguageDisplayName;

        public bool CreateAndUpdateFromPlainTextDumpUnsafe;

        public string SettingsPath { get; set; }
        public string SettingsDirectory => Path.GetDirectoryName(SettingsPath);

        public LanguageSettings()
        {
            Version = "0.1.0.0";
            GameVersion = GameConfig.gameVersion.ToFullString();
        }
    }
}