# Aberrations.md — 异想体图鉴（独立策划文件）

> 文档版本：v1.2（2026-09-12）
> 依据：`GDD.docx` v1.11（§3.2 异想体系统、§3.4 判定公式）、`FDD.md` v2.7（§FDD-10 数据规格）
> 用途：**策划专用图鉴**。新增/修改异想体只改本文件，不必动 GDD。程序同源数据文件：`Assets/Data/Aberrations.json`
>
> ⚠️ 策划填写规则：
> 1. `workModifiers` 是**乘数**，基准 **1.0**，**禁止负值**（极低取 0.1）。写作 "+0.2" 时实际存储 1.2。
> 2. 危险等级：`1=ZAYIN / 2=TETH / 3=HE / 4=WAW / 5=ALEPH`。
> 3. ZAYIN 级默认**无出逃**（`escapeTrigger/escapeBehavior/suppressMethod` 填 null）。
> 4. 新机制（如特殊效果）用 `specialEffects` 承载，不要硬塞进已有字段。
> 5. 数值为占位时标注 `[占位]`；定稿值来自 GDD。
> 6. **绑定 = 彩蛋**：异想体绑定乐队少女**仅用于叙事彩蛋**（专属台词/背景呼应），**不影响任何数值与流程**（GDD v1.11）。异想体可先于对应角色出现，属正常设计。
> 7. 抽取字段：`minDay`（最早出现天数）+ `weight`（池内权重，0=不进池）。

---

## 抽取规则速查（GDD v1.11）

- **Day 1 固定**：`未奏响的乐章`（ZAYIN，教学用，不参与抽取）
- **Day 2 起**：从池中按"等级概率表"抽取候选（数量 `3+天数/3`，上限 5），玩家选 1 收容，未选回池

> 原则：**Day 2 起 5 种难度每日都可能出现**，仅概率不同（无硬门槛）。

| 天数 | ZAYIN | TETH | HE | WAW | ALEPH |
|---|---|---|---|---|---|
| Day 1 | 固定（教学） | — | — | — | — |
| Day 2-5 | 50% | 30% | 12% | 6% | 2% |
| Day 6-10 | 35% | 32% | 18% | 10% | 5% |
| Day 11-15 | 25% | 30% | 22% | 15% | 8% |
| Day 16-20 | 18% | 27% | 25% | 20% | 10% |
| Day 21-25 | 12% | 23% | 27% | 25% | 13% |
| Day 26-30 | 8% | 18% | 27% | 30% | 17% |
| Day 31-35 | 5% | 14% | 25% | 34% | 22% |
| Day 36-40 | 3% | 10% | 22% | 38% | 27% |
| Day 41-45 | 2% | 7% | 18% | 40% | 33% |
| Day 46-50 | 1% | 4% | 14% | 41% | 40% |

> 默认 `minDay` = **1**（5 种难度全程可出现，不再按等级设硬门槛）。百分比为占位，待调参。

---

## 字段速查

| 字段 | 类型 | 说明 |
|---|---|---|
| `id` | string | 唯一标识（`aber_xxx_NN`） |
| `name` | string | 名称 |
| `type` | enum | 创伤型 / 回忆型 / 欲望型 / 工具型 |
| `riskLevel` | int | 1-5（ZAYIN~ALEPH） |
| `mood` / `moodDangerMin` / `moodDangerMax` | float | 情绪值 0-100 与危险窗口 |
| `linkedCharacterId` | string | 对应角色（乐队少女 id 或 null） |
| `workModifiers` | list | 各工作类型的**乘数**修正（1.0 基准） |
| `energyPerSuccess` | int | 每次成功产出的**情感粒子**（当日能源）基准值；失败约 25% |
| `pointsPerSuccess` | int | 每次成功产出的**异想体点数**（解锁剧情/研发装备用） |
| `accumulatedPoints` | float | ☆运行时：已累积的异想体点数（**不写在配置里**） |
| `storyUnlockThresholds` | int[] | 异想体点数解锁剧情的档位 |
| `escapeTrigger` / `escapeBehavior` / `suppressMethod` | string | 出逃规则（ZAYIN 为 null） |
| `specialEffects` | list | 专属效果（trigger/effect/target/value） |
| `background` | string | 背景故事 |

---

## 图鉴

### ① 熄灭的太阳（aber_sun_01）

| 项 | 值 |
|---|---|
| type | 创伤型 |
| riskLevel | **3（HE）** — 派遣门槛：判定属性 ≥ III 级/41 |
| linkedCharacterId | `band_ppp_kasumi`（户山香澄） |
| 情绪值 | 默认 60；危险窗口 <20 或 >90 |
| 工作适配（乘数） | 演奏 **1.2** / 谈话 **1.0** / 创作 **0.9** / 比武 **0.7** |
| 产出 | 情感粒子 `[占位]` / 异想体点数 **+8**（GDD 已定） |
| storyUnlockThresholds | 0 / 40 / 120 |
| 出逃条件 | 情绪值跌破 10，或连续 3 天未工作 |
| 抽取 | `minDay` **1**（5 种难度全程可出现，无硬门槛） / `weight` 100 / 绑定香澄=**彩蛋**（不影响流程） |
| 出逃行为 | 全局工作成功率 -10%；形态"黯淡的日轮"周期点名成员，精神额外 -5 |
| 镇压/安抚 | 执行"演奏"（显示名"为它演奏最后一支歌"）累计成功 2 次，或共感 ≥ III 的成员"谈话"成功 1 次 |
| 背景 | 同位体香澄在"没能让某场演出发光"的遗憾中凝固出的存在——它记得所有观众离场后的寂静 |

