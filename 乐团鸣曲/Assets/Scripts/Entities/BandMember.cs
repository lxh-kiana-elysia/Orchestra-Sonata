using System.Collections.Generic;

namespace YuetanMingqu
{
    /// <summary>
    /// 乐队少女数据类（纯数据，不含逻辑）。
    /// 数据来源：Assets/Data/BandMembers.json
    /// 字段规格：Band_Members.md「字段说明」；两套数据库分离见 GDD §3.3（v1.5）
    /// 与 Character（普通员工）的区别：有专属装备与个人线、不可改外观、不会死亡（崩溃仅退场数日）。
    /// 说明：本类只存★静态配置；运行时状态（个人线进度、已解锁剧情、当前四维、崩溃状态）由程序生成并入存档。
    /// </summary>
    [System.Serializable]   // 标记可序列化：JsonUtility 才能读取/写入该类
    public class BandMember
    {
        /// <summary>唯一标识，格式 band_&lt;乐队缩写&gt;_&lt;角色&gt;（如 band_ppp_kasumi；PPP=Poppin'Party、mygo=MyGO!!!!!）</summary>
        public string id;

        /// <summary>显示名称（原著角色名）</summary>
        public string name;

        /// <summary>所属乐队（如 Poppin'Party、MyGO!!!!!）</summary>
        public string band;

        /// <summary>血量初始值（默认 10）；判定「比武」工作的属性；归零则崩溃</summary>
        public int hp;

        /// <summary>精神初始值（默认 10）；判定「谈话」工作的属性；过低时无法工作</summary>
        public int sp;

        /// <summary>演奏水平初始值（默认 10）；判定「演奏」工作的属性</summary>
        public int performance;

        /// <summary>共感初始值（默认 10）；判定「创作」工作的属性</summary>
        public int empathy;

        /// <summary>专属技能描述文案（纯文本，程序不据此计算；技能生效需另有效果数据结构）</summary>
        public string skill;

        /// <summary>恢复速率（草案 5.0/单位时间）；回复室设施可提升</summary>
        public float recoveryRate;

        /// <summary>加入条件：完成某任务 / 某异想体点数达档位。角色解锁只看此字段</summary>
        public string unlockCondition;

        /// <summary>专属装备（E.G.O 式）：加入时自动获得，不可转让、无需研发</summary>
        public Equipment equipment;

        /// <summary>绑定的同位体异想体 id（如 aber_sun_01）；仅为叙事彩蛋，不影响流程</summary>
        public string linkedAberrationId;
    }

    /// <summary>
    /// 乐队少女数据库（壳类）。
    /// 作用：JsonUtility 不支持解析顶层数组，故用本类包住 bandMembers 列表，
    /// 与 BandMembers.json 最外层的 {"bandMembers":[...]} 一一对应。
    /// </summary>
    [System.Serializable]
    public class BandMembersDatabase
    {
        /// <summary>乐队少女列表，键名必须与 JSON 的 bandMembers 一致</summary>
        public List<BandMember> bandMembers;
    }

    /// <summary>
    /// 装备槽引用（不是装备的完整数据）。
    /// 这里只存装备 id，装备的名称/属性/研发点数等完整数据见 Equipments.json（Equipments.md）。
    /// 用途：乐队少女加入时自动获得并绑定这一套；普通员工死亡时装备掉落消失。
    /// </summary>
    [System.Serializable]
    public class Equipment
    {
        /// <summary>武器装备 id（如 ego_weapon_sun_01）</summary>
        public string weapon;

        /// <summary>护甲装备 id（如 ego_armor_sun_01）</summary>
        public string armor;

        /// <summary>饰品装备 id（如 ego_gift_sun_01）</summary>
        public string accessory;
    }
}
