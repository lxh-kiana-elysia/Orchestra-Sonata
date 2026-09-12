# ARCHITECTURE.md — 乐团鸣曲 系统架构与开发指南

> 文档版本：v1.4（2026-09-12）
> 上游依据：`GDD.docx` **v1.9**（设计宪法） · `agent.md` **v3.5**（AI 执行手册）
> 定位：面向开发者的**技术架构说明**——系统怎么分层、模块怎么交互、接口长什么样、数据怎么流动。
>
> ⚠️ **当前实现状态**：Unity 工程为 **URP 2D 空模板**（`Assets/` 下只有 `Scenes/`、`Settings/` 与默认配置），**尚无游戏脚本**。本文描述的是**目标架构**，需按 §7 开发顺序逐步实现。

---

## 1. 项目概述

### 1.1 游戏定位

| 项 | 值 |
|---|---|
| 游戏名 | 乐团鸣曲 |
| 类型 | 叙事驱动管理模拟 × 视觉小说 |
| 框架来源 | 脑叶式"收容-工作-出逃-考验"结构 × BanG Dream 同人叙事 |
| 玩家身份 | 旁观引导管理者（随演出被卷入的 Ring 公司新人） |
| 平台 | Steam（Windows），免费、无内购无广告 |
| 引擎 | Unity 6.0.38f1（URP 2D 17.0.3 / Input System / uGUI） |
| 叙事 | Yarn Spinner + 自研 `DialogueTrigger` 状态层 |
| 首版范围 | 前 2 支乐队 / 前 2 章 / 2 条个人线，约 15-20 天通关 |
| 完整版愿景 | 10 支乐队 ≈50 天（3 层级：上层 4 / 中层 3 / 下层 3） |

### 1.2 一句话玩法

每天开工前"编成"员工 → 点「开始这一天」→ 在部门场景中**实时操控**员工对异想体执行 4 种工作（演奏/谈话/创作/比武）→ 收集情感粒子达标 → 结算推进下一天；期间异想体可能因情绪值出逃，需指挥镇压；累计推进乐队考验，最终在第 50 天走向真/假结局。

### 1.3 关键设计约束（影响架构，实现时不可违背）

- **结构同构脑叶**：乐队节点 ↔ 脑叶部门；乐队考验 ↔ 核心抑制；记忆日 ↔ 记忆库；真/假结局 ↔ 多结局。
- **数值节奏**：四维 1-120（I~EX 六档）；一局 50 天；记忆日 = Day 5/10/…/45；真结局窗口 Day46。
- **无永久损失**：无永久 Game Over，角色/剧情不永久锁死；失败转为剧情分支。
- **数据驱动**：一切数值走 JSON/SO，禁硬编码（便于策划调参与测试）。

---

## 2. 核心系统架构

### 2.1 分层架构

```
┌─────────────────────────────────────────────────┐
│  UI 层（Screens / Panels / Widgets）             │  只做显示与输入转发
│  MainMenu · TreeOverview · Deployment ·          │  不含玩法规则
│  Department · Codex · Pause · Settings · Loading │
└───────────────┬─────────────────────────────────┘
                │ 事件订阅 / 指令调用
┌───────────────▼─────────────────────────────────┐
│  Systems 层（玩法系统）                           │  唯一规则计算处
│  GameManager(单例调度) · DataManager · EventManager│  通过事件解耦
│  WorkSystem · TrialSystem · SaveSystem           │
│  RecruitmentSystem · TrainingSystem              │
└───────────────┬─────────────────────────────────┘
                │ 读取 / 写入
┌───────────────▼─────────────────────────────────┐
│  Entities 层（纯数据）                            │  无逻辑
│  Character · Aberration · WorkResult ·           │  可序列化
│  Enums · GameConfig                              │
└───────────────┬─────────────────────────────────┘
                │ 序列化
┌───────────────▼─────────────────────────────────┐
│  数据层：Assets/Data/*.json（BandMembers 独立）+  ScriptableObject │
│  Characters · Aberrations · GameConfig · Dialogues│
└─────────────────────────────────────────────────┘

旁挂：Narrative 层（DialogueTrigger 状态层 + DialogueManager/Yarn）
      —— 只负责剧情触发与播放，不改数值
```

