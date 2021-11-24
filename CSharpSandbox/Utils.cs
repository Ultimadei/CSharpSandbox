namespace CSharpSandbox
{
    internal class Utils
    {
        public static void Write(params object[] oo)
        {
            foreach (var o in oo)
                if (o == null)
                    Console.ResetColor();
                else if (o is ConsoleColor)
                    Console.ForegroundColor = (ConsoleColor)o;
                else
                    Console.Write(o.ToString());
        }

        public static void Shuffle<T>(ref IList<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = RandomNum.rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        public const ConsoleColor DefaultColor = ConsoleColor.Gray;
    }
}
