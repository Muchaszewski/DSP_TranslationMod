using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using TranslationCommon.SimpleJSON;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace TranslationCommon.Translation
{
    /// <summary>
    ///     Container for single language methods
    /// </summary>
    public class LanguageContainer
    {
        /// <summary>
        ///     Settings of the language
        /// </summary>
        public LanguageSettings Settings;
        
        /// <summary>
        ///     Translation data
        /// </summary>
        public LanguageData Translation;
        
        public Font[] Fonts;

        /// <summary>
        ///     Constructor of the container with settings alread loaded into memory
        /// </summary>
        /// <param name="settings"></param>
        public LanguageContainer(LanguageSettings settings)
        {
            Settings = settings;
        }

        /// <summary>
        ///     Load translation data using template Language data
        /// </summary>
        /// <param name="template"></param>
        public void LoadTranslation(LanguageData template)
        {
            var translationFilePath = Path.Combine(Settings.SettingsDirectory, LanguageData.TranslationFileName);
            PlainTextDump(template, translationFilePath);

            if (Fonts == null)
            {
                Debug.Log($"Loading bundle {Settings.FontBundlePath}");
                AssetBundle fontBundle = AssetBundle.LoadFromFile(Settings.FontBundlePath);
                if (fontBundle != null)
                {
                    Fonts = fontBundle.LoadAllAssets<Font>();
                }
            }

            if (Settings.ImportFromLegacy)
            {
                LegacyDump(template, translationFilePath);
            }
            else
            {
                CrowdinDump(template);
            }
        }

        /// <summary>
        ///     Load and/or Dump Crowdin format translation
        /// </summary>
        /// <param name="template">Template for new language files</param>
        private void CrowdinDump(LanguageData template)
        {
            Func<TranslationProto, string> key = proto => $"{proto.Name}";
            Func<TranslationProto, string> value = proto => proto.Translation;
            var translationCrowdinFilePath = Path.Combine(Settings.SettingsDirectory, LanguageData.TranslationCrowdinFileName);
            if (!File.Exists(translationCrowdinFilePath))
            {
                var dict = ToCrowdinDictionary(template.TranslationTable, key, value);
                File.WriteAllText(translationCrowdinFilePath, JSON.ToJson(dict));
            }
            else
            {
                var dictionary = JSON.FromJson<Dictionary<string, string>>(File.ReadAllText(translationCrowdinFilePath));
                Translation = new LanguageData();
                Translation.TranslationTable = new List<TranslationProto>();
                foreach (var pair in dictionary)
                {
                    // TODO make sure that miss matches with name and ID are handled
                    var translationProto = TranslationProto.FromCrowdin(pair.Key, pair.Value);
                    var match = template.TranslationTable.FirstOrDefault(proto => proto.Name == translationProto.Name);
                    if (translationProto.Name == "string" && match != null)
                    {
                        translationProto.Original = match.Original;
                        translationProto.Name = match.Name;
                    }

                    Translation.TranslationTable.Add(translationProto);
                }

                //Translation.UpdateTranslationItems(Settings, template);
                // overwrite if new translation show up
                var dict = ToCrowdinDictionary(Translation.TranslationTable, key, value);
                File.WriteAllText(translationCrowdinFilePath, JSON.ToJson(dict));
            }
        }

        /// <summary>
        ///     Load and/or Dump Legacy format translation
        /// </summary>
        /// <param name="template">Template for new language files</param>
        /// <param name="translationFilePath">Translation file path</param>
        private void LegacyDump(LanguageData template, string translationFilePath)
        {
            if (!File.Exists(translationFilePath))
            {
                File.WriteAllText(translationFilePath, JSON.ToJson(template));
            }
            else
            {
                Translation = JSON.FromJson<LanguageData>(File.ReadAllText(translationFilePath));
                UpdateTranslationItems(Settings, Translation, template);
                // overwrite if new translation show up
                File.WriteAllText(translationFilePath, JSON.ToJson(Translation));
            }
        }
        
        /// <summary>
        ///     Load and/or Dump Plain text format translation
        /// </summary>
        /// <param name="template">Template for new language files</param>
        /// <param name="translationFilePath">Translation file path</param>
        private void PlainTextDump(LanguageData template, string translationFilePath)
        {
            if (Settings.CreateAndUpdateFromPlainTextDumpUnsafe)
            {
                var translationDumpFilePath = Path.Combine(Settings.SettingsDirectory, LanguageData.TranslationDumpFileName);
                if (!File.Exists(translationDumpFilePath))
                {
                    var sb = new StringBuilder();
                    foreach (var table in template.TranslationTable)
                    {
                        sb.AppendLine(table.Original);
                        sb.AppendLine("-----");
                    }

                    File.WriteAllText(translationDumpFilePath, sb.ToString());
                }
                else
                {
                    var lines = File.ReadAllText(translationDumpFilePath);
                    var split = lines.Split(new string[] {"-----"}, StringSplitOptions.None);
                    Translation = new LanguageData {TranslationTable = new List<TranslationProto>()};
                    for (var index = 0; index < template.TranslationTable.Count; index++)
                    {
                        Translation.TranslationTable.Add(new TranslationProto(template.TranslationTable[index], split[index]
                            .Trim('\r', '\n')));
                    }

                    File.WriteAllText(translationFilePath, JSON.ToJson(Translation));
                }
            }
        }

        /// <summary>
        ///     Converts list of translation to Dictionary for Crowdin support
        /// </summary>
        /// <param name="translationProto">Translation list to convert</param>
        /// <param name="key">Key Expression</param>
        /// <param name="value">Value Expression</param>
        /// <returns></returns>
        private Dictionary<string, string> ToCrowdinDictionary(List<TranslationProto> translationProto, Func<TranslationProto, string> key, Func<TranslationProto, string> value)
        {
            var dict = new Dictionary<string, string>();
            foreach (var proto in translationProto)
            {
                if (dict.ContainsKey(key(proto)))
                {
                    Debugger.Break();
                }
                else
                {
                    dict.Add(key(proto), value(proto));
                }
            }

            return dict;
        }
        
        /// <summary>
        ///     Updates target file with new template data using settings
        /// </summary>
        /// <param name="settings">Settings</param>
        /// <param name="targetFile">Target file</param>
        /// <param name="template">Template file</param>
        public void UpdateTranslationItems(LanguageSettings settings, LanguageData targetFile, LanguageData template)
        {
            var missMatchList = new List<TranslationProto>();
            var tempTranslationTable = targetFile.TranslationTable.ToList();
            // Find invalid or missing translations
            foreach (var translationProto in tempTranslationTable)
            {
                var match = template.TranslationTable.FirstOrDefault(proto => proto.Name == translationProto.Name);
                if (match != null)
                {
                    if (match.Original != translationProto.Original)
                    {
                        translationProto.IsValid = false;
                        missMatchList.Add(translationProto);
                        ConsoleLogger.LogWarning($"Translation for {translationProto.Original} -- {translationProto.Translation} is no longer valid! This entry original meaning has changed");
                    }
                    else
                    {
                        translationProto.Original = match.Original;
                        if (translationProto.Original.StartsWith("UI"))
                        {
                            translationProto.Translation = match.Original;
                        }
                    }
                }
                else
                {
                    translationProto.IsValid = false;
                    missMatchList.Add(translationProto);
                    ConsoleLogger.LogWarning($"Translation for {translationProto.Original} -- {translationProto.Translation} is no longer valid! This entry was probably removed");
                }
            }
            // New translations
            foreach (var translationProto in template.TranslationTable)
            {
                var match = targetFile.TranslationTable.FirstOrDefault(proto => proto.Name == translationProto.Name);
                if (match == null)
                {
                    missMatchList.Add(translationProto);
                    tempTranslationTable.Add(translationProto);
                    ConsoleLogger.LogWarning($"New translation entry for {translationProto.Original}  (Upgrade from {settings.GameVersion} to {GameConfig.gameVersion.ToFullString()})");
                }
            }

            targetFile.TranslationTable = tempTranslationTable;
            settings.GameVersion = GameConfig.gameVersion.ToFullString();
        }
    }
}