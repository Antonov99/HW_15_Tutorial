using Lessons.Game;

namespace Lessons.Entities.Common.Components
{
    public sealed class WeaponComponent
    {
        public WeaponConfig Value { get; }

        public WeaponComponent(WeaponConfig config)
        {
            Value = config;
        }
    }
}