### 2.2 游戏状态机（GameManager 统一调度）

```
BOOT → MAIN_MENU(L0) → LOADING → CUTSCENE（每日剧情）
      → TREE_OVERVIEW(L1 生命树·部门开放)
      → LOADING → DEPLOY(L2 编成·部署员工)
      → DEPARTMENT(L3)
            ├─ BATTLE（出逃/镇压）
            ├─ CUTSCENE（节点剧情）
            └─ PAUSE / SETTINGS / CODEX（覆盖层）
      → DAY_END（结算）→ 次日 CUTSCENE → 回 TREE_OVERVIEW（下一天）
      → ENDING（Day50 真/假结局）
```

> **流程顺序（2026-09-12 修订）**：生命树（L1）在编成（L2）**之前**——每天开始先过剧情 → 生命树选「开放哪个部门 + 今日异想体收容进哪个部门」→ 加载 → 编成部署员工 → 「开始这一天」进入 L3。层级编号与实际顺序一致。

**约束**：状态切换只由 `GameManager` 发起，UI 与各 System 不得自行 `LoadScene`。

### 2.3 模块清单

| 模块 | 类型 | 职责 |
|---|---|---|
| `GameManager` | 单例 | 全局状态机、天数推进、记忆日锚定/回滚、结局判定 |
| `DataManager` | 系统 | 加载/缓存 JSON 与 SO，提供按 ID 查询 |
| `EventManager` | 系统 | 发布/订阅，跨系统解耦 |
| `UIManager` | 系统 | 屏幕（Screen）栈管理、面板开关 |
| `CameraController` | 系统 | 部门场景正交相机：平移（WASD/中键拖动）、滚轮缩放（0.5x~2.0x）、边界钳制、平滑插值（FDD-12） |
| `WorkSystem` | 系统 | 工作判定、进度、产出结算、成长计算、收益递减 |
| `TrialSystem` | 系统 | 乐队考验开启/完成判定（与乐队解锁解耦） |
| `SaveSystem` | 系统 | 存档读写、记忆日快照与回滚 |
| `RecruitmentSystem` | 系统 | 普通员工候选生成、捏人、录用、槽位校验 |
| `TrainingSystem` | 系统 | 星石训练加点、费用计算 |
| `Character` / `Aberration` / `WorkResult` | 数据 | 纯数据类 |
| `GameConfig` | 数据 | 全局配置（含占位值） |
| `DialogueTrigger` / `DialogueManager` | 叙事 | 剧情触发与播放 |

---

## 3. 模块职责与交互关系

### 3.1 交互原则

1. **单向依赖**：UI → Systems → Entities（反之禁止）。
2. **事件解耦**：Systems 之间不直接持有引用，通过 `EventManager` 通信。
3. **UI 无规则**：UI 只发"意图"事件（如 `RequestStartWork`），由 System 计算后发"结果"事件（如 `WorkCompleted`），UI 订阅后刷新。

### 3.2 典型交互：一次工作的完整链路

```
[UI] DepartmentScreen（玩家选中员工 → 点击异想体 → 选工作）
   │  发布 RequestStartWork(employeeId, aberrationId, workType)
   ▼
[WorkSystem] 校验（员工可用? 槽位? 门槛?）
   │  计算最终成功率（纯函数）
   │  生成工作进度任务（随时间推进）
   ▼
[WorkSystem] 进度完成 → 判定成功/失败
   │  计算产出（情感粒子 / 异想体点数 / 成长值 / 消耗）
   │  更新 Character / Aberration 数据
   │  发布 WorkCompleted(WorkResult)
   ▼
[UI] 订阅 → 播放结果反馈（≤5 秒短反馈）或节点剧情
[Narrative] 若命中剧情触发条件 → 播放完整剧情
[GameManager] 若当日能源达标 → 允许结束当天
```

### 3.3 事件清单（建议命名）

