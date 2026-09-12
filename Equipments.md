# Equipments.md — 装备图鉴（独立策划文件）

> 文档版本：v1.1（2026-09-12）
> 依据：`GDD.docx` v1.9（装备随少女绑定、员工死亡掉落）、`FDD.md` v2.6（FDD-11 装备系统）
> 用途：**策划专用装备表**。新增/修改装备只改本文件。程序同源数据：`Assets/Data/Equipments.json`
>
> ⚠️ 策划填写规则：
> 1. **所有装备均由异想体研发而来** —— 每个异想体产出**一套**装备（武器 1 + 护甲 1 + 饰品 1）。
> 2. 任何人可消耗该异想体的**异想体点数**研发该套装备（研发 = 解锁型号，**可装备给任意数量**满足条件的员工）。
> 3. 与该异想体**绑定的乐队少女**，加入时**自动获得**该套装备（无需研发、不可转让），见 `Band_Members.md`。
> 4. `resistances` 中 **1.0 = 普通，<1 = 抗性，>1 = 弱点**。
> 5. 装备要求**仅在装备前检查**；装备后不因属性下降而卸下。
> 6. **普通员工死亡** → 其装备掉落消失，需重新研发；**乐队少女不死亡**，装备保留。
> 7. 数值为占位时标注 `[占位]`。

---

## 装备来源规则（总览）

```
每个异想体 ──产出──> 一套装备（武器 + 护甲 + 饰品）
                        │
        ┌───────────────┴────────────────┐
        ▼                                ▼
  绑定该异想体的乐队少女              普通员工 / 其他角色
  加入时自动获得（绑定、无需研发）    消耗该异想体点数研发后装备
```

- 同一套装备**不是**"专属版 + 通用版"两套——只有一套，区别仅在**获得方式**。
- 研发后解锁的是"型号"，不是"单件"：可同时装备给多名满足条件者（与脑叶一致）。
- 饰品例外：饰品**不通过研发**，从对该异想体工作完成时随机掉落（每名角色独立获得）。

---

## 字段说明

| 字段 | 类型 | 说明 |
|---|---|---|
| `id` / `name` | string | 标识与名称 |
| `type` | enum | `weapon` / `armor` / `accessory` |
| `grade` | enum | 装备等级 `ZAYIN/TETH/HE/WAW/ALEPH`（源自异想体等级） |
| `sourceAberrationId` | string | **来源异想体**（所有装备必填） |
| `damageType` / `damage` | — | 仅武器：`RED/WHITE/BLACK/PALE` + 伤害区间 `[min,max]` |
| `attackSpeed` / `attackRange` | — | 仅武器：`fast/normal/slow`、`short/medium/long` |
| `resistances` | dict | 仅护甲：四色抗性 |
| `slot` | enum | 仅饰品：`head/face/neck/hand/back/charm` |
| `bonus` | dict | 饰品属性加成 |
| `requirement` | dict | 装备要求 `{ stat, level }`（**仅装备前检查**） |
| `effects` | list | 附加效果（如工作成功率修正） |
| `unlockCost` | int | 研发所需该异想体点数（饰品无此字段，用 `dropWeight`） |

---

## 装备图鉴

### ① 来自「熄灭的太阳」（aber_sun_01，HE）

| id | 名称 | 类型 | 等级 | 关键属性 | 要求 | 研发点数 |
|---|---|---|---|---|---|---|
| `ego_weapon_sun_01` | 未熄的拨片 | weapon | HE | RED 伤害 `[5,8]`、攻速 normal、射程 short；演奏成功率 +5% `[占位]` | 演奏水平 ≥ II | 40 |
| `ego_armor_sun_01` | 余晖外套 | armor | HE | 抗性 RED 0.8 / WHITE 0.6 / BLACK 1.2 / PALE 1.5 | 共感 ≥ II | 120 |
| `ego_gift_sun_01` | 星形发夹 | accessory | — | 槽位 `head`；共感 +3 `[占位]`；掉落权重 30 | 无 | —（掉落） |

> 香澄（`band_ppp_kasumi`）为其同位体，加入时**自动获得**这一套。

### ② 来自「未奏响的乐章」（aber_score_01，ZAYIN）

| id | 名称 | 类型 | 等级 | 关键属性 | 要求 | 研发点数 |
|---|---|---|---|---|---|---|
| `ego_weapon_score_01` | 初启的音符 | weapon | ZAYIN | WHITE 伤害 `[3,5]` `[占位]`、攻速 fast、射程 medium；演奏成功率 +5% `[占位]` | 演奏水平 ≥ I | 20 |
| `ego_armor_score_01` | 空白谱衣 | armor | ZAYIN | 抗性 RED 1.0 / WHITE 0.7 / BLACK 0.9 / PALE 1.2 `[占位]`；精神恢复 +10% | 共感 ≥ I | 50 |
| `ego_gift_score_01` | 萤火音符 | accessory | — | 槽位 `charm`；共感 +3 `[占位]`；掉落权重 30 | 无 | —（掉落） |

> 爱音（`band_mygo_anon`）为其同位体，加入时**自动获得**这一套。

---

## 完整 JSON 示例（Equipments.json）

```jsonc
[
  {
    "id": "ego_weapon_sun_01",
    "name": "未熄的拨片",
    "type": "weapon",
    "grade": "HE",
    "sourceAberrationId": "aber_sun_01",
    "damageType": "RED",
    "damage": [5, 8],
    "attackSpeed": "normal",
    "attackRange": "short",
    "requirement": { "stat": "performance", "level": 2 },
    "effects": [ { "type": "workSuccessRate", "workType": "Performance", "value": 0.05 } ],
    "unlockCost": 40
  },
  {
    "id": "ego_armor_sun_01",
    "name": "余晖外套",
    "type": "armor",
    "grade": "HE",
    "sourceAberrationId": "aber_sun_01",
    "resistances": { "RED": 0.8, "WHITE": 0.6, "BLACK": 1.2, "PALE": 1.5 },
    "requirement": { "stat": "empathy", "level": 2 },
    "effects": [],
    "unlockCost": 120
  },
  {
    "id": "ego_gift_sun_01",
    "name": "星形发夹",
    "type": "accessory",
    "slot": "head",
    "sourceAberrationId": "aber_sun_01",
    "bonus": { "empathy": 3 },
    "dropWeight": 30
  }
]
```

---

## 待补充

- 其余异想体（随 `Aberrations.md` 扩充）各补一套装备
- 各等级装备的数值曲线（ZAYIN→ALEPH 的伤害/抗性/加成梯度）
- 装备的美术表现（发光/音波等，按本作原创方向，不基于脑叶视觉）

---

## 变更记录

| 日期 | 版本 | 说明 |
|---|---|---|
| 2026-09-12 | v1.1 | 质检修复：依据升为 GDD v1.9 / FDD v2.6；⚠️ 首版第 2 支乐队 **Pastel*Palettes** 的同位体异想体未创建 → 丸山彩（）的专属装备暂缺，待异想体确定后补一套 |
| 2026-09-08 | v1.0 | 初版：独立出装备图鉴。明确**所有装备均由异想体研发而来**（每异想体一套：武器+护甲+饰品）；绑定少女自动获得、他人消耗点数研发；收录熄灭的太阳与未奏响的乐章两套装备（含 JSON 示例） |
