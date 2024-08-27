using System;

namespace Game.GameEngine.Mechanics
{
    public sealed class Component_HitPoints :
        IComponent_GetHitPoints,
        IComponent_SetHitPoints,
        IComponent_GetMaxHitPoints,
        IComponent_SetMaxHitPoints,
        IComponent_AddHitPoints,
        IComponent_OnHitPointsChanged,
        IComponent_OnMaxHitPointsChanged,
        IComponent_SetupHitPoints
    {
        public event Action<int> OnHitPointsChanged
        {
            add { engine.OnCurrentPointsChanged += value; }
            remove { engine.OnCurrentPointsChanged -= value; }
        }

        public event Action<int> OnMaxHitPointsChanged
        {
            add { engine.OnMaxPointsChanged += value; }
            remove { engine.OnMaxPointsChanged -= value; }
        }

        public int HitPoints
        {
            get { return engine.Current; }
        }

        public int MaxHitPoints
        {
            get { return engine.Max; }
        }

        private readonly IHitPoints engine;

        public Component_HitPoints(IHitPoints engine)
        {
            this.engine = engine;
        }

        public void Setup(int current, int max)
        {
            engine.Setup(current, max);
        }

        public void SetHitPoints(int hitPoints)
        {
            engine.Current = hitPoints;
        }

        public void SetMaxHitPoints(int hitPoints)
        {
            engine.Max = hitPoints;
        }

        public void AddHitPoints(int range)
        {
            engine.Current += range;
        }
    }
}