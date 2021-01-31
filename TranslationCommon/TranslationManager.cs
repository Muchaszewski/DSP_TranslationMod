using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using SimpleJSON;
using UnityEngine;

namespace TranslationCommon
{
    public static class TranslationManager
    {
        public const string PlayerPrefsCode = "dps_translate_muchaszewski_language";
        
        private static List<LanguageContainer> _langauges;
        private static Dictionary<string, TranslationProto> _translationDictionary;
        private static LanguageContainer _currentLanguage;
        private static bool _isInitialized;

        /// <summary>
        ///     List of all translation languages, theirs settings and translations
        /// </summary>
        public static List<LanguageContainer> Langauges
        {
            get
            {
                if (_langauges == null)
                {
                    CheckForNewTranslationFolder();
                    if (!IsInitialized)
                    {
                        LoadCurrentLanguage();
                    }
                }
                return _langauges;
            }
            set => _langauges = value;
        }

        public static string SelectedLanguage { get; set; }

        public static LanguageContainer CurrentLanguage
        {
            get => _currentLanguage;
            set
            {
                _currentLanguage = value;
                if (_currentLanguage != null)
                {
                    SetupTranslationDictionary();
                    if (!IsInitialized)
                    {
                        LoadCurrentLanguage();
                    }
                }
            }
        }
        
        public static bool IsInitialized
        {
            get => _isInitialized;
            set => _isInitialized = value;
        }

        public static Dictionary<string, TranslationProto> TranslationDictionary
        {
            get => _translationDictionary;
            private set => _translationDictionary = value;
        }

        private static void SetupTranslationDictionary()
        {
            TranslationDictionary = CurrentLanguage?.Translation.TranslationTable.ToDictionary(proto => proto.Name);
        }
        
        public static void LoadCurrentLanguage()
        {
            if (!IsInitialized)
            {
                var stringProtoSet = LDB.strings;
                var templateLanguageData = new LanguageData(stringProtoSet);
                foreach (var languageContainer in Langauges)
                {
                    if (languageContainer.Translation == null)
                    {
                        languageContainer.LoadTranslation(templateLanguageData);
                    }
                }
                
                var result = PlayerPrefs.GetString(TranslationManager.PlayerPrefsCode);
                if (!String.IsNullOrEmpty(result))
                {
                    SelectedLanguage = result;
                    if (!Langauges.Exists(container =>
                        container.Settings.LanguageDisplayName == SelectedLanguage))
                    {
                        ConsoleLogger.LogFatal($"Selected language do not exists... {result}");
                        SelectedLanguage = null;
                    }
                }
                IsInitialized = true;
            }

            if (!string.IsNullOrEmpty(SelectedLanguage))
            {
                var selectedContainer = Langauges.FirstOrDefault(container =>
                    container.Settings.LanguageDisplayName == SelectedLanguage);
                CurrentLanguage = selectedContainer;
                Localization.language = Language.enUS;
                Localization.OnLanguageChange(Language.enUS);
            }
        }

        /// <summary>
        ///     Translation folder settings file name
        /// </summary>
        public const string SettingsJsonFileName = "settings.json";
        /// <summary>
        ///     Root translation folder
        /// </summary>
        public static string TranslationDirectory => $"{Utils.PluginPath}/Translation";
        

        /// <summary>
        ///     Gets all languages form the translation folder 
        /// </summary>
        private static void CheckForNewTranslationFolder()
        {
            if (!Directory.Exists(TranslationDirectory))
            {
                Directory.CreateDirectory(TranslationDirectory);
            }
            var directories = Directory.GetDirectories(TranslationDirectory);
            Langauges = new List<LanguageContainer>();
            foreach (var directory in directories)
            {
                LanguageSettings settings;
                settings = GetLanguageSettings(directory);

                settings.SettingsPath = Path.Combine(directory, SettingsJsonFileName);
                Langauges.Add(new LanguageContainer(settings));
            }
        }

        /// <summary>
        ///     Get language settings
        /// </summary>
        /// <param name="directory">Directory name</param>
        /// <returns></returns>
        private static LanguageSettings GetLanguageSettings(string directory)
        {
            LanguageSettings settings = null;
            
            string translationMain = null;
            foreach (var file in  Directory.GetFiles(directory))
            {
                if (file.Contains(SettingsJsonFileName)) translationMain = file;
            }
            
            if (translationMain == null)
            {
                settings = new LanguageSettings()
                {
                    LanguageDisplayName = Path.GetFileName(directory),
                };
                File.WriteAllText(Path.Combine(directory, SettingsJsonFileName), JSON.ToJson(settings, true));
            }
            else
            {
                settings = JSON.FromJson<LanguageSettings>(File.ReadAllText(translationMain));
                // Overwrite file with potential new settings
                File.WriteAllText(Path.Combine(directory, SettingsJsonFileName), JSON.ToJson(settings, true));
            }

            return settings;
        }
    }
}