| 事件 | 发布者 | 订阅者 | 载荷 |
|---|---|---|---|
| `GameStateChanged` | GameManager | UI / Systems | 新状态 |
| `DayStarted` / `DayEnded` | GameManager | 全部 | 天数 |
| `WorkRequested` | UI | WorkSystem | 员工/异想体/工作类型 |
| `WorkCompleted` | WorkSystem | UI / Narrative | `WorkResult` |
| `AberrationMoodChanged` | WorkSystem | UI / GameManager | 异想体 ID、新情绪值 |
| `AberrationEscaped` | WorkSystem | UI / GameManager | 异想体 ID |
| `AberrationSuppressed` | WorkSystem | UI | 异想体 ID |
| `TrialUnlocked` / `TrialCompleted` | TrialSystem | UI / GameManager | 乐队 ID |
| `MemoryDayAnchored` | SaveSystem | UI | 天数 |
| `ResourceChanged` | GameManager | UI | 资源类型、数量 |

---

## 4. 关键接口定义

> 以下为**目标接口规范**（C#，Unity 风格）。工程当前为空，实现时需与人类确认命名细节。

### 4.1 数据类（Entities）

```csharp
// 员工类型与工作类型
public enum EmployeeType { Normal, Band }
public enum WorkType { Performance, Talk, Creation, Combat }   // 演奏/谈话/创作/比武
public enum RiskLevel { ZAYIN, TETH, HE, WAW, ALEPH }

// 角色（普通员工 + 乐队少女共用）
// 字段分两类：★=静态配置（策划填 JSON）；☆=运行时状态（程序生成/存档，不写在配置里）
[System.Serializable]
public class Character {
    // ★ 静态配置
    public string id;
    public string name;
    public EmployeeType employeeType;
    public string band;                  // 普通员工为空
    public Dictionary<string,string> appearance;  // 普通员工可捏人；乐队少女 null
    public string skill;
    public float recoveryRate = 5.0f;    // 每单位时间恢复速度（占位）

    // ☆ 运行时状态（初始化时取配置初始值，之后随游戏变化并存入存档）
    public int hp, sp;                   // 当前值（初始 10）
    public int hpMax, spMax;
    public int performance, empathy;     // 演奏水平 / 共感（初始 10）
    public int todayWorkCount;           // 当日派遣次数（用于收益递减）
    public List<string> unlockedStoryNodes;
    public float personalLineProgress;
    public bool isCollapsed;             // 崩溃/死亡状态
}

> **配置与运行时分离原则**：`BandMembers.json` / `Characters.json` 只存 ★ 静态配置与四维**初始值**；
> 运行时状态（当前四维、剧情进度、个人线、崩溃状态、装备槽位）由程序在运行时生成并写入存档（见 FDD-06）。
> 角色的**解锁只看 `unlockCondition`**（完成特定任务 / 异想体点数达档位），与运行时进度无关。

// 异想体
// 字段分两类：★=静态配置（策划填 JSON）；☆=运行时状态（程序生成/存档）
[System.Serializable]
public class Aberration {
    // ★ 静态配置
    public string id, name, type;
    public RiskLevel riskLevel;
    public float mood;                   // 初始情绪值 0-100
    public float moodDangerMin, moodDangerMax;
    public string linkedCharacterId;
    public Dictionary<WorkType,float> workModifiers;  // 工作偏好修正（乘数，1.0 基准）
    public string escapeTrigger, escapeBehavior, suppressMethod;
    public int energyPerSuccess;         // 每次成功产出的【情感粒子】基准值（失败约 25%）
    public int pointsPerSuccess;         // 每次成功产出的【异想体点数】
    public int[] storyUnlockThresholds;  // 异想体点数解锁剧情的档位
    public string background;
    public int minDay = 1;               // 最早出现天数（默认 1，5 种难度全程可出现）
    public int weight = 100;             // 池内权重（0 = 不进池）

    // ☆ 运行时状态（存入存档）
    public float accumulatedPoints;      // 该异想体已累积的异想体点数（解锁剧情 / 研发装备用）
}
}

// 工作结果
[System.Serializable]
public class WorkResult {
    public bool success;
    public int energyGained;             // 情感粒子
    public int aberrationPointsGained;
    public int hpCost, spCost;
    public int growth;                   // 对应属性成长
    public string feedbackTextKey;       // 短反馈文本
    public string storyNodeId;           // 命中节点剧情时非空
}
```

### 4.2 判定与结算（**纯函数，必须可单测**）

