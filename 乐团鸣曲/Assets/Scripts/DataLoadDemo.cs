using UnityEngine;

namespace YuetanMingqu
{
    /// <summary>
    /// M1 数据加载验证脚本（临时脚本）。
    /// 作用：把四份 JSON 拖进 Inspector 槽位，Play 后在 Console 打印，确认数据层读得通。
    /// 后续由正式引导流程替换（BOOT 阶段在 GameManager 中调用 DataManager.LoadAll）。
    /// 为什么继承 MonoBehaviour：需要挂在场景物体上才会执行 Start()，且需要 Inspector 面板来拖文件引用。
    /// </summary>
    public class DataLoadDemo : MonoBehaviour
    {
        // 以下四个字段在 Unity Inspector 中拖入 Assets/Data 下对应的 .json 文件。
        // [SerializeField] 的作用：让 private 字段显示在 Inspector 上，既保持封装又能拖引用。

        /// <summary>乐队少女数据（BandMembers.json）</summary>
        [SerializeField] private TextAsset _bandMembersJson;

        /// <summary>异想体数据（Aberrations.json）</summary>
        [SerializeField] private TextAsset _aberrationJson;

        /// <summary>普通员工数据（Characters.json）</summary>
        [SerializeField] private TextAsset _characterJson;

        /// <summary>全局配置（GameConfig.json）</summary>
        [SerializeField] private TextAsset _gameConfigJson;

        [SerializeField] private TextAsset _bandNodeJson;

        /// <summary>
        /// 场景启动后：先校验四个文件引用，再加载数据，最后打印结果。
        /// Start 由 Unity 在第一帧更新前自动调用一次（agent.md §4.2：引用须校验）。
        /// </summary>
        private void Start()
        {
            // ① 引用校验：任何一个没拖就报错并中止，避免后续空引用崩溃
            if (_aberrationJson == null)
            {
                Debug.LogError("未挂载异想体数据");
                return;
            }

            if (_bandMembersJson == null)
            {
                Debug.LogError("未挂载乐队少女数据");
                return;
            }

            if (_characterJson == null)
            {
                Debug.LogError("未挂载普通员工数据数据");
                return;
            }

            if (_gameConfigJson == null)
            {
                Debug.LogError("未挂载游戏配置数据");
                return;
            }

            if (_bandNodeJson == null)
            {
                Debug.LogError("未挂载部门数据");
                return;
            }

            // ② 加载：注意实参顺序要与 DataManager.LoadAll 的形参顺序一致
            //    （bandMembers, aberrations, characters, config）
            if (DataManager.LoadAll(_bandMembersJson, _aberrationJson, _characterJson, _gameConfigJson,_bandNodeJson))
            {
                // ③ 打印验证结果：数量 + 配置值 + 按 id 查询
                Debug.Log(DataManager.Characters.characters.Count);
                Debug.Log(DataManager.BandMembers.bandMembers.Count);
                Debug.Log(DataManager.Aberrations.aberrations.Count);
                Debug.Log(DataManager.Config.dayLoop.quotaBase);
                Debug.Log(DataManager.GetBandMember("band_ppp_kasumi").name + "\n" + DataManager.GetBandMember("band_ppp_kasumi").linkedAberrationId);
                Debug.Log($"Bands 条数：{DataManager.BandNodes.bandNodes.Count}");
                Debug.Log($"节点1：nodeIndex={DataManager.BandNodes.bandNodes[0].nodeIndex} " +
                          $"unlockDay={DataManager.BandNodes.bandNodes[0].unlockDay} " +
                          $"leader={DataManager.BandNodes.bandNodes[0].leader} " +
                          $"leaderId={DataManager.BandNodes.bandNodes[0].leaderId}");
            }
        }
    }

}
