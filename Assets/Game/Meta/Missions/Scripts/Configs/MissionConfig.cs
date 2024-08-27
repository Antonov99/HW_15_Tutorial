using UnityEngine;

namespace Game.Meta
{
    public abstract class MissionConfig : ScriptableObject
    {
        public string Id
        {
            get { return id; }
        }

        public MissionDifficulty Difficulty
        {
            get { return difficulty; }
        }

        public int MoneyReward
        {
            get { return moneyReward; }
        }

        public MissionMetadata Metadata
        {
            get { return metadata; }
        }

        [SerializeField]
        private string id;

        [SerializeField]
        private MissionDifficulty difficulty;

        [SerializeField]
        private int moneyReward;

        [Space]
        [SerializeField]
        private MissionMetadata metadata;

        public abstract Mission InstantiateMission();

        public abstract string Serialize(Mission mission);

        public abstract void DeserializeTo(string serializedData, Mission mission);
    }
}