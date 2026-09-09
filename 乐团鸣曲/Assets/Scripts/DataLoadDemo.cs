using UnityEngine;

namespace YuetanMingqu
{
    public class DataLoadDemo : MonoBehaviour
    {
        [SerializeField] private TextAsset _bandMembersJson;
        [SerializeField] private TextAsset _aberrationJson;
        [SerializeField] private TextAsset _characterJson;
        [SerializeField] private TextAsset _gameConfigJson;

        private void Start()
        {
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

            if (DataManager.LoadAll(_bandMembersJson, _aberrationJson, _characterJson, _gameConfigJson))
            {
                Debug.Log(DataManager.Characters.characters.Count);
                Debug.Log(DataManager.BandMembers.bandMembers.Count);
                Debug.Log(DataManager.Aberrations.aberrations.Count);
                Debug.Log(DataManager.Config.dayLoop.quotaBase);
                Debug.Log(DataManager.GetBandMember("band_ppp_kasumi").name + "\n" + DataManager.GetBandMember("band_ppp_kasumi").linkedAberrationId);
            }
        }
    }

}