```csharp
public static class WorkRules {
    /// 最终成功率 = min(1, 基础成功率 × (判定属性值/120) × 异想体修正)
    public static float CalcSuccessRate(float baseRate, int statValue, float aberrationModifier);

    /// 判定属性由工作类型决定（演奏→演奏水平 / 谈话→精神 / 创作→共感 / 比武→血量）
    public static int GetJudgeStat(Character c, WorkType type);

    /// 收益递减：同日对同一异想体第 n 次工作的产出系数
    public static float CalcDiminishingFactor(int todayWorkCount);
}
```

### 4.3 系统接口

```csharp
// 数据加载
public interface IDataProvider {
    T Load<T>(string path);
    Character GetCharacter(string id);
    Aberration GetAberration(string id);
    GameConfig Config { get; }
}

// 工作系统
public interface IWorkSystem {
    bool CanStart(string employeeId, string aberrationId, WorkType type, out string reason);
    void StartWork(string employeeId, string aberrationId, WorkType type);
    void CancelWork(string employeeId);          // 中断：无进度收益
    // 进度推进由 Update/Timer 驱动，完成时发布 WorkCompleted
}

// 存档（含记忆日）
public interface ISaveSystem {
    void Save(int day);
    bool Load(out int day);
    void AnchorMemoryDay(int day);               // 记忆日锚定快照
    bool RollbackToMemoryDay(out int targetDay); // 回滚到最近记忆日
}

// 考验
public interface ITrialSystem {
    bool IsUnlocked(string bandId);
    bool TryStart(string bandId);
    bool IsComplete(string bandId);
    bool AreAllTrialsComplete(int currentDay);   // Day46 前完成 → 真结局
}
```

### 4.4 配置（GameConfig.json 结构示意）

> **命名规范（强制）**：所有 JSON 配置键与数据类字段一律用 **camelCase**（首词小写、后续词首字母大写），
> 例：`quotaBase`、`memoryDays`、`sourceAberrationId`。
> **禁止 snake_case**（`quota_base`）——Unity `JsonUtility` 要求键名与 C# 字段名**逐字符一致**，
> 不一致时**不报错、值静默为 0**，是极难排查的坑。

```jsonc
{
  "version": 1,
  "dayLoop": {
    "finalDay": 50,
    "trueEndingDeadlineDay": 46,
    "memoryDays": [5,10,15,20,25,30,35,40,45],
    "quotaBase": 300,              // TODO(设计待确认): 关联 GDD §3.12
    "quotaGrowthPer10Days": 2.0,
    "trialDayQuotaMultiplier": 1.3 // 考验日上调 30%（已定稿）
  },
  "aberrationPool": {
    "candidatesBase": 3, "candidatesPerDays": 3, "candidatesMax": 5
  },
  "training": {
    "costByTargetLevel": [10,30,100,300,800]   // I→II / II→III / III→IV / IV→V / V→EX 星石（草案）
  },
  "recruitment": { "cost": 50, "slotsByFacilityLevel": [2,4,6,8] },
  "save": { "directoryName": "ResonanceShelter" }
}
```

> 说明：训练为**按级提升**（GDD §2），故不再有 `gainPerTrain`（旧的"每次 +N 点"已废弃）。
> 所有占位值须带 `// TODO(设计待确认): 关联 GDD §X.X` 注释（见 agent.md §4.2 / §10）。

---

## 5. 数据流说明

### 5.1 启动与数据加载

```
游戏启动 → BOOT
  → DataManager 读取 Assets/Data/*.json
      • BandMembers.json（乐队少女，独立库：专属装备、固定形象、任务加入）
      • Characters.json（普通员工：每日招募、可捏人、无专属装备）
      • Aberrations.json / Quests.json / GameConfig.json
  → 反序列化为 Character[] / Aberration[] / GameConfig，缓存到内存（按 ID 索引）
  → GameManager 进入 MAIN_MENU
```
**要点**：所有系统通过 `DataManager` 按 ID 取数据，**不直接读文件**（便于替换 Addressables 与缓存）。

### 5.2 一天的运行时数据流

