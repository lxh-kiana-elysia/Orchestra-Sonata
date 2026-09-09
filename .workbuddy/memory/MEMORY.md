# 项目长期记忆 — 乐团鸣曲

## 项目概况（2026-09-07 回滚重启后）
- 《乐团鸣曲》：叙事驱动管理模拟 × 视觉小说（脑叶式框架 × BanG Dream 同人），Unity 6000.0.38f1 URP 2D 空模板，Steam 免费发布。
- Unity 工程路径：`C:\Users\keven\Desktop\乐团鸣曲\乐团鸣曲`（内层）。
- 2026-09-07 旧开发成果全部回滚删除（备份在 `桌面\乐团鸣曲_备份_20260907\`），9-08 起以导师制重新开发。

## 文档体系（版本对齐，冲突时 GDD 优先）
- `GDD.docx` **v1.5**（设计宪法）；`agent.md` **v3.1**；`ARCHITECTURE.md` **v1.1**；`FDD.md` **v1.9**；`UI_Interaction_Spec.md`（F1-F14）。
- **独立策划文件（v1.5"内容文件独立化"）**：`Band_Members.md`（乐队少女名册，含香澄/爱音完整数据与专属装备）→ 程序文件 `Assets/Data/BandMembers.json`；`Aberrations.md`（异想体图鉴+字段速查+JSON）；`Equipments.md`（装备图鉴，`ego_weapon_sun_01` 等 ID）；`Quests.md`；`Narrative_Index.md`。根目录 15 张脑叶参考图。
- **命名（强制，2026-09-08 定）**：JSON 键与数据类字段一律 **camelCase**，**严禁 snake_case**（JsonUtility 键名不符时静默为 0，高危）。故 `hair_style`→`hairStyle`。
- **字段拆分定稿**：`energyPerSuccess`=情感粒子产出 / `pointsPerSuccess`=异想体点数产出 / `accumulatedPoints`=运行时累积点数（原 `points`）。
- **静态配置 vs 运行时**：角色/异想体 JSON 只存静态配置与四维**初始值**；`todayWorkCount`/`personalLineProgress`/`unlockedStoryNodes`/`isCollapsed`/`accumulatedPoints` 由程序生成入存档。
- 全文抽取缓存：`.workbuddy/gdd_extract.txt`（GDD 每次改版后需重新抽取）。
- **v1.5 关键变更**：①角色拆两套库 `Characters.json`（普通员工）/ `BandMembers.json`（乐队少女，字段集不同）；②`workModifiers` 一律**乘数**基准 1.0、**禁负值**（+0.2→1.2）；③异想体新增 `minDay` / `weight` / `specialEffects`；④Day1 固定异想体「未奏响的乐章」aber_score_01（ZAYIN，教学，对应千早爱音 anon_chihaya），完整数据在 FDD §10.3；⑤抽取概率表 5 天区间（GDD 表 3）；⑥死亡规则定稿：普通员工**会死亡**（装备掉落），乐队少女**不死亡**（崩溃退场 3 天，成长/装备/个人线保留）。

## 协作模式（导师制，用户 2026-09-08 定）
- 里程碑：M0 工程跑通 → M1 数据能读 → M2 界面能点 → M3 一天能玩(最小可玩) → M4 系统完整 → M5 内容填充。
- 每步给四件：目标/手动操作/AI 生成部分/验证清单；**每次只给当前一步**，用户回"完成"再推进。
- **2026-09-08 晚更新（最高优先）**：晚风要求"**不要直接替我完成全部代码**"——AI 只讲目标、核心概念、在流程中的作用 + 具体任务，代码由他自己写；他提问时解释原因与方法、给方向，不直接给完整答案。此后交付以"引导任务"为主，不再批量生成 .cs。
- AI 职责（agent.md §1）：搭框架、写核心代码、审查优化；**绝不**改数值机制、写剧情、定美术、填占位、push/回滚 git。
- 遇到"待定/占位"：停下问，代码留 `// TODO(设计待确认): 关联 GDD §X.X`，占位值进 GameConfig.json。

## 已定稿核心规则（勿改）
- 四维：HP(红)/SP(白)/演奏水平(蓝)/共感(紫)，初始全 10，刻度 1-120，分档 I~EX 六档；4 工作与四维一一映射；判定 min(1, 基础×(判定属性/120)×异想体修正)；成败都成长∝消耗。
- 三层资源：情感粒子（当日）/星石（结余 10:1 草案）/异想体点数（按异想体累积）；禁"羁绊点数"。
- 日循环：编成 L1→「开始这一天」→生命树 L2→部门 L3 实时操控；候选池 3+天数/3（上限5）；重复工作收益递减。
- 记忆日 Day5/10…/45；真结局=Day46 前完成全部考验（首版占位 Day15 前 2 个）；Day50 结算；无永久 Game Over。
- 存档目录名已定稿 **ResonanceShelter**（persistentDataPath/ResonanceShelter/Saves/）。
- 合规：不用 BanG Dream 官方素材；不复制脑叶角色美术；美术原创方向（粉紫渐变），脑叶图仅作布局参照。

## 实现约定（v3.1/ARCHITECTURE）
- 分层：Entities 纯数据 / Systems 事件解耦（EventManager）/ UI 无规则 / Narrative 不改数值；状态切换仅 GameManager 发起。
- 数据驱动禁硬编码；核心计算抽纯函数（可单测，Unity Test Framework EditMode，Assets/Tests/）。
- **已拍板（M1）**：JSON 方案 = **JsonUtility**；根命名空间 = **YuetanMingqu**。
- 进度：M0 完成（2026-09-08）；M1 已交付（数据三件套+Entities 4 文件+静态 DataManager+DataLoadDemo，演示场景=用户自建的 MainMenu.unity），等"M1 完成"。
- 晚风会自学抢跑（已自建 MainMenu 场景/GameManager），交付前先盘点工作区避免覆盖其成果；其 GameManager 单例骨架已保留并入 YuetanMingqu 命名空间。
