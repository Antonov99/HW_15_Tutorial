using Game.Gameplay.Player;
using GameSystem;

namespace Game.Gameplay.Hero
{
    public sealed class ConveyorVisitor : TriggerObserver<ConveyorTrigger>
    {
        private ConveyorVisitInteractor visitInteractor;

        [GameInject]
        public void Construct(ConveyorVisitInteractor visitInteractor)
        {
            this.visitInteractor = visitInteractor;
        }

        protected override void OnHeroEntered(ConveyorTrigger target)
        {
            var zoneType = target.Zone;
            if (zoneType == ConveyorTrigger.ZoneType.LOAD)
            {
                EnterLoadZone(target);
            }

            if (zoneType == ConveyorTrigger.ZoneType.UNLOAD)
            {
                EnterUnloadZone(target);
            }
        }

        protected override void OnHeroExited(ConveyorTrigger target)
        {
            var zoneType = target.Zone;
            if (zoneType == ConveyorTrigger.ZoneType.LOAD)
            {
                visitInteractor.InputZone.Exit();
            }

            if (zoneType == ConveyorTrigger.ZoneType.UNLOAD)
            {
                visitInteractor.OutputZone.Exit();
            }
        }

        private void EnterLoadZone(ConveyorTrigger trigger)
        {
            var inputZone = visitInteractor.InputZone;
            if (inputZone.IsEntered)
            {
                inputZone.Exit();
            }
            
            inputZone.Enter(trigger.Conveyor);
        }

        private void EnterUnloadZone(ConveyorTrigger trigger)
        {
            var outputZone = visitInteractor.OutputZone;
            if (outputZone.IsEntered)
            {
                outputZone.Exit();
            }
            
            outputZone.Enter(trigger.Conveyor);
        }
    }
}