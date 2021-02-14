using System;
using System.IO;

namespace TranslationCommon.Translation
{
    /// <summary>
    ///     Settings file
    /// </summary>
    [Serializable]
    public class LanguageSettings
    {
        /// <summary>
        ///     Translation settings version
        /// </summary>
        public string Version;

        /// <summary>
        ///     Game version
        /// </summary>
        public string GameVersion;

        /// <summary>
        ///     Original language of the in game translation
        /// </summary>
        public string OriginalLanguage;

        /// <summary>
        ///     Display name in the menu
        /// </summary>
        public string LanguageDisplayName;

        /// <summary>
        ///     Should use legacy format
        /// </summary>
        public bool ImportFromLegacy;

        /// <summary>
        ///     Should use plain text format
        /// </summary>
        public bool CreateAndUpdateFromPlainTextDumpUnsafe;

        /// <summary>
        ///     Name of builtin font
        /// </summary>
        public string BuildInFontName;

        /// <summary>
        ///     Name of builtin font
        /// </summary>
        public string CustomFontLocation;
        
        /// <summary>
        ///     Settings path
        /// </summary>
        public string SettingsPath { get; set; }

        /// <summary>
        ///     Settings directory
        /// </summary>
        public string SettingsDirectory => Path.GetDirectoryName(SettingsPath);

        /// <summary>
        ///     Update settings file
        /// </summary>
        public void VersionUpdate()
        {
            var defaultVersion = "0.1.0.3";
            if (Version != defaultVersion) Version = defaultVersion;
            
            GameVersion = GameConfig.gameVersion.ToFullString();
            
            if (String.IsNullOrEmpty(OriginalLanguage)) OriginalLanguage = "ENUS";
        }
    }
}