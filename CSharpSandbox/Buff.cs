namespace CSharpSandbox
{
    class Buff
    {
        public Buff(float value, int duration, BuffType type, BuffOperator op = BuffOperator.FLAT)
        {
            this.value = value;
            this.duration = duration;
            this.type = type;
            this.op = op;
        }

        public enum BuffType
        {
            STRENGTH,
            HEALTH,
            CRITICAL_CHANCE,
            CRITICAL_DAMAGE
        }

        public enum BuffOperator
        {
            FLAT, // Stat is increased by a number
            PERCENTAGE, // Stat is increased by a percentage
        }

        // Returns the amount that this buff would increase value by (does not modify value itself)
        public float Apply(float value, int durationPenalty = 1)
        {
            // Reduce the duration
            if(duration != DURATION_MAX) duration -= durationPenalty;

            // If this buff has expired, do not apply any buff
            if (duration <= 0) return 0.0f;
            else if (op == BuffOperator.FLAT) return this.value;
            else if (op == BuffOperator.PERCENTAGE) return this.value * value;

            // Should be unreachable 
            return 0.0f;
        }

        private readonly float value;
        private readonly BuffType type;
        private readonly BuffOperator op;

        private int duration;

        // A duration which will never run out
        public const int DURATION_MAX = int.MaxValue;

        public BuffType Type { get { return type; } }
        public BuffOperator Op { get { return op; } }
        public float Value { get { return value; } }
        public int Duration { get { return duration; } }
    }
}
