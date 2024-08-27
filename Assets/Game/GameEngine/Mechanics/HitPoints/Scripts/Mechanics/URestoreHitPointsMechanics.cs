using Elementary;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game.GameEngine.Mechanics
{
    [AddComponentMenu("GameEngine/Mechanics/Hit Points/Mechanics «Restore Hit Points»")]
    public sealed class URestoreHitPointsMechanics : MonoBehaviour
    {
        [SerializeField]
        public UTakeDamageEngine takeDamageEngine;

        [SerializeField]
        public UHitPoints hitPointsEngine;

        [SerializeField]
        [FormerlySerializedAs("countdown")]
        public MonoCountdown delay;

        [SerializeField]
        public MonoPeriod restorePeriod;

        [Space]
        [SerializeField]
        [FormerlySerializedAs("restoreHitPoinsPerOne")]
        public IntAdapter restoreAtTime;

        private void OnEnable()
        {
            takeDamageEngine.OnDamageTaken += OnDamageTaken;
            delay.OnEnded += OnDelayEnded;
            restorePeriod.OnPeriodEvent += OnRestoreHitPoints;
        }

        private void OnDisable()
        {
            takeDamageEngine.OnDamageTaken -= OnDamageTaken;
            delay.OnEnded -= OnDelayEnded;
            restorePeriod.OnPeriodEvent -= OnRestoreHitPoints;
        }

        private void OnDamageTaken(TakeDamageArgs damageArgs)
        {
            if (hitPointsEngine.Current <= 0)
            {
                return;
            }

            //Сброс задержки:
            delay.ResetTime();
            if (!delay.IsPlaying)
            {
                delay.Play();
            }

            //Сброс периода:
            restorePeriod.Stop();
        }

        private void OnDelayEnded()
        {
            restorePeriod.Play();
        }

        private void OnRestoreHitPoints()
        {
            hitPointsEngine.Current += restoreAtTime.Current;
            if (hitPointsEngine.Current >= hitPointsEngine.Max)
            {
                restorePeriod.Stop();
            }
        }
    }
}