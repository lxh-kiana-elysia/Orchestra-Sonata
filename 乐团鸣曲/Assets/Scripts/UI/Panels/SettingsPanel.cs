using UnityEngine;
using UnityEngine.UI;
namespace YuetanMingqu
{
    /// <summary>
    /// 设置面板（F3，覆盖层）。对应 UI_Interaction_Spec F3。
    ///
    /// 职责边界（重要）：本类【只做数据准备与意图转发】，不碰开关。
    ///   - 何时显示/隐藏 → 由 UIManager 按 GameState 决定（本类不调 SetActive）
    ///   - 「保存」→ 写回 SettingsSystem 后请求 GoBack
    ///   - 「取消」→ 直接请求 GoBack（草稿丢弃，不动 Current）
    ///
    /// 草稿机制：所有编辑先落在 _draft（副本），只有「保存」才写回 Current，
    /// 因此「取消」天然无损——这正是"改代码不会污染已保存数据"的保证。
    /// </summary>
    public class SettingsPanel : MonoBehaviour
    {
        /// <summary>主音量滑块（0-1）</summary>
        [SerializeField] private Slider masterVolume;

        /// <summary>音乐音量滑块（0-1）</summary>
        [SerializeField] private Slider musicVolume;

        /// <summary>音效音量滑块（0-1）</summary>
        [SerializeField] private Slider sfxVolume;

        /// <summary>编辑草稿：面板打开时从 Current 复制，保存时写回 Current</summary>
        private SettingsData _draft;

        /// <summary>
        /// 准备草稿：确保设置已加载 → 新建草稿 → 从 Current 复制 → 刷新滑块显示。
        /// 由 OnEnable 调用（面板被 UIManager 激活的那一刻）。
        /// </summary>
        private void PrepareDraft()
        {
            // 首次打开时设置可能还没从磁盘读过，这里兜底加载
            if (SettingsSystem.Current == null)
            {
                SettingsSystem.Load();
            }
            // 每次打开都重建草稿：丢弃上次未保存的编辑，保证"取消"语义正确
            _draft = new SettingsData();
            CopyCurrentToDraft();
            RefreshSliders();
        }

        /// <summary>主音量滑块回调：写入草稿（不落盘、不改 Current）</summary>
        public void OnMasterChanged(float v)
        {
            _draft.masterVolume = v;
        }

        /// <summary>音乐音量滑块回调：写入草稿</summary>
        public void OnMusicChanged(float v)
        {
            _draft.musicVolume = v;
        }

        /// <summary>音效音量滑块回调：写入草稿</summary>
        public void OnSfxChanged(float v)
        {
            _draft.sfxVolume = v;
        }

        /// <summary>
        /// 「保存」：草稿写回 Current → 落盘 → 请求返回上一层。
        /// 顺序不可颠倒：必须【先改内存、再写磁盘、最后退界面】，
        /// 否则会出现"保存的是上一次的值"。
        /// </summary>
        public void OnSave()
        {
            ApplyDraftToCurrent();
            SettingsSystem.Save();
            GameManager.Instance.GoBack();
        }

        /// <summary>「取消」：丢弃草稿并返回（Current 与磁盘均不变）</summary>
        public void OnCancel()
        {
            GameManager.Instance.GoBack();
        }

        /// <summary>「恢复默认」：把草稿重置为默认值并刷新滑块（仍需「保存」才生效）</summary>
        public void ResetToDefault()
        {
            _draft.ResetToDefault();
            RefreshSliders();
        }

        /// <summary>把已保存的 Current 复制到草稿（打开面板时的初始状态）</summary>
        private void CopyCurrentToDraft()
        {
            _draft.masterVolume = SettingsSystem.Current.masterVolume;
            _draft.musicVolume = SettingsSystem.Current.musicVolume;
            _draft.sfxVolume = SettingsSystem.Current.sfxVolume;
        }

        /// <summary>把草稿值刷到三个滑块上（用于打开面板与恢复默认）</summary>
        private void RefreshSliders()
        {
            masterVolume.value = _draft.masterVolume;
            musicVolume.value = _draft.musicVolume;
            sfxVolume.value = _draft.sfxVolume;
        }

        /// <summary>把草稿写回 Current（仅内存；落盘由 SettingsSystem.Save 负责）</summary>
        private void ApplyDraftToCurrent()
        {
            SettingsSystem.Current.masterVolume = _draft.masterVolume;
            SettingsSystem.Current.musicVolume = _draft.musicVolume;
            SettingsSystem.Current.sfxVolume = _draft.sfxVolume;
        }

        /// <summary>
        /// 面板被激活时：先准备草稿，再挂监听。
        /// 顺序讲究：先摆好数据再接收玩家输入，避免刷滑块时触发回调写入脏值。
        /// 用代码绑定（AddListener）而非 Inspector 绑定，规避"改名失效"与"参数模式选错"两类坑。
        /// </summary>
        private void OnEnable()
        {
            PrepareDraft();

            masterVolume.onValueChanged.AddListener(OnMasterChanged);
            musicVolume.onValueChanged.AddListener(OnMusicChanged);
            sfxVolume.onValueChanged.AddListener(OnSfxChanged);
        }

        /// <summary>面板被关闭时注销监听，与 OnEnable 严格配对，防止重复监听或幽灵回调</summary>
        private void OnDisable()
        {
            masterVolume.onValueChanged.RemoveListener(OnMasterChanged);
            musicVolume.onValueChanged.RemoveListener(OnMusicChanged);
            sfxVolume.onValueChanged.RemoveListener(OnSfxChanged);
        }
    }
}

