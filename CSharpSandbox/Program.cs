namespace CSharpSandbox
{
    class MainProgram
    {
        public static void Fight(List<Creature> offense, List<Creature> defence)
        {
            foreach(Creature creature in offense)
            {
                // Add some spacing
                Console.Write("\n");

                creature.Attack(defence);                

                // Stop as soon as all the defence have died
                if (defence.Count == 0) break;
            }
        }

        public static void Run(int ratCount, int ogreCount)
        {
            Utils.Write(ConsoleColor.Yellow, "~ Combat Simulator 2021 v0.3 ~\n\n", ConsoleColor.White);

            List<Creature> rats = new(ratCount);
            List<Creature> ogres = new(ogreCount);

            for (int i = 0; i < ratCount; i++)
            {
                rats.Add(new Rat($"Rat #{i + 1}", ratCount));
            }

            for (int i = 0; i < ogreCount; i++)
            {
                ogres.Add(new Ogre($"Ogre #{i + 1}"));
            }

            bool combat = true;
            int turnsElapsed = 0;

            Utils.Write("There are ", ConsoleColor.Blue, $"{ratCount}", ConsoleColor.White, 
                " rats, up against ", ConsoleColor.Blue, $"{ogreCount}", ConsoleColor.White, " ogres. This might get gruesome!!\n\n");
            while (combat)
            {
                Utils.Write(ConsoleColor.White, "\nTurn ", ConsoleColor.Blue, $"{turnsElapsed + 1}", ConsoleColor.White,
                    " beginning ~ ", ConsoleColor.Blue, $"{rats.Count}", ConsoleColor.White,
                " rats and ", ConsoleColor.Blue, $"{ogres.Count}", ConsoleColor.White, " ogres are still alive\n\n");

                Utils.Write(ConsoleColor.White, "~~~",  ConsoleColor.DarkYellow, "RAT TURN", ConsoleColor.White, "~~~\n");
                Fight(rats, ogres);
                if (ogres.Count > 0) Utils.Write(ConsoleColor.White, "\n~~~", ConsoleColor.DarkYellow, "OGRE TURN", ConsoleColor.White, "~~~\n");
                Fight(ogres, rats);

                if (ogres.Count == 0)
                {
                    Utils.Write(ConsoleColor.White, "\nThe rats have won!!! ",
                        ConsoleColor.Blue, $"{ratCount - rats.Count}", ConsoleColor.White,
                        " rats died for the cause\n"
                    );
                    combat = false;
                }
                else if (rats.Count == 0)
                {
                    Utils.Write(ConsoleColor.White, "\nThe ogres have won!!! ",
                        ConsoleColor.Blue, $"{ogreCount - ogres.Count}", ConsoleColor.White,
                        " ogres died for the cause\n"
                    );
                    combat = false;
                }

                // Increment the turn counter
                turnsElapsed++;
            }

            Utils.Write(ConsoleColor.White, "The war ended after ", 
                ConsoleColor.Blue, $"{turnsElapsed}", ConsoleColor.White, 
                " gruelling turns\n\n"
            );
        }

        public static int Main()
        {
            bool done = false;

            int ratCount = 25;
            int ogreCount = 3;

            while (!done)
            {
                Console.Clear();

                Run(ratCount, ogreCount);
                Utils.Write(ConsoleColor.White, "\nPress ESC to exit, Up arrow to increase creature count, Down arrow to decrease creature count or the Left or Right arrows to switch between selected creature\n");

                bool processing = true;
                bool ratsSelected = true;

                while (processing)
                {
                    if(ratsSelected) Utils.Write(ConsoleColor.White, "Current Armies: < ", ConsoleColor.Yellow, $"{ratCount}", ConsoleColor.White,
                        $" > rats, {ogreCount} ogres");
                    else Utils.Write(ConsoleColor.White, $"Current Armies: {ratCount} rats, < ", ConsoleColor.Yellow, $"{ogreCount}", ConsoleColor.White,
                        $" > ogres");

                    switch (Console.ReadKey(false).Key)
                    {
                        case ConsoleKey.Escape:
                            done = true;
                            processing = false;
                            break;
                        case ConsoleKey.UpArrow:
                            if (ratsSelected) ratCount++;
                            else ogreCount++;
                            break;
                        case ConsoleKey.DownArrow:
                            if (ratsSelected) ratCount--;
                            else ogreCount--;

                            if (ratCount < 1) ratCount = 1;
                            if (ogreCount < 1) ogreCount = 1;
                            break;
                        case ConsoleKey.LeftArrow:
                        case ConsoleKey.RightArrow:
                            ratsSelected = !ratsSelected;
                            break;
                        default:
                            processing = false;
                            break;
                    }

                    // Clear the line
                    Console.Write("\r" + new string(' ', Console.BufferWidth - 1) + "\r");
                }
            }

            return 0;
        }
    }
}