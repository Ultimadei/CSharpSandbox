namespace CSharpSandbox
{
    class RandomNum
    {
        public static int RandomInteger(int min, int max)
        {
            // Range is min (inclusive) to max (exclusive), so +1 to the upper bound in order to include max
            return rng.Next(min, max + 1);
        }

        public static double RandomDouble(double min, double max)
        {
            return rng.NextDouble() * (max - min) + min;
        }

        public static float RandomFloat(float min, float max)
        {
            return (float)RandomDouble(min, max);
        }

        public static bool RandomBool(float chance)
        {
            return chance == 0.0 ? false : RandomDouble(0.0, 1.0) <= chance;
        }

        public static readonly Random rng = new();
    }
}