```
① 部门开放与收容分配（TREE_OVERVIEW，L1，每日必经）
   每日剧情结束 → 进入生命树
   → 若到达解锁天数且有开放机会：点选 LOCKED 节点开放 → F5 确认弹窗 → OPENING → OPEN
   → 当日抽到的异想体（候选池 3+天数/3，上限 5，选 1；未选回池）在此决定收容进哪个已开放且未满的部门
     （每部门异想体上限 4，[GDD §3.1 四 草案]）
   → 5 的倍数天不抽新异想体，跳过分配直接继续
   → 点「继续」→ 加载 → 进入 L2 编成

② 编成（DEPLOY，L2）
   从可用员工（普通员工 + 已解锁乐队少女）中选当日阵容；可在此训练（消耗星石）
   → 点「开始这一天」→ 进入 L3 部门场景（加载该部门员工与异想体实例）

③ 工作循环（DEPARTMENT，实时）
   选中员工 → 移动（寻路）→ 对异想体下达工作
   → WorkSystem 校验 → 进度条推进（可暂停/变速）
   → 完成 → 纯函数判定 → 生成 WorkResult
   → 更新：情感粒子 += / 异想体点数 += / 属性成长 += / 血精按消耗 -
   → 发布 WorkCompleted → UI 播短反馈；命中条件则 Narrative 播节点剧情
   → 异想体情绪值更新：≤0 → 出逃事件；=100 → 突破事件（正向解锁个人线）

④ 日终结算（DAY_END）
   能源达标 → 允许结束当天
   → 结余情感粒子按比率转星石；员工属性按当日工作结算成长
   → 若当日为记忆日 → SaveSystem 锚定快照
   → 天数 +1 → 回到 ①
```

### 5.3 存档与回滚数据流

```
常规存档：GameManager 汇总（天数 / 资源 / 角色 / 异想体 / 剧情进度 / 编成）
   → SaveSystem 序列化为 JSON → 写 persistentDataPath/ResonanceShelter/Saves/

记忆日锚定：记忆日（Day 5/10/…/45）当天打开编成界面时
   → SaveSystem 另存一份快照 snapshot_dayN.json

回滚：玩家在暂停菜单选"回到记忆日"
   → SaveSystem 读取最近快照 → 恢复资源/角色/异想体/已解锁乐队
   → 注意：回滚会移除快照之后新招录的员工（见 GDD §3.12）
```

### 5.4 结局判定数据流

```
Day 推进到 46 → GameManager 检查 TrialSystem.AreAllTrialsComplete()
   ├─ 全部完成 → 标记 trueEndingUnlocked = true（"归途之音充满"）
   └─ 未完成   → 保持 false
Day 50 → 结算结局：trueEndingUnlocked ? 真结局 : 假结局
```

---

## 6. 开发注意事项

### 6.1 规则类（来自 GDD，违反复工）

- 四维初始值全 10；4 种工作与四维一一映射，不可改。
- 判定公式固定：`min(1, 基础成功率 × (判定属性值/120) × 异想体修正)`。
- 成功/失败**都成长**（失败量较小）；成长量 ∝ 消耗量级。
- 同日对同一异想体重复工作**收益递减**（不禁止派遣，只降产出）。
- 情绪值：归零 → 出逃；爆满(100) → 突破事件（正向）。**无永久锁死**。
- 记忆日 = Day 5/10/…/45；真结局截止 Day46；结局日 Day50（记忆日不含 50）。
- 剧情分级：日常短反馈（≤5 秒）vs 节点完整演出（第 1/3/5/10 次、点数跨档、角色组合、章节/考验）。

### 6.2 工程类（来自 agent.md v3.5）

| 项 | 要求 |
|---|---|
| 数据驱动 | 数值全部走 `GameConfig.json`/数据表，**禁硬编码** |
| 占位值 | 带 `// TODO(设计待确认): 关联 GDD §X.X`，不得自行填值 |
| 分层 | Entities 无逻辑 / UI 无玩法规则 / Systems 事件解耦 |
| 场景切换 | 只由 GameManager 发起，不散落 `LoadScene` |
| 性能 | 目标 60fps（最低 30）；Update 禁 `Find`/`GetComponent`；提示用对象池；资源用 Addressables 延迟加载 |
| 单类规模 | ≤300 行推荐；复杂系统可放宽 500 行，需注释说明 |
| 测试 | 纯逻辑（判定/结算/成长/递减/回滚）抽成不依赖 MonoBehaviour 的纯函数并单测；每系统附 `Test_<System>.unity` |
| 命名 | 类/方法 PascalCase；私有字段 `_camelCase`；中文注释 |

