using System;
using Entities;
using Game.SceneAudio;
using Game.Gameplay.Vendors;
using GameSystem;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game.Gameplay.Player
{
    [Serializable]
    public sealed class VendorInteractor
    {
        public event Action<VendorSellResult> OnResourcesSold;

        [ReadOnly]
        [ShowInInspector]
        public float IncomeMultiplier { get; set; } = 1;

        private ResourceStorage resourceStorage;

        private MoneyStorage moneyStorage;

        private MoneyPanelAnimator_AddMoney moneyAnimator;

        [SerializeField]
        private AudioClip saleSFX;

        [GameInject]
        public void Construct(
            ResourceStorage resourceStorage,
            MoneyStorage moneyStorage,
            MoneyPanelAnimator_AddMoney moneyAnimator
        )
        {
            this.resourceStorage = resourceStorage;
            this.moneyStorage = moneyStorage;
            this.moneyAnimator = moneyAnimator;
        }

        public void SellResources(IEntity vendor)
        {
            var vendorInfo = vendor.Get<IComponent_Info>();
            var resourceType = vendorInfo.ResourceType;

            var amount = resourceStorage.GetResource(resourceType);
            if (amount <= 0)
            {
                return;
            }

            var price = vendorInfo.PricePerOne;
            resourceStorage.ExtractResource(resourceType, amount);

            var income = Mathf.RoundToInt(price * amount * IncomeMultiplier);
            moneyStorage.EarnMoney(income);

            var result = new VendorSellResult
            {
                vendor = vendor,
                resourceType = resourceType,
                resourceAmount = amount,
                moneyIncome = income
            };

            PlayParticlesToUI(result, income);
            InteractWithVendor(vendor);
            PlaySound();
            
            OnResourcesSold?.Invoke(result);
        }

        private void InteractWithVendor(IEntity vendor)
        {
            if (vendor.TryGet(out IComponent_CompleteDeal component))
            {
                component.NotifyAboutCompleted();
            }
        }

        private void PlayParticlesToUI(VendorSellResult result, int income)
        {
            var emissionPosition = result.vendor.Get<IComponent_GetParticlePosition>().Position;
            moneyAnimator.PlayIncomeFromWorld(emissionPosition, income);
        }

        private void PlaySound()
        {
            SceneAudioManager.PlaySound(SceneAudioType.INTERFACE, saleSFX);
        }
    }
}