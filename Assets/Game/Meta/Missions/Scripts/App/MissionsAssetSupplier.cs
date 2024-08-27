using Game.App;
using UnityEngine;

namespace Game.Meta
{
    public sealed class MissionsAssetSupplier : IConfigLoader
    {
        private MissionCatalog catalog;

        public MissionConfig GetMission(string id)
        {
            return catalog.FindMission(id);
        }

        public MissionConfig[] GetMissions(MissionDifficulty difficulty)
        {
            return catalog.FindMissions(difficulty);
        }

        public MissionConfig[] GetAllMissions()
        {
            return catalog.GetAllMissions();
        }

        void IConfigLoader.LoadConfigs()
        {
            catalog = Resources.Load<MissionCatalog>(MissionExtensions.MISSION_CATALOG_RESOURCE_PATH);
        }
    }
}