### 6.3 协作与流程

- AI 只写代码（框架/功能/审查优化），**不参与设计决策**；设计歧义按提问模板停下确认。
- Git：AI 可 `add`/写 commit 信息，**绝不 push / 回滚 / 删分支**（由人类执行）。
- 交付须验收：功能在 Unity 中可实际运行、无编译错误、数值走配置（agent.md §3.1 ⑤）。
- 合规：不使用 BanG Dream 官方素材与脑叶公司角色/美术；AI 生成素材需自查（GDD 第 7 章）。
- **美术边界（已定稿）**：本项目**美术风格为原创方向，不基于脑叶**——不复用其配色、氛围与机械/工业视觉语言。脑叶界面参考图（F1-F14）**仅用于界面布局、信息架构与交互流程的参照**，不得照搬视觉表现（GDD §5.2 / §3.13）。程序实现结构与交互即可，视觉资产按本作原创方向产出。

### 6.4 当前待办（按优先级）

1. **Phase 0**：建目录结构 + 四大 Manager 骨架 + JSON 加载 Demo + L0 主菜单/加载图/设置。
2. **Phase 1**：数据层完备 + 存档（记忆日）+ 招录/训练。
3. **Phase 2**：日循环 + 生命树总览 + 部门场景 + 实时操控。
4. **Phase 3**：Yarn 剧情接入 + 剧情分级。
5. **Phase 4-5**：打磨、测试、Steam 页与合规。

---

## 7. 相关文档

| 文档 | 作用 |
|---|---|
| `GDD.docx` v1.9 | **设计宪法**：玩法规则、数值、流程、UI 分层、视觉规范 |
| `agent.md` v3.5 | **AI 执行手册**：职责边界、工作流程、代码标准、协作规范 |
| `ARCHITECTURE.md`（本文件）v1.2 | **技术架构**：分层、模块、接口、数据流 |
| `README.md` | 项目概览 |
| 根目录 15 张 `*.png` | 脑叶界面参考图（F1-F14） |

## 变更记录

| 日期 | 版本 | 说明 |
|---|---|---|
| 2026-09-07 | v1.0 | 初版：基于 GDD v1.4 与 agent.md v3.1 编写，涵盖项目概述、架构分层、状态机、模块职责与交互、关键接口（数据类/纯函数/系统接口/配置）、四类数据流（启动/一天/存档回滚/结局）、开发注意事项 |
| 2026-09-12 | v1.4 | 全文档质检修复同步：上游依据升为 GDD **v1.9** / agent.md **v3.5**；§7 相关文档表与 §6.2 引用同步。**架构分层、状态机、接口与数据流均无变化** |
| 2026-09-12 | v1.3 | 依据 GDD **v1.8 全面对齐脑叶**同步：①中层部门顺序与天数改为 **中央本部 Day20 → 惩戒部 Day25 → 福利部 Day30**（解锁序列 1/6/11/16/**20/25/30**/36/41/46）；②节点 2/3 对调、RAS 7→6、Morfonica 6→7；③队长更正（Afterglow = **美竹兰**、MyGO!!!!! = **高松灯**）。**架构分层、状态机、接口与数据流均无变化** |
| 2026-09-12 | v1.2 | 依据 GDD **v1.7 队长制**同步：上游依据升为 GDD v1.7 / agent.md v3.3。部门节点的"负责人"由脑叶 Sephirot 名改为**乐队队长名**（`Bands.json` 用 `leader` / `leaderId`，队长就是 `BandMembers.json` 中 `isLeader: true` 的乐队少女，**不新增 NPC 实体**）。**架构分层、状态机、接口与数据流均无变化**，仅数据字段与显示内容调整 |
| 2026-09-12 | v1.1 | 依据 GDD **v1.6 流程重排**同步：①§2.1 屏幕顺序改为 `MainMenu · TreeOverview · Deployment · Department`；②§2.2 状态机改为 `每日剧情 → TREE_OVERVIEW(L1 生命树·部门开放) → LOADING → DEPLOY(L2 编成) → DEPARTMENT(L3)`，生命树成为**每日必经**节点；③§5.2 数据流①改为"部门开放与收容分配（L1）"、②改为"编成（L2）"；④上游依据升为 GDD v1.6 / agent.md v3.2 |