> 数据卡详见 GDD §3.2 示例一。

---

### ② 未奏响的乐章（aber_score_01）· 2026-09-08 定稿

| 项 | 值 |
|---|---|
| type | 回忆型 |
| riskLevel | **1（ZAYIN）** — 完全无害，无派遣门槛 |
| linkedCharacterId | `band_mygo_anon`（千早爱音） |
| 外观 | 漂浮的半透明光质乐谱架，空白五线谱；音符如萤火虫飘落消散 |
| 情绪值 | 默认 50；**无危险窗口**（ZAYIN 不触发出逃） |
| 工作适配（乘数） | 演奏 **1.3** / 创作 **1.1** / 谈话 **1.0** / 比武 **0.5** |
| 产出 | 成功 +10 情感粒子；失败约 25% |
| storyUnlockThresholds | 0 / 20 / 50 |
| 出逃 | **无**（ZAYIN 无害） |
| 抽取 | `minDay` 1 / `weight` 0（**Day1 固定教学异想体，不进池**） / 绑定爱音=**彩蛋**（不影响流程） |
| 背景 | 一段从未被奏响的旋律，凝固成了等待知音的空谱。它没有过去，只在等待一个能赋予它意义的人 |

**specialEffects（管理须知）**

| trigger | effect | target | value | 说明 |
|---|---|---|---|---|
| `workResultExcellent` | `restoreSp` | `worker` | 8 `[占位]` | 工作结果为"优"→ 恢复该员工精神 |
| `fullOutput` | `restoreSp` | `department` | 5 `[占位]` | 满产出 → 该部门所有员工恢复精神 |

**对应 JSON 片段**（完整版见 FDD §10.3；字段名以 v1.9 为准）

```jsonc
{
  "id": "aber_score_01",
  "name": "未奏响的乐章",
  "type": "回忆型",
  "riskLevel": 1,
  "mood": 50,
  "moodDangerMin": 0,
  "moodDangerMax": 100,
  "linkedCharacterId": "band_mygo_anon",
  "workModifiers": [
    { "workType": "Performance", "modifier": 1.3 },
    { "workType": "Creation",    "modifier": 1.1 },
    { "workType": "Talk",        "modifier": 1.0 },
    { "workType": "Combat",      "modifier": 0.5 }
  ],
  "energyPerSuccess": 10,            // 情感粒子（成功基准值）
  "pointsPerSuccess": 5,             // 异想体点数（草案，待调参）
  "storyUnlockThresholds": [0, 20, 50],
  "escapeTrigger": null,
  "escapeBehavior": null,
  "suppressMethod": null,
  "specialEffects": [
    { "trigger": "workResultExcellent", "effect": "restoreSp", "target": "worker", "value": 8 },
    { "trigger": "fullOutput", "effect": "restoreSp", "target": "department", "value": 5 }
  ],
  "background": "一段从未被奏响的旋律，凝固成了等待知音的空谱。它没有过去，只在等待一个能赋予它意义的人。"
}
```

---

## 待补充与待确认

- ⏳ **Pastel\*Palettes 的同位体异想体尚未创建**（节点 2）。**这不是阻塞项**——
  按 GDD §3.2「绑定 = 彩蛋」规则，异想体与角色**不要求一一对应**，异想体可以没有对应角色。
  因此 PasPale 的乐队任务可挂在池中任意异想体上，丸山彩也可暂时没有专属装备，**均不影响流程与里程碑**。
  待补（非必须）：该异想体的 id / 名称 / type / riskLevel / workModifiers / 绑定角色。
- ℹ️ **「未奏响的乐章」既是 Day 1 固定教学异想体、又绑定 MyGO 的角色（千早爱音，节点 8 登场）——这是设计预期，不需要调整**：
  按 GDD §3.2，绑定只作用于该异想体自身的情境（专属台词 / 背景故事），**不会引出、解锁或影响玩家遇到对应角色的时机与内容**。
  玩家在第 1 天遇到它，只是遇到"一个与某位尚未登场的少女有共鸣的异想体"，属正常的彩蛋式铺垫。
- 异想体类型分布与数量规划（首版需要多少个）：GDD §5.4 内容规模
- 后续异想体按"每日候选池"逐步添加（GDD §1.3(1)）
- 异想体的绑定角色为**可选彩蛋**（GDD §3.2）：可以没有绑定，也可多个异想体对应同一角色的不同侧面；**不要求一一对应**

---

## 变更记录

| 日期 | 版本 | 说明 |
|---|---|---|
| 2026-09-08 | v1.0 | 初版：从 GDD 独立出异想体图鉴。收录"熄灭的太阳"与"未奏响的乐章"（含 specialEffects 与 JSON 片段）；明确 workModifiers 为乘数、禁止负值 |
| 2026-09-12 | v1.1 | 质检修复：依据升为 GDD v1.9 / FDD v2.6；新增「待补充与待确认」两项说明 |
| 2026-09-12 | v1.2 | **纠正误判**：把 PasPale 异想体缺失、"未奏响的乐章"教学定位**从"阻塞/错位"降级为"可选彩蛋"**——GDD §3.2 早已明确"绑定绝不产生数值或流程影响、不阻塞任何任务与剧情"。同步重写"每个异想体需配一名对应角色"为"可选"；依据升 GDD v1.10 |
