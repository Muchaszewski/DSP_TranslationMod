using System;

namespace TranslationCommon
{
    [Serializable]
    public class TranslationProto
    {
        public bool IsValid;
            
        public string Name;
        public int ID;
        
        public string Original;
        public string Translation;
        
        public string GetTranslation()
        {
            return Translation;
        }

        public TranslationProto(TranslationProto proto, string translation)
        {
            Name = proto.Name;
            ID = proto.ID;
            Original = proto.Original;
            Translation = translation;
        }

        public static TranslationProto FromCrowdin(string name, string translation)
        {
            var split = name.Split('_');
            TranslationProto result = new TranslationProto();
            result.Name = split[0];
            result.ID = int.Parse(split[1]);
            result.Translation = translation;
            return result;
        }
        
        public TranslationProto(string name, int id, string original, string translation)
        {
            Name = name;
            ID = id;
            Original = original;
            Translation = translation;
        }

        public TranslationProto()
        {
            
        }
    }
}