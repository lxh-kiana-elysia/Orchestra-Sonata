using System.Collections.Generic;
using UnityEngine;

namespace YuetanMingqu
{
    /// <summary>
    /// 全局管理器（单例）：全局状态机与系统调度核心（ARCHITECTURE §2.2）。
    /// 状态切换只由本类发起（ARCHITECTURE §2.2 约束），其他脚本只能调用本类方法请求切换。
    /// 当前含：单例骨架 + 状态机（ChangeState / PushState / GoBack）+ StateChanged 广播。
    /// 待补：EventManager 接入、天数推进、记忆日锚定（M3+）。
    /// </summary>
    [DefaultExecutionOrder(-100)]    // 数值越小越先执行：保证其他脚本访问 Instance 时本类已 Awake
    public class GameManager : MonoBehaviour
    {
        /// <summary>全局唯一实例</summary>
        public static GameManager Instance { get; private set; }

        /// <summary>当前状态。显式初始化为 Boot，不依赖 int 默认值 0（防枚举顺序变动导致漂移）</summary>
        public GameState CurrentState { get; private set; } = GameState.Boot;

        /// <summary>状态返回栈：PushState 时压入旧状态，GoBack 时弹出</summary>
        private readonly Stack<GameState> _stateStack = new Stack<GameState>();

        /// <summary>状态变化广播。订阅方负责 += / -= 配对，否则会残留悬空引用</summary>
        public event System.Action<GameState> StateChanged;

        private void Awake()
        {
            // 已存在其他实例时销毁自己，保证全局唯一
            if (Instance != null && Instance != this)
            {
                Debug.LogWarning("GameManager 重复创建，销毁新实例");
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
            Debug.Log("GameManager 单例已初始化");
        }

        private void OnDestroy()
        {
            // 清空静态引用，防止外部拿到已销毁的实例
            if (Instance == this)
            {
                Instance = null;
            }
        }

        private void Start()
        {
            Debug.Log("乐团鸣曲 —— GameManager 已启动");
        }

        /// <summary>
        /// 主线流程切换：直接替换当前状态，不压栈（ARCHITECTURE §2.2）。
        /// 用于"原来的东西整个换掉"的场合（Cutscene / Loading / DayEnd 等）。
        /// 会广播 StateChanged；场景加载将在 EventManager 接入后（M3）补齐。
        ///
        /// 刻意不做去重：本方法由程序内部调用，若出现"连续两次切到同一状态"
        /// 那多半是流程 bug，应当让它暴露出来，而不是被静默吞掉。
        /// </summary>
        public void ChangeState(GameState next)
        {
            var prev = CurrentState;
            CurrentState = next;
            Debug.Log($"[State] {prev} -> {next}");
            StateChanged?.Invoke(CurrentState);
        }

        /// <summary>
        /// 打开覆盖层/子界面：把当前状态压栈后切换（供 GoBack 返回）。
        /// 判断标准：屏幕上原来的东西还在（被盖住）→ 用本方法；
        ///          原来的东西整个换掉（主线流程）→ 用 ChangeState。
        ///
        /// 会做去重：本方法由玩家点击直接触发，连点两下按钮会连压两层，
        /// 之后要按两次返回才退得出来。这里挡掉，属于体验保护。
        /// </summary>
        public void PushState(GameState next)
        {
            // 已经在目标状态了就不重复压栈（防手抖/连点）
            if (next == CurrentState) return;

            _stateStack.Push(CurrentState);
            var prev = CurrentState;
            CurrentState = next;
            Debug.Log($"[State] {prev} -> {next} (stack: {_stateStack.Count})");
            StateChanged?.Invoke(CurrentState);
        }

        /// <summary>
        /// 返回上一个状态：弹出状态栈并恢复（配合 PushState 使用）。
        /// 栈空时打警告并保持原状态不动——栈空说明调用方逻辑有误，不该静默兜底成某个默认状态。
        /// </summary>
        public void GoBack()
        {
            if (_stateStack.Count == 0)
            {
                Debug.LogWarning("[State] 状态栈为空，GoBack 被忽略");
                return;
            }

            CurrentState = _stateStack.Pop();
            Debug.Log($"[State] 返回 -> {CurrentState}");
            StateChanged?.Invoke(CurrentState);
        }
    }
}
