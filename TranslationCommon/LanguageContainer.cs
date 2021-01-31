using System;
using System.Collections.Generic;
using System.IO;
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
                    var split = lines.Split(new string[]{"-----"}, StringSplitOptions.None);
                    Translation = new LanguageData {TranslationTable = new List<TranslationProto>()};
                    for (var index = 0; index < template.TranslationTable.Count; index++)
                    {
                        Translation.TranslationTable.Add(new TranslationProto(template.TranslationTable[index], split[index]
                            .Trim('\r', '\n')));
                    }
                    File.WriteAllText(translationFilePath, JSON.ToJson(Translation, true));
                }
            }

            if (!File.Exists(translationFilePath))
            {
                File.WriteAllText(translationFilePath, JSON.ToJson(template, true));
            }
            else
            {
                Translation = JSON.FromJson<LanguageData>(File.ReadAllText(translationFilePath));
                Translation.UpdateTranslationItems(Settings, template);
                // overwrite if new translation show up
                File.WriteAllText(translationFilePath, JSON.ToJson(Translation, true));

            }
        }
    }
}