using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using SimpleJSON;
using UnityEngine;

namespace TranslationCommon
{
    public class LanguageContainer
    {
        public LanguageSettings Settings;
        public LanguageData Translation;

        public LanguageContainer(LanguageSettings settings)
        {
            Settings = settings;
        }

        public void LoadTranslation(LanguageData template)
        {
            var translationFilePath = Path.Combine(Settings.SettingsDirectory, LanguageData.TranslationFileName);
            PlainTextDump(template, translationFilePath);

            if (Settings.ImportFromLegacy)
            {
                LegacyDump(template, translationFilePath);
            }
            else
            {
                CrowdinDump(template);
            }
        }

        private void CrowdinDump(LanguageData template)
        {
            Func<TranslationProto, string> key = proto => $"{proto.Name}_{proto.ID}";
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

        private void LegacyDump(LanguageData template, string translationFilePath)
        {
            if (!File.Exists(translationFilePath))
            {
                File.WriteAllText(translationFilePath, JSON.ToJson(template));
            }
            else
            {
                Translation = JSON.FromJson<LanguageData>(File.ReadAllText(translationFilePath));
                Translation.UpdateTranslationItems(Settings, template);
                // overwrite if new translation show up
                File.WriteAllText(translationFilePath, JSON.ToJson(Translation));
            }
        }

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
    }
}