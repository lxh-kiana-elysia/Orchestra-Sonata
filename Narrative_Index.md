# Narrative_Index.md — 剧情索引与模板（独立策划文件）

> 文档版本：v1.1（2026-09-12）
> 依据：`GDD.docx` v1.9（§2 世界观、§3.5 剧情系统、§3.12 完整流程）
> 用途：**策划专用剧情索引**。所有剧情文本独立管理，新增/修改剧情只改本目录，不必动 GDD。
> 程序存放：`Assets/Data/Dialogues/`（Yarn Spinner 的 `.yarn` 文件）或 JSON 对话表。
>
> ⚠️ 当前状态：**世界观与主线框架已定，具体剧情文本待策划撰写**（GDD §2）。

---

## 一、剧情分级（GDD §3.5，必读）

| 层级 | 触发条件 | 表现 | 时长 |
|---|---|---|---|
| **日常反馈** | 每次工作完成 | 一行台词 / 表情差分 / 小演出 | ≤5 秒 |
| **节点剧情** | ①同一异想体工作第 1/3/5/10 次 ②异想体点数跨档 ③特定角色组合对特定异想体 ④章节/考验节点 | 完整演出（立绘+对话+场景） | 不限 |
| **组合触发** | 特定角色组合 + 特定异想体 | 专属隐藏互动剧情（鼓励编成搭配） | 短 |

---

## 二、剧情文件组织（建议）

```
Assets/Data/Dialogues/
├── prologue.yarn          # 序章（Day 1 开场）
├── daily/                 # 每日剧情（Day1-50）
│   ├── day_01.yarn
│   └── ...
├── character/             # 个人线（每名乐队少女一个文件）
│   ├── kasumi.yarn
│   └── anon.yarn
├── aberration/            # 异想体相关剧情（按异想体 id）
│   ├── aber_sun_01.yarn
│   └── aber_score_01.yarn
└── ending/                # 结局
    ├── ending_true.yarn
    └── ending_false.yarn
```

**命名规则**：`<类型>_<id>.yarn`，节点名用 `Node_<场景>_<序号>`。

---

## 三、已定稿的剧情锚点（必须实现）

| 锚点 | 时机 | 内容（策划待写） | 关联 |
|---|---|---|---|
| 序章开场 | Day 1 开始 | 演出高潮情感共鸣 → 时空裂缝 → 众人被卷入 Ring | GDD §2.1 |
| 每日开场 | 每天编成前 | `DAY X` 标签 + 当日剧情片段 | GDD §3.12 |
| 部门解锁 | 节点解锁时 | 该乐队相关角色登场对话（F5 弹窗） | GDD §3.1-L2 |
| 角色加入 | 完成加入条件 | 该乐队少女加入的演出（如香澄 Day1、爱音完成任务后） | `Band_Members.md` |
| 异想体解锁剧情 | 点数达档位 | 每个异想体的背景故事展开 | `Aberrations.md` 的 `storyUnlockThresholds` |
| 考验前后 | 考验开启/完成 | 该乐队的核心剧情（对应脑叶核心抑制） | `Quests.md` |
| 记忆日 | Day 5/10/…/45 | 记忆日相关演出（锚定提示） | GDD §3.12 |
| **真结局** | Day 50（Day46 前全部考验完成） | 归途之音被驱动 → 真结局演出 | GDD §2.3 |
| **假结局** | Day 50（未全部完成） | 缺失考验的收场 → 可回档补齐 | GDD §2.3 |

---

## 四、剧情文本模板（策划照此填写）

### 4.1 日常反馈（短，一行）
```yaml
id: fb_kasumi_work_success_01
character: band_ppp_kasumi
trigger: workSuccess
text: "嘿嘿，感觉还不错！"
```
> 建议每名角色至少 3 条成功 + 3 条失败反馈，避免重复。

### 4.2 节点剧情（完整）
```yaml
id: node_sun_first_work
title: 第一次触碰熄灭的太阳
trigger:
  type: workCount
  aberrationId: aber_sun_01
  count: 1
scene: department_control
participants: [band_ppp_kasumi]
dialogue:
  - speaker: band_ppp_kasumi
    text: "它……好像在等一首歌。"
  - speaker: null          # null = 旁白
    text: "[熄灭的太阳] 微微颤动，光斑在它的表面缓慢亮起。"
  - speaker: band_ppp_kasumi
    text: "那——这首，是我还没唱完的那首。"
effects:
  - type: mood
    target: aber_sun_01
    value: +12
  - type: aberrationPoints
    target: aber_sun_01
    value: +8
```

### 4.3 分支选项（F12 交互）
```yaml
id: node_choice_example
text: "要如何回应？"
options:
  - text: "关于脑叶公司"
    goto: node_choice_lobotomy
  - text: "关于你"
    goto: node_choice_about_you
```
> 最多 3 个选项；不可 ESC 跳过（见 `UI_Interaction_Spec.md` F12）。

---

## 五、写作注意事项（合规，GDD 第 7 章）

- ✅ 心理议题（孤独、遗憾、自我怀疑）可以写，但**必须导向理解、成长与救赎**。
- ✅ 异想体"被理解"是正向推进（爆满=突破事件，不是失败）。
- ❌ 禁止：角色自杀、自残、恶意丑化等极端描写。
- ❌ 禁止：永久锁死角色/剧情线的负面结局（无永久损失原则）。
- ❌ 禁止：使用《BanG Dream!》官方台词/歌词原文；异想体设定为原创。
- 每篇完稿后在 GDD 第 7 章合规清单打勾。

---

## 六、当前待办

- [ ] 撰写序章（Day 1 开场）
- [ ] 撰写 2 名已定稿乐队少女（香澄、爱音）的个人线
- [ ] 撰写 2 个已定稿异想体的解锁剧情（按 `storyUnlockThresholds`）
- [ ] 撰写每日剧情片段模板（Day1-50 骨架）
- [ ] 撰写真/假结局文本

---

## 变更记录

| 日期 | 版本 | 说明 |
|---|---|---|
| 2026-09-12 | v1.1 | 质检修复：依据升为 GDD v1.9；核对「记忆日锚点 = Day 5/10/…/45」「真结局 Day46 前完成全部考验 / Day50 演出」与 GDD v1.9 一致，无内容冲突 |
| 2026-09-08 | v1.0 | 初版：从 GDD 独立出剧情索引。含分级规则、文件组织、已定稿剧情锚点、三类文本模板、合规注意事项、当前待办 |
