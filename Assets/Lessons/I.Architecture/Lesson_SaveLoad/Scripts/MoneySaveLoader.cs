using System;
using Game.GameEngine.GameResources;
using Game.Gameplay.Player;
using UnityEngine;

namespace Lessons.I.Architecture.Lesson_SaveLoad
{
    [Serializable]
    public class MoneySaveLoader : SaveLoader<MoneyStorage, MoneyData>
    {
        protected override MoneyData ConvertToData(MoneyStorage service)
        {
            Debug.Log($"<color=yellow>Convert to data = {service.Money}</color>");
            return new MoneyData()
            {
                Money = service.Money
            };
        }

        protected override void SetupData(MoneyStorage service, MoneyData data)
        {
            Debug.Log($"<color=yellow>Setup data = {service.Money}</color>");
            service.SetupMoney(data.Money);
        }

        protected override void SetupDefaultData(MoneyStorage service)
        {
            Debug.Log($"<color=yellow>Setup default data = {100}</color>");
            service.SetupMoney(100);
        }
    }
}