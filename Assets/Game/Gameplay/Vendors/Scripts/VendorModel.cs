using System;
using Elementary;
using Entities;
using Game.GameEngine;
using Game.GameEngine.GameResources;
using JetBrains.Annotations;
using Declarative;
using UnityEngine;

namespace Game.Gameplay.Vendors
{
    public sealed class VendorModel : DeclarativeModel
    {
        [Section]
        [SerializeField]
        private ScriptableVendor config;

        [Section]
        [SerializeField]
        private ResourceInfoCatalog resourceCatalog;

        [Section]
        [SerializeField, Space]
        private Core core;

        [Section]
        [SerializeField]
        private Components components;

        [Section]
        [SerializeField]
        private Visual visual;

        [Section]
        [SerializeField]
        private Canvas canvas;
        
        [Serializable]
        private sealed class Core
        {
            public Emitter dealEmitter = new();
        }

        [Serializable]
        private sealed class Components
        {
            [SerializeField]
            private MonoEntityStd entity;

            [SerializeField]
            private Transform particlePoint;

            [Construct]
            private void Construct(ScriptableVendor config, Core core)
            {
                entity.Add(new Component_Info(config));
                entity.Add(new Component_ObjectType(config.objectType));
                entity.Add(new Component_CompleteDeal(core.dealEmitter));
                entity.Add(new Component_GetParticlePosition(particlePoint));
            }
        }

        [Serializable]
        private sealed class Visual
        {
            [SerializeField]
            private Animator animator;
            
            [SerializeField]
            private string dealAnimation = "Dance";

            [Construct]
            private void Construct(Core core)
            {
                core.dealEmitter.AddListener(() => animator.Play(dealAnimation, -1, 0));
            }
        }

        [Serializable]
        private sealed class Canvas
        {
            [SerializeField]
            private InfoWidget infoView;

            [SerializeField]
            private RectTransform moveTransform;

            private readonly UIParryMechanics parryMechanics = new();

            [Construct]
            private void ConstructView(ScriptableVendor config, ResourceInfoCatalog resourceCatalog)
            {
                var resourceType = config.resourceType;
                var pricePerResource = config.pricePerOne;
                var resourceIcon = resourceCatalog.FindResource(resourceType).icon;
                infoView.SetPrice(pricePerResource.ToString());
                infoView.SetIcon(resourceIcon);
            }

            [Construct]
            private void ConstructParry()
            {
                parryMechanics.moveTransform = moveTransform;
            }
        }
    }
}