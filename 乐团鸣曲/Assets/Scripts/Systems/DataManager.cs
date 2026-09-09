using UnityEngine;

namespace YuetanMingqu
{
    public static class DataManager
    {
        public static BandMembersDatabase BandMembers { get; private set; }
        public static AberrationDatabase Aberrations { get; private set; }
        public static CharacterDatabase Characters { get; private set; }
        public static GameConfig Config { get; private set; }
        public static bool LoadAll(TextAsset bandMemberJson, TextAsset aberrationsJson,
                                   TextAsset charactersJson, TextAsset configJson)
        {
            BandMembers = JsonUtility.FromJson<BandMembersDatabase>(bandMemberJson.text);
            Aberrations = JsonUtility.FromJson<AberrationDatabase>(aberrationsJson.text);
            Characters = JsonUtility.FromJson<CharacterDatabase>(charactersJson.text);
            Config = JsonUtility.FromJson<GameConfig>(configJson.text);
            if (BandMembers == null || Aberrations == null || Characters == null || Config == null
                ||BandMembers.bandMembers == null || Aberrations.aberrations == null ||Characters.characters==null)
            {
                Debug.LogError("存在空数据集");
                return false;
            }
            else
            {
                return true;
            }
        }

        public static BandMember GetBandMember(string id)
        {
            if (BandMembers == null) return null;

            foreach (BandMember member in BandMembers.bandMembers)
            {
                if(member.id == id)
                    return member;
            }
            return null;

            //return BandMembers.bandMembers.Find(m=>m.id==id);
        }

        public static Aberration GetAberration(string id)
        {
            if (Aberrations == null) return null;

            foreach (Aberration aberration in Aberrations.aberrations)
            {
                if (aberration.id == id)
                    return aberration;
            }
            return null;

            //return Aberrations.aberrations.Find(m => m.id == id);
        }

        public static Character GetCharacter(string id)
        {
            if (Characters == null) return null;

            foreach (Character character in Characters.characters)
            {
                if (character.id == id)
                    return character;
            }
            return null;

            //return Characters.characters.Find(m => m.id == id);
        }
    }
}

