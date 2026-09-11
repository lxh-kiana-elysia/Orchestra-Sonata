using UnityEngine;

namespace YuetanMingqu
{
    /// <summary>
    /// 数据管理器：加载并缓存四份 JSON，提供按 id 查询。
    /// 数据来源：Assets/Data/ 下的 Characters.json / BandMembers.json / Aberrations.json / GameConfig.json
    /// 设计说明（ARCHITECTURE §5.1）：所有系统经由此处取数据，不直接读文件，便于日后换成 Addressables。
    /// 为什么是静态类：数据入口需全局可访问且不依赖场景物体，故不继承 MonoBehaviour，
    /// 也不挂 Inspector 引用（文件引用由调用方传入，见 DataLoadDemo）。
    /// </summary>
    public static class DataManager
    {
        /// <summary>普通员工库（Characters.json）。键 characters；未加载时为 null</summary>
        public static CharacterDatabase Characters { get; private set; }

        /// <summary>乐队少女库（BandMembers.json）。键 bandMembers；未加载时为 null</summary>
        public static BandMembersDatabase BandMembers { get; private set; }

        /// <summary>异想体库（Aberrations.json）。键 aberrations；未加载时为 null</summary>
        public static AberrationDatabase Aberrations { get; private set; }

        /// <summary>全局配置（GameConfig.json）。顶层即对象，无需壳类</summary>
        public static GameConfig Config { get; private set; }

        /// <summary>
        /// 解析四份 JSON 并缓存到上面的静态属性。
        /// </summary>
        /// <param name="bandMemberJson">BandMembers.json</param>
        /// <param name="aberrationsJson">Aberrations.json</param>
        /// <param name="charactersJson">Characters.json</param>
        /// <param name="configJson">GameConfig.json</param>
        /// <returns>全部解析成功返回 true；任一为空返回 false 并输出错误日志</returns>
        /// <remarks>
        /// 用 TextAsset.text 而非文件路径：打包后 Assets 路径失效，TextAsset 会随包体一起发布。
        /// 判空检查到列表层：键名写错时壳类仍会创建、但内部列表为 null，只查壳类会漏掉这种情况。
        /// </remarks>
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

        /// <summary>
        /// 按 id 查询乐队少女。
        /// </summary>
        /// <param name="id">乐队少女 id，如 band_ppp_kasumi</param>
        /// <returns>找到返回该条数据；未找到或未加载返回 null（调用方需判空）</returns>
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

        /// <summary>
        /// 按 id 查询异想体。
        /// </summary>
        /// <param name="id">异想体 id，如 aber_sun_01</param>
        /// <returns>找到返回该条数据；未找到或未加载返回 null（调用方需判空）</returns>
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

        /// <summary>
        /// 按 id 查询普通员工。
        /// </summary>
        /// <param name="id">员工 id，如 staff_sample_01</param>
        /// <returns>找到返回该条数据；未找到或未加载返回 null（调用方需判空）</returns>
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
