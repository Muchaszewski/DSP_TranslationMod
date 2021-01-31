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