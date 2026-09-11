using System.Collections.Generic;

namespace YuetanMingqu
{
    /// <summary>
    /// 普通员工数据类（纯数据，不含逻辑）。
    /// 数据来源：Assets/Data/Characters.json；规则见 GDD §3.3 / FDD §4.2
    /// 与 BandMember（乐队少女）的区别：可招录、可捏人改外观、无个人线、无专属装备；死亡则永久失去。
    /// 说明：本类只存★静态配置；当前四维、今日工作次数、崩溃/死亡状态等由程序生成并入存档。
    /// 注：普通员工通常不写死名单，而是由招录系统随机生成；当前文件中的条目用于验证数据读取。
    /// </summary>
    [System.Serializable]   // 标记可序列化：JsonUtility 才能读取/写入该类
    public class Character
    {
        /// <summary>唯一标识（如 staff_sample_01）</summary>
        public string id;

        /// <summary>显示名称（招录时可自由修改）</summary>
        public string name;

        /// <summary>血量初始值（默认 10）；判定「比武」工作的属性；归零则死亡</summary>
        public int hp;

        /// <summary>精神初始值（默认 10）；判定「谈话」工作的属性；过低时无法工作</summary>
        public int sp;

        /// <summary>演奏水平初始值（默认 10）；判定「演奏」工作的属性</summary>
        public int performance;

        /// <summary>共感初始值（默认 10）；判定「创作」工作的属性</summary>
        public int empathy;

        /// <summary>捏人外观：发型（普通员工可改；乐队少女无此字段）</summary>
        public string hairStyle;

        /// <summary>捏人外观：发色</summary>
        public string hairColor;

        /// <summary>捏人外观：眼睛样式</summary>
        public string eyeStyle;

        /// <summary>捏人外观：表情</summary>
        public string expression;

        /// <summary>恢复速率（草案 5.0/单位时间）；回复室设施可提升</summary>
        public float recoveryRate;
    }

    /// <summary>
    /// 普通员工数据库（壳类）。
    /// 作用：JsonUtility 不支持解析顶层数组，故用本类包住 characters 列表，
    /// 与 Characters.json 最外层的 {"characters":[...]} 一一对应。
    /// </summary>
    [System.Serializable]
    public class CharacterDatabase
    {
        /// <summary>普通员工列表，键名必须与 JSON 的 characters 一致（注意是复数）</summary>
        public List<Character> characters;
    }
}
