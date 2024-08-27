using UnityEngine;

namespace CustomParticles
{
    /// <summary>
    ///     <para>Moves from previous to next value by particle chunks.</para>
    /// </summary>
    public sealed class FloatValueIterator : IValueIterator<float>
    {
        public int ParticleCount
        {
            get { return particleCount; }
        }

        public float CurrentValue
        {
            get { return currentValue; }
        }

        private readonly int particleCount;

        private readonly float valuePerParticle;

        private readonly float lastValuePerParticle;

        private float currentValue;

        private int pointer;

        public FloatValueIterator(float previousValue, float newValue, int particleCount)
        {
            currentValue = previousValue;

            var diff = newValue - previousValue;
            if (diff == 0 || particleCount == 0)
            {
                return;
            }

            var valuePerParticle = diff / particleCount;
            if (Mathf.Abs(valuePerParticle) > 0)
            {
                this.valuePerParticle = valuePerParticle;
                lastValuePerParticle = this.valuePerParticle + diff % particleCount;
                this.particleCount = particleCount;
            }
            else
            {
                this.valuePerParticle = Mathf.Sign(diff);
                lastValuePerParticle = this.valuePerParticle;
                this.particleCount = Mathf.RoundToInt(diff);
            }
        }

        public bool NextValue(out float value)
        {
            if (pointer > particleCount)
            {
                value = 0;
                return false;
            }

            pointer++;
            if (pointer < particleCount)
            {
                value = valuePerParticle;
                currentValue += value;
            }
            else
            {
                value = lastValuePerParticle;
                currentValue += value;
            }

            return true;
        }
    }
}