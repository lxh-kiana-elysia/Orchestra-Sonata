using System.Collections.Generic;

namespace YuetanMingqu
{
    /// <summary>
    /// 异想体数据类（纯数据，不含逻辑）。
    /// 数据来源：Assets/Data/Aberrations.json
    /// 字段规格：Aberrations.md「字段速查」/ FDD §10；规则见 GDD §3.2 / §3.9
    /// 说明：本类只存★静态配置，程序只读不改；运行时状态（如已累积点数）由程序生成并入存档。
    /// 注：JSON 中的 _note 字段是给策划看的备注，本类不声明，JsonUtility 会自动忽略。
    /// </summary>
    [System.Serializable]   // 标记可序列化：JsonUtility 才能读取/写入该类
    public class Aberration
    {
        /// <summary>唯一标识，格式 aber_xxx_NN（如 aber_sun_01）</summary>
        public string id;

        /// <summary>显示名称（如「熄灭的太阳」）</summary>
        public string name;

        /// <summary>类型：创伤型 / 回忆型 / 欲望型 / 工具型</summary>
        public string type;

        /// <summary>危险等级 1-5：1=ZAYIN、2=TETH、3=HE、4=WAW、5=ALEPH；决定派遣门槛与产出系数</summary>
        public int riskLevel;

        /// <summary>情绪值 0-100 的初始值（☆运行时会变化：归零出逃、爆满触发突破事件）</summary>
        public float mood;

        /// <summary>情绪危险窗口下限：低于此值进入危险状态（ZAYIN 级填 0 表示无窗口）</summary>
        public float moodDangerMin;

        /// <summary>情绪危险窗口上限：高于此值进入危险状态（ZAYIN 级填 100 表示无窗口）</summary>
        public float moodDangerMax;

        /// <summary>绑定的同位体角色 id（乐队少女）。仅为叙事彩蛋：不影响数值、不阻塞流程</summary>
        public string linkedCharacterId;

        /// <summary>四种工作的成功率修正，乘数、基准 1.0、禁止负值（如 1.2 = 提升 20%）</summary>
        public List<WorkModifier> workModifiers;

        /// <summary>每次成功工作产出的【情感粒子】（当日能源）基准值；失败约为 25%</summary>
        public int energyPerSuccess;

        /// <summary>每次成功工作产出的【异想体点数】，用于解锁剧情节点与研发装备</summary>
        public int pointsPerSuccess;

        /// <summary>异想体点数解锁剧情的档位（如 0/40/120）</summary>
        public List<int> storyUnlockThresholds;

        /// <summary>出逃触发条件（如「情绪值跌破 10，或连续 3 天未工作」）；ZAYIN 级为 null</summary>
        public string escapeTrigger;

        /// <summary>出逃后的行为与对设施的影响（如全局成功率 -10%）；ZAYIN 级为 null</summary>
        public string escapeBehavior;

        /// <summary>镇压/安抚方式（通常对应「比武」工作）；ZAYIN 级为 null</summary>
        public string suppressMethod;

        /// <summary>背景故事文本，用于图鉴展示</summary>
        public string background;

        /// <summary>专属效果表（如工作结果为「优」时恢复精神），由 WorkSystem 结算后读取执行</summary>
        public List<SpecialEffect> specialEffects;

        /// <summary>最早出现天数：抽取时先按 minDay ≤ 当前天数 过滤，默认 1</summary>
        public int minDay;

        /// <summary>池内抽取权重；0 = 不进池（仅固定/剧情出现，如 Day1 教学异想体），默认 100</summary>
        public int weight;
    }

    /// <summary>
    /// 异想体数据库（壳类）。
    /// 作用：JsonUtility 不支持解析顶层数组，故用本类包住 aberrations 列表，
    /// 与 Aberrations.json 最外层的 {"aberrations":[...]} 一一对应。
    /// </summary>
    [System.Serializable]
    public class AberrationDatabase
    {
        /// <summary>异想体列表，键名必须与 JSON 的 aberrations 一致</summary>
        public List<Aberration> aberrations;
    }

    /// <summary>
    /// 单条工作修正，对应 JSON 中 workModifiers 数组的一个元素。
    /// </summary>
    [System.Serializable]
    public class WorkModifier
    {
        /// <summary>工作类型：Performance 演奏 / Talk 谈话 / Creation 创作 / Combat 比武</summary>
        public string workType;

        /// <summary>乘数修正，基准 1.0（&gt;1 提升、&lt;1 降低），禁止负值</summary>
        public float modifier;
    }

    /// <summary>
    /// 异想体专属效果，对应 JSON 中 specialEffects 数组的一个元素。
    /// 由 WorkSystem 在工作结算后读取并执行。
    /// </summary>
    [System.Serializable]
    public class SpecialEffect
    {
        /// <summary>触发时机：workResultExcellent（结果为优）/ fullOutput（本次满产出）</summary>
        public string trigger;

        /// <summary>效果类型：restoreSp 恢复精神 / restoreHp 恢复血量</summary>
        public string effect;

        /// <summary>作用对象：worker（执行工作的员工）/ department（该部门所有员工）</summary>
        public string target;

        /// <summary>效果数值（占位，待实测调参）</summary>
        public int value;
    }
}
