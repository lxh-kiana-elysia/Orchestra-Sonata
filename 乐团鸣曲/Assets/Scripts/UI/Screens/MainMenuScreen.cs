using UnityEngine;

namespace YuetanMingqu
{
    /// <summary>
    /// 主菜单界面（L0 开始菜单）的按钮回调集合（UI_Interaction_Spec F1）。
    /// 职责边界：只把"玩家意图"翻译成一次事件或状态请求，不处理任何数据/规则。
    /// 本类不直接开关任何界面——界面显隐统一由 UIManager 依据 GameState 决定。
    /// </summary>
    public class MainMenuScreen : MonoBehaviour
    {
        /// <summary>
        /// F1「新游戏」：开启新的一局（Day1）。
        /// TODO(M3): 待存档系统就绪后，改为 新建存档 → ChangeState(Cutscene)，进入每日剧情。
        /// </summary>
        public void OnClickNewGame()
        {
            Debug.Log("开始新游戏");
        }

        /// <summary>
        /// F1「继续游戏」：读取最近一次存档继续。
        /// TODO(M3): 待存档系统就绪后，改为 载入最近存档 → 恢复到对应 GameState。
        /// </summary>
        public void OnClickContinue()
        {
            Debug.Log("继续游戏");
        }

        /// <summary>
        /// F1「设置」：打开设置面板。
        /// 用 PushState 而非 ChangeState —— 主菜单仍留在屏幕上（被盖住），
        /// 关闭设置后需 GoBack 回到主菜单（UI_Interaction_Spec F3「返回上一层」）。
        /// </summary>
        public void OnClickSettings()
        {
            GameManager.Instance.PushState(GameState.Settings);
        }

        /// <summary>
        /// F1「退出游戏」：关闭应用程序。
        /// TODO(M3): 若存在未保存进度，需在此处插入确认弹窗，避免误触丢档。
        /// </summary>
        public void OnClickQuit()
        {
            Debug.Log("退出游戏");
        }
    }
}
