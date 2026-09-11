namespace YuetanMingqu
{
    /// <summary>
    /// 全局游戏状态枚举，与 ARCHITECTURE §2.2 状态机、GDD v1.6 流程重排一一对应。
    ///
    /// 主线流程（换屏，用 GameManager.ChangeState）：
    ///   Boot（启动/初始化）
    ///     → MainMenu（L0 开始菜单）
    ///     → Cutscene（每日剧情）
    ///     → TreeOverview（L1 生命树：开放部门 + 今日异想体收容分配）
    ///     → Deploy（L2 编成：部署员工，点「开始这一天」）
    ///     → Department（L3 部门场景：实时操控）
    ///     → DayEnd（当日结算）
    ///     → 次日 Cutscene（循环）
    ///     → Ending（第 50 天结算；无永久 Game Over）
    ///
    /// TODO(M3 补回): 上面为便于阅读做了简化，省略了 Loading 中转。
    ///   完整的 GDD v1.6 流程应在每次跨场景时插入 Loading：
    ///   MainMenu →(加载)→ Cutscene →(加载)→ TreeOverview →(加载)→ Deploy → Department
    ///   届时需同步更新本注释与 UIManager 的 Loading 表现。
    ///
    /// 覆盖层（原界面仍在，用 GameManager.PushState / GoBack 进出）：
    ///   Settings（设置）、Pause（暂停）、Codex（图鉴）
    ///
    /// 其他：
    ///   Loading（场景/资源加载中转）、Battle（战斗，M4+ 范畴）
    ///
    /// 注意：枚举值顺序会影响其底层 int 值。CurrentState 的初始值是显式赋的
    /// （GameManager 中 = GameState.Boot），不依赖"默认 0"这种巧合——
    /// 否则将来在开头插入新枚举项，初始状态会被静默改掉。
    /// </summary>
    public enum GameState
    {
        Boot,           // 启动 / 全局初始化
        MainMenu,       // L0 开始菜单
        Settings,       // 设置面板（覆盖层）
        Deploy,         // L2 编成：部署员工
        Loading,        // 加载中转
        TreeOverview,   // L1 生命树：部门开放 + 收容分配
        Department,     // L3 部门场景：实时操控
        Battle,         // 战斗（M4+）
        Cutscene,       // 每日剧情
        Pause,          // 暂停菜单（覆盖层）
        Codex,          // 图鉴（覆盖层）
        Ending,         // 第 50 天结算 / 结局
        DayEnd          // 当日结算
    }
}
