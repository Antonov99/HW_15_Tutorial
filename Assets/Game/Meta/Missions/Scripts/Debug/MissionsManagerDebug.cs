#if UNITY_EDITOR
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game.Meta
{
    public sealed class MissionsManagerDebug : MonoBehaviour
    {
        [SerializeField]
        private MissionsManager manager;

        [PropertySpace(8)]
        [Button]
        private void ReceiveReward(MissionDifficulty difficulty)
        {
            var mission = manager.GetMission(difficulty);
            if (manager.CanReceiveReward(mission))
            {
                manager.ReceiveReward(mission);
                Debug.Log($"RECEIVED REWARD {mission.MoneyReward} FROM MISSION {mission.Id}");
            }
        }

        private void Reset()
        {
            manager = GetComponent<MissionsManager>();
        }
    }
}
#endif