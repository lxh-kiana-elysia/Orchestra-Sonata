using UnityEngine;

namespace YuetanMingqu
{
    /// <summary>
    /// 界面管理器：订阅 GameManager 的状态变化，据此决定各界面/面板的显隐
    /// （ARCHITECTURE §2.3「屏幕（Screen）栈管理、面板开关」）。
    ///
    /// 设计约束：
    /// 1. 不做 DontDestroyOnLoad —— 它持有的引用属于本场景。跨场景常驻会让引用
    ///    指向已销毁的物体（看似非 null 的"假 null"），开关界面时报 MissingReference。
    /// 2. 每个场景各挂一份，各管本场景内的界面，互不干扰。
    /// 3. 必须挂在"根级、全程不关闭"的物体上：本类靠 OnDisable 退订，
    ///    若挂在会被关掉的物体上（如某个面板），它一隐退就丢订阅，之后再也收不到状态变化。
    ///
    /// 职责边界：只负责开关（SetActive）。"打开后要做什么"（读设置、刷 Slider 等）
    /// 由各界面自己的代码负责，本类不越界。
    /// </summary>
    public class UIManager : MonoBehaviour
    {
        /// <summary>设置面板的根物体（整块面板的总开关，而非某一个子控件）</summary>
        [SerializeField] GameObject _settingsRoot;

        /// <summary>
        /// 订阅状态变化事件。
        /// 放在 Start 而非 OnEnable —— Unity 不保证 OnEnable 早于其他物体的 Awake 执行
        /// （每个物体各自按 Awake → OnEnable 的顺序走，整体顺序取决于 Hierarchy）。
        /// Start 才保证在所有物体的 Awake / OnEnable 都执行完之后调用，
        /// 届时 GameManager.Instance 必定已就绪。
        /// </summary>
        private void Start()
        {
            if (GameManager.Instance == null)
            {
                Debug.LogError("[UIManager] GameManager 尚未初始化，UI 将无法响应状态变化");
                return;
            }

            GameManager.Instance.StateChanged += ApplyState;
        }

        /// <summary>
        /// 退订事件，防止悬空引用（GameManager 因 DontDestroyOnLoad 活得比本场景久）。
        /// 注意：-= 一个未注册过的委托是安全的，不会抛异常，所以无需先判断是否订阅过。
        /// </summary>
        private void OnDisable()
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.StateChanged -= ApplyState;
            }
        }

        /// <summary>
        /// 依据当前状态刷新界面显隐。
        /// 目前写法是"先全部关掉、再按状态打开需要的那个"，等价于一份显式的状态→界面映射表；
        /// 后续界面对变多时，switch 里按同样套路追加 case 即可。
        /// </summary>
        /// <param name="s">GameManager 广播出的当前状态</param>
        private void ApplyState(GameState s)
        {
            // 统一复位：避免上一个状态的界面残留
            _settingsRoot.SetActive(false);

            switch (s)
            {
                case GameState.Settings:
                    _settingsRoot.SetActive(true);
                    break;
                default:
                    break;
            }
        }
    }
}
