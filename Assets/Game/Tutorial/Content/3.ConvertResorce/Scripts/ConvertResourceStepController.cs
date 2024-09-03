﻿using Game.Gameplay.Hero;
using Game.Gameplay.Player;
using Game.Tutorial.Gameplay;
using Game.Tutorial.UI;
using GameSystem;
using UnityEngine;

namespace Game.Tutorial
{
    [AddComponentMenu("Tutorial/Step «Convert Resource»")]
    public sealed class ConvertResourceStepController : TutorialStepController
    {
        private PointerManager pointerManager;
        
        private NavigationManager navigationManager;

        private ScreenTransform screenTransform;
        
        private readonly ConvertResourceInspector inspector = new();

        [SerializeField]
        private ConvertResourceConfig config;

        [SerializeField]
        private ConvertResourcePanelShower panelShower = new();

        [SerializeField]
        private Transform pointerTransform;

        public override void ConstructGame(GameContext context)
        {
            pointerManager = context.GetService<PointerManager>();
            navigationManager = context.GetService<NavigationManager>();
            screenTransform = context.GetService<ScreenTransform>();
            
            var conveyorVisitInputZoneObserver = context.GetService<ConveyorVisitInputZoneObserver>();
            inspector.Construct(conveyorVisitInputZoneObserver);
            
            panelShower.Construct(config);

            base.ConstructGame(context);
        }

        protected override void OnStart()
        {
            inspector.Inspect(callback: NotifyAboutCompleteAndMoveNext);
            
            var targetPosition = pointerTransform.position;
            pointerManager.ShowPointer(targetPosition, pointerTransform.rotation);
            navigationManager.StartLookAt(targetPosition);
            
            panelShower.Show(screenTransform.Value);
        }

        protected override void OnStop()
        {
            panelShower.Hide();
            pointerManager.HidePointer();
        }
    }
}