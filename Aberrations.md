# Aberrations.md — 异想体图鉴（独立策划文件）

> 文档版本：v1.0（2026-09-08）
> 依据：`GDD.docx` v1.5（§3.2 异想体系统、§3.4 判定公式）、`FDD.md` v1.3（§FDD-10 数据规格）
> 用途：**策划专用图鉴**。新增/修改异想体只改本文件，不必动 GDD。程序同源数据文件：`Assets/Data/Aberrations.json`
>
> ⚠️ 策划填写规则：
> 1. `workModifiers` 是**乘数**，基准 **1.0**，**禁止负值**（极低取 0.1）。写作 "+0.2" 时实际存储 1.2。
> 2. 危险等级：`1=ZAYIN / 2=TETH / 3=HE / 4=WAW / 5=ALEPH`。
> 3. ZAYIN 级默认**无出逃**（`escapeTrigger/escapeBehavior/suppressMethod` 填 null）。
> 4. 新机制（如特殊效果）用 `specialEffects` 承载，不要硬塞进已有字段。
> 5. 数值为占位时标注 `[占位]`；定稿值来自 GDD。

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
| `pointsPerSuccess` | int | 成功时产出的情感粒子基准值 |
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
| linkedCharacterId | `band_pp_kasumi`（户山香澄） |
| 情绪值 | 默认 60；危险窗口 <20 或 >90 |
| 工作适配（乘数） | 演奏 **1.2** / 谈话 **1.0** / 创作 **0.9** / 比武 **0.7** |
| 产出 | 每次成功 +8 情感粒子（基准） |
| storyUnlockThresholds | 0 / 40 / 120 |
| 出逃条件 | 情绪值跌破 10，或连续 3 天未工作 |
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
| 背景 | 一段从未被奏响的旋律，凝固成了等待知音的空谱。它没有过去，只在等待一个能赋予它意义的人 |

**specialEffects（管理须知）**

| trigger | effect | target | value | 说明 |
|---|---|---|---|---|
| `workResultExcellent` | `restoreSp` | `worker` | 8 `[占位]` | 工作结果为"优"→ 恢复该员工精神 |
| `fullOutput` | `restoreSp` | `department` | 5 `[占位]` | 满产出 → 该部门所有员工恢复精神 |

**对应 JSON 片段**（完整版见 FDD §10.3）

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
  "pointsPerSuccess": 10,
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

## 待补充

- 异想体类型分布与数量规划（首版需要多少个）：GDD §5.4 内容规模
- 后续异想体按"每日候选池"逐步添加（GDD §1.3(1)）
- 每个异想体需配一名对应角色（乐队少女）或明确标注为"共鸣器/环境型"特例

---

## 变更记录

| 日期 | 版本 | 说明 |
|---|---|---|
| 2026-09-08 | v1.0 | 初版：从 GDD 独立出异想体图鉴。收录"熄灭的太阳"与"未奏响的乐章"（含 specialEffects 与 JSON 片段）；明确 workModifiers 为乘数、禁止负值 |
