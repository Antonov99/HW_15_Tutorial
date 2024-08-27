using System;
using Elementary;
using Game.GameEngine.GameResources;
using Declarative;
using UnityEngine;

namespace Game.Gameplay.Conveyors
{
    public sealed class ConveyorModel : DeclarativeModel
    {
        [Section]
        [SerializeField]
        public ScriptableConveyour config;

        [Section]
        [SerializeField]
        private ResourceInfoCatalog resourceCatalog;

        [Section]
        [SerializeField, Space]
        public Core core;

        [Section]
        [SerializeField]
        private Visual visual;

        [Section]
        [SerializeField]
        private Canvas canvas;

        [Serializable]
        public sealed class Core
        {
            [SerializeField]
            public BoolVariable enableVariable = new();

            [SerializeField, Space]
            public IntVariableLimited loadStorage = new();

            [SerializeField]
            public IntVariableLimited unloadStorage = new();

            [SerializeField, Space]
            public Timer workTimer = new();

            public WorkMechanics workMechanics = new();

            [Construct]
            private void ConstructStorages(ScriptableConveyour config)
            {
                loadStorage.MaxValue = config.inputCapacity;
                unloadStorage.MaxValue = config.outputCapacity;
            }

            [Construct]
            private void ConstructWork(ScriptableConveyour config)
            {
                workTimer.Duration = config.workTime;
                workMechanics.Construct(
                    isEnable: enableVariable,
                    loadStorage: loadStorage,
                    unloadStorage: unloadStorage,
                    workTimer: workTimer
                );
            }
        }
        
        [Serializable]
        public sealed class Visual
        {
            [SerializeField]
            private ConveyorVisual conveyorView;

            [SerializeField]
            private ZoneVisual loadZoneView;

            [SerializeField]
            private ZoneVisual unloadZoneView;

            private readonly ConveyorVisualAdapter conveyorViewAdapter = new();

            private readonly ZoneVisualAdapter loadZoneViewAdapter = new();

            private readonly ZoneVisualAdapter unloadZoneViewAdapter = new();

            [Construct]
            private void Construct(Core core)
            {
                conveyorViewAdapter.Construct(core.workTimer, conveyorView);
                loadZoneViewAdapter.Construct(core.loadStorage, loadZoneView);
                unloadZoneViewAdapter.Construct(core.unloadStorage, unloadZoneView);
            }
        }

        [Serializable]
        public sealed class Canvas
        {
            [SerializeField]
            private InfoWidget infoView;

            private readonly InfoWidgetAdapter infoViewAdapter = new();

            [Construct]
            private void Construct(ScriptableConveyour config, ResourceInfoCatalog resourceCatalog, Core core)
            {
                infoViewAdapter.Construct(core.workTimer, infoView);

                var inputType = config.inputResourceType;
                var inputIcon = resourceCatalog.FindResource(inputType).icon;
                infoView.SetInputIcon(inputIcon);

                var outputType = config.outputResourceType;
                var outputIcon = resourceCatalog.FindResource(outputType).icon;
                infoView.SetOutputIcon(outputIcon);
            }
        }
    }
}