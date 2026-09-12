# 项目长期记忆 — 乐团鸣曲

## 项目概况（2026-09-07 回滚重启后）
- 《乐团鸣曲》：叙事驱动管理模拟 × 视觉小说（脑叶式框架 × BanG Dream 同人），Unity 6000.0.38f1 URP 2D 空模板，Steam 免费发布。
- Unity 工程路径：`C:\Users\keven\Desktop\乐团鸣曲\乐团鸣曲`（内层）。
- 2026-09-07 旧开发成果全部回滚删除（备份在 `桌面\乐团鸣曲_备份_20260907\`），9-08 起以导师制重新开发。

## 文档体系（版本对齐，冲突时 GDD 优先）
- 2026-09-12 起对齐版本：**`GDD.docx` v1.8**；**`agent.md` v3.4**；**`ARCHITECTURE.md` v1.3**；**`FDD.md` v2.5**；**`UI_Interaction_Spec.md` v1.3**；**`Band_Members.md` v1.2**。（v1.7 = 部门部长改乐队队长 + 移除 Sephirot 代号；v1.8 = 部门顺序与解锁天数全面对齐脑叶 + 队长更正）
- **★GDD v1.6 流程重排（2026-09-12，最高优先，替代旧 L1/L2 说法）**：L0 开始菜单 →（加载）→ 每日剧情 CUTSCENE → **L1 生命树·部门开放（每日必经）** →（加载）→ **L2 编成·部署员工** → 「开始这一天」→ **L3 部门场景**。
  - 生命树职责：① 开放部门（LOCKED 点选 → F5 确认 → OPENING → OPEN）；② 决定**当日抽到的异想体收容进哪个已开放且未满的部门**（每部门上限 4，草案）；③ 点「继续」→ 加载 → 进 L2 编成。
  - **5 的倍数天不抽新异想体**，生命树跳过分配直接继续。
  - 「开始这一天」按钮归 **L2 编成**，点击后**直接进 L3**（不再经过生命树）。
  - 状态机构型：BOOT → MAIN_MENU → LOADING → CUTSCENE → TREE_OVERVIEW(L1) → LOADING → DEPLOY(L2) → DEPARTMENT(L3) → DAY_END → 次日 CUTSCENE → TREE_OVERVIEW。
- **★部门负责人 = 乐队队长，禁用脑叶 Sephirot 名（GDD v1.7，最高优先）**：Malkuth/Yesod/Netzach/Hod/Chesed/Tiphereth/Geburah/Hokma/Binah/Kether **全部移除**，节点上显示**队长名**。
  - 10 节点（**部门顺序与解锁天数已全面对齐脑叶**）：①控制部 Poppin'Party **户山香澄** Day1 ②情报部 Pastel*Palettes **丸山彩** Day6 ③安保部 Afterglow **美竹兰** Day11 ④培训部 Hello, Happy World! **弦卷心** Day16 ⑤**中央本部** Roselia **凑友希那** **Day20** ⑥**惩戒部** RAISE A SUILEN **CHU²** **Day25** ⑦**福利部** Morfonica **仓田真白** **Day30** ⑧记录部 MyGO!!!!! **高松灯** Day36 ⑨研发部 Ave Mujica **丰川祥子** Day41 ⑩构筑部 梦限大MewType **仲町阿拉蕾** Day46。
  - **解锁序列 = 1/6/11/16/20/25/30/36/41/46**，中层为 Day20/25/30（照搬脑叶），**不再是等距 5 天一档**。副作用：三个中层部门都落在 5 的倍数天（记忆日）→ 当天不抽新异想体，生命树只做"开放部门"。
  - ⚠️ **队长以策划拍板为准，不要再去查资料"纠正"**：**Afterglow = 美竹兰**（主唱兼吉他、发起人；虽部分中文资料写贝斯手上原绯玛丽是"协调员/队长"，本作**以兰为准**）；RAS = **CHU²**；MyGO!!!!! 原著无队长，本作**自定高松灯**。
  - **队长 = 可派遣的乐队少女之一，不是独立 NPC、不新增数据实体**：只需 `BandMembers.json` 加 `isLeader: true`，`Bands.json` 用 `leader`/`leaderId` 反查。队长可工作、崩溃退场 3 天、**不死亡**、不额外占派遣名额；队长身份只给部门加成 + 专属台词 + 节点显示名，**不影响数值与流程**。
- **FDD v2.3 的 FDD-08 已重写**为 8 小节（生命树职责表 / 编成 / 节点状态机 / `Bands.json` 草案 / 边界与待确认 / 验收点）。**遗留 3 项待确认**：①工程缺 `Bands.json` 部门节点数据源（M3 开工前必补）；②L3 内如何切换到另一已开放部门未定义；③所有部门全满 4 个时的处理未定。
- **FDD v2.1**：日循环状态机**废止「未达标强制结束（forced=true）」分支**（与 GDD §3.11 冲突）；明确两条回滚路径区别——「重新开始这一天」=当日内存态重置、不写盘；「回到记忆库」=读最近记忆日快照。
- **FDD v2.2 新增 FDD-12 相机控制系统**（M3 范畴）：正交相机、WASD/中键拖动平移、滚轮缩放 0.5x~2.0x 钳制、边界钳制、插值平滑、`LateUpdate` 更新；部门场景尺寸草案 40×12 单位、默认视野约 1/2 宽度；用 Input System 定义 Camera/Pan·Zoom·Reset。
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
- 日循环：**L1 生命树（部门开放+今日异想体收容分配）→ 加载 → L2 编成 → 「开始这一天」→ L3 部门实时操控**；候选池 3+天数/3（上限5）；重复工作收益递减；每部门异想体上限 4（草案）；5 的倍数天不抽新异想体。
- 记忆日 Day5/10…/45；真结局=Day46 前完成全部考验（首版占位 Day15 前 2 个）；Day50 结算；无永久 Game Over。
- 存档目录名已定稿 **ResonanceShelter**（persistentDataPath/ResonanceShelter/Saves/）。
- 合规：不用 BanG Dream 官方素材；不复制脑叶角色美术；美术原创方向（粉紫渐变），脑叶图仅作布局参照。

## 实现约定（v3.1/ARCHITECTURE）
- 分层：Entities 纯数据 / Systems 事件解耦（EventManager）/ UI 无规则 / Narrative 不改数值；状态切换仅 GameManager 发起。
- 数据驱动禁硬编码；核心计算抽纯函数（可单测，Unity Test Framework EditMode，Assets/Tests/）。
- **UI 事件绑定两大坑（2026-09-11 踩过，务必遵守）**：①**改名陷阱**——UnityEvent 按"方法名字符串"保存绑定，public 方法改名后 Inspector 绑定静默失效（显示 No Function/Missing）；②**参数模式陷阱**——绑定时要选**动态参数**（用事件实时值，序列化为 `m_Mode: 0`），误选成"常量参数"版本（`m_Mode: 4`/Float + `m_FloatArgument`）会让回调**永远收到固定常量**（如 0），表现为"数据被莫名清零"。**推荐做法：改用代码绑定** `slider.onValueChanged.AddListener(OnXxx)`（OnEnable 加、OnDisable 移除），可同时规避以上两坑。
- **★源文件编码强制 UTF-8（无 BOM）（2026-09-12 定，全项目 13 个 .cs 已统一）**：C# 编译器按 UTF-8 解读 `.cs`，GBK 中文注释属"侥幸通过"，会导致①注释显示乱码 ②git diff 全红 ③合并冲突。**写文件后必须校验编码**：`open(p,'rb').read().decode('utf-8')` 不抛异常才算过。
  - **排查命令**（Python，遍历 Assets/Scripts 所有 .cs 检测非 UTF-8）：
    ```python
    import os
    for dp,_,fn in os.walk(root):
        for f in fn:
            if f.endswith('.cs'):
                p=os.path.join(dp,f)
                try: open(p,'rb').read().decode('utf-8')
                except UnicodeDecodeError: print('BAD',p)
    ```
  - **修复命令**：`raw.decode('gbk')` 后 `open(p,'w',encoding='utf-8',newline='')` 重写（`newline=''` 保留 CRLF）。转换前先确认无 U+FFFD 坏字符，否则会把乱码固化。
  - **AI 工具坑**：`Write` 工具在本环境写中文有时落成 GBK → 优先用 `Edit` 做局部改，整文件重写后必须做编码校验。
- **Bash 环境间歇性损坏（2026-09-12 反复出现）**：`dirname: command not found` / `ls`·`head`·`grep` 找不到 → 报错无关实际命令，可照常看 stdout。**替代方案**：①文件操作改用 Python（`C:/Users/keven/.workbuddy/binaries/python/versions/3.13.12/python.exe`）②读写用 Read/Write/Edit/Grep/Glob 工具 ③**绝不能把含空格的中文文本当命令行参数传**（曾把文档段落拆成 4 个 0 字节垃圾文件，已清入回收站）。
- **删除文件必须走回收站**：本环境用 `SHFileOperationW`（`FO_DELETE=3` + `FOF_ALLOWUNDO=0x40`），不用 `os.remove`/`rm`。
- **已拍板（M1）**：JSON 方案 = **JsonUtility**；根命名空间 = **YuetanMingqu**。
- 进度（2026-09-12）：M0、M1 完成；**M2 三小步全部完成并已提交**——第 1 小步（F1 主菜单）、第 2 小步（F3 设置面板）、**第 3 小步（状态机 + 屏幕栈 + UIManager）已实现并通过 5 项运行测试**。M2 待办：加载屏 F2（讨论延后）。
- **M2 提交（4 个 commit，2026-09-12）**：`dd497fb` 数据类统一 UTF-8（纯编码）→ `92b90c0` feat(M2) 状态机+屏幕栈+UIManager → `11c246a` docs(M1) 数据层注释补全 → `1e10313` chore 导入 TMP + Fondamento 字体。**尚未 push**（远端 `github.com/lxh-kiana-elysia/-.git`，需晚风自行认证推送）。
- **Git 路径坑**：仓库根在 `Desktop\乐团鸣曲`（含 `.git`），Unity 工程在子目录 `乐团鸣曲\`。所有 git 路径必须带 `乐团鸣曲/` 前缀，`git show HEAD:乐团鸣曲/Assets/...` 才是对的。
- **M2 状态机实现细节（已定稿）**：`GameState` 13 个枚举（Boot/MainMenu/Settings/Deploy/Loading/TreeOverview/Department/Battle/Cutscene/Pause/Codex/Ending/DayEnd）；`GameManager` 提供 `ChangeState`（主线换屏，不压栈、不去重）/ `PushState`（覆盖层，压栈 + 去重防连点）/ `GoBack`（弹栈）；判断标准=**屏幕上原内容是否还在**。`GameManager` 挂 `[DefaultExecutionOrder(-100)]`。
- **UIManager 架构决策（已定稿）**：不能 `DontDestroyOnLoad`（会持所属场景已销毁物体的假 null 引用）；**每场景一份**，各管本场景界面；必须挂**根级、全程不关闭**的物体（否则 `OnDisable` 丢订阅）；**订阅放 `Start`**（`OnEnable` 可能早于别的物体 `Awake`，实测踩过）；职责严格限定"只开关 SetActive"，业务由各界面自己负责。
- **M2/M3 里程碑细化**（2026-09-12 重订）：M2 = 主菜单 + 设置 + **状态机/屏幕栈**（含加载屏）；M3 = 一天能玩，**7 小步**：①**M3-0 补 `Bands.json` + `BandNodeData`**（FDD-08 §8.4 字段 **`id`/`band`/`leader`/`leaderId`/`layer`/`nodeIndex`/`unlockDay`/`defaultState`**——注意 **`sephirot` 字段已删除**，`unlockDay` 序列 = 1/6/11/16/**20/25/30**/36/41/46，容量 4 建议进 GameConfig）。
   ⚠️ **当前 `Assets/Data/Bands.json` 不可用**：有 JSON 语法错误（`"band": "Poppin'Party",` 尾部多余逗号）+ 中文字段名 `"部门"` + 仍用 `sephirot` + 只有 1 个节点。正确模板见 FDD §8.4（10 条齐全）。②L1 生命树 ③L2 编成 ④L3 部门场景 + HUD ⑤相机控制（FDD-12）⑥工作闭环 ⑦结算与次日。
- 晚风会自学抢跑（已自建 MainMenu 场景/GameManager），交付前先盘点工作区避免覆盖其成果；其 GameManager 单例骨架已保留并入 YuetanMingqu 命名空间。
- **当前脚本清单**（`Assets/Scripts/`，共 13 个 .cs，全部 UTF-8 无 BOM）：
  - `Entities/`：Character.cs、BandMember.cs、Aberrations.cs、GameConfig.cs、SettingsData.cs、**GameState.cs**
  - `Systems/`：DataManager.cs、GameManager.cs、SettingsSystem.cs、**UIManager.cs**
  - `UI/Screens/`：MainMenuScreen.cs；`UI/Panels/`：SettingsPanel.cs
  - `DataLoadDemo.cs`（临时验证脚本，待 BOOT 流程替换）
  - 场景仅 `Scenes/MainMenu.unity`（`_UIManager` 根级，`_settingsRoot` 指向 SettingsPanel 的 GameObject）。
