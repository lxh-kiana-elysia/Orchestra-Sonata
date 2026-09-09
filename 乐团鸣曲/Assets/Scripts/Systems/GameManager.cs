using UnityEngine;

namespace YuetanMingqu
{
    /// <summary>
    /// 全局管理器（单例）：全局状态机与系统调度核心（ARCHITECTURE §2.2）。
    /// 状态切换只由本类发起（ARCHITECTURE §2.2 约束）。
    /// M1 阶段仅含单例骨架；状态机将在 M2 接入。
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        /// <summary>全局唯一实例</summary>
        public static GameManager Instance { get; private set; }

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
    }
}
