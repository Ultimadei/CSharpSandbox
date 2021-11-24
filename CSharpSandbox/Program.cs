namespace CSharpSandbox
{
    class MainProgram
    {
        private static void Fight(CreatureTeam offense, CreatureTeam defence)
        {
            // Creatures can die while fighting, so copy the creatures into a safe array to iterate over
            Creature[] offenseList = new Creature[offense.Count];
            offense.creatureList.CopyTo(offenseList, 0);

            foreach (Creature creature in offenseList)
            {
                // Add some spacing
                Console.Write("\n");

                creature.Attack(defence);

                // Stop as soon as all the defence have died
                if (defence.Count == 0) break;
            }
        }

        // Checks if either of the teams have won and outputs results accordingly. Returns true if either team won
        private static bool HandleVictory(CreatureTeam teamA, CreatureTeam teamB)
        {
            Console.Write("\n");

            // Team B victory
            if (teamA.Count == 0)
            {
                Console.WriteLine($"Team {teamB.TeamName} has won!!!");
                return true;
            } else if (teamB.Count == 0) // Team A victory
            {
                Console.WriteLine($"Team {teamA.TeamName} has won!!!");
                return true;
            }

            return false;
        }

        public static void Run(string teamAName, string teamBName, int[] creatureDistribution)
        {
            Utils.Write(ConsoleColor.Yellow, " ~ Combat Simulator 2021 v0.6 ~\n\n", ConsoleColor.White);

            int creatureTypeCount = CreatureTeam.creatureTypeCount;

            int[] teamACreatureDistribution = new int[creatureTypeCount];
            int[] teamBCreatureDistribution = new int[creatureTypeCount];

            for (int i = 0; i < creatureTypeCount; i++)
            {
                teamACreatureDistribution[i] = creatureDistribution[i];
            }

            for (int i = 0; i < creatureTypeCount; i++)
            {
                // Team B comes second in the distribution array parameter
                teamBCreatureDistribution[i] = creatureDistribution[i + creatureTypeCount];
            }

            CreatureTeam teamA = new(teamAName);
            CreatureTeam teamB = new(teamBName);

            teamA.Construct(teamACreatureDistribution);
            teamB.Construct(teamBCreatureDistribution);

            bool combat = true;
            int turnsElapsed = 0;

            for(uint i = 0; i <= 1; i++)
            {
                if (i == 0) Utils.Write($"Team {teamA.TeamName} consists of ");
                else Utils.Write($"Team {teamB.TeamName} consists of ");

                // List the creatures and their counts
                for (uint j = 0; j < creatureTypeCount; j++)
                {
                    Utils.Write(ConsoleColor.Yellow, $"{creatureDistribution[j + (i * creatureTypeCount)]}", 
                        ConsoleColor.White, $" {CreatureTeam.creatureNamesPlural[j]}");

                    // Grammar
                    if (j < creatureTypeCount - 2)
                    {
                        Utils.Write(", ");
                    }
                    else if (j < creatureTypeCount - 1) Utils.Write(" and ");
                }
                Utils.Write("\n");
            }

            while (combat)
            {
                Console.WriteLine($"\nTurn {turnsElapsed + 1} beginning:\n");

                for (uint i = 0; i <= 1; i++)
                {
                    if (i == 0) Utils.Write($"{teamA.TeamName}: ");
                    else Utils.Write($"{teamB.TeamName}: ");

                    // List the creatures and their counts
                    for (uint j = 0; j < creatureTypeCount; j++)
                    {
                        Utils.Write(ConsoleColor.Yellow, $"{creatureDistribution[j + (i * creatureTypeCount)]}",
                            ConsoleColor.White, $" {CreatureTeam.creatureNamesPlural[j]}");

                        // Grammar
                        if (j < creatureTypeCount - 2)
                        {
                            Utils.Write(", ");
                        }
                        else if (j < creatureTypeCount - 1) Utils.Write(" and ");
                    }

                    Utils.Write(" are still alive\n");
                }

                Utils.Write(ConsoleColor.White, "\n~~~",  ConsoleColor.DarkYellow, $" {teamA.TeamName}'s TURN ", ConsoleColor.White, "~~~\n");
                
                Fight(teamA, teamB);
                teamA.Update();

                if (HandleVictory(teamA, teamB))
                {
                    combat = false;
                    goto EndCombat;
                }

                Utils.Write(ConsoleColor.White, "~~~", ConsoleColor.DarkYellow, $" {teamB.TeamName}'s TURN ", ConsoleColor.White, "~~~\n");

                Fight(teamB, teamA);
                teamB.Update();

                combat = !HandleVictory(teamA, teamB);

            EndCombat:

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

            int creatureTypeCount = CreatureTeam.creatureTypeCount;
            int[] creatureDistribution = new int[creatureTypeCount * 2];

            // Set a rat for team A
            creatureDistribution[0] = 1;
            // Set a rat for team B
            creatureDistribution[creatureTypeCount + 1] = 1;

            string teamAName = "Ratimusan";
            string teamBName = "Ogrimkero";

            Console.WindowWidth = 200;

            while (!done)
            {
                Console.Clear();

                Run(teamAName, teamBName, creatureDistribution);
                
                Console.Write("ESC: Exit\nUp arrow / Down arrow: Increase / Decrease selected creature count\nLeft arrow / Right arrow: Switch selected creature\nSpacebar: Return to the top of the screen\nEnter: Replay\n\n");

                bool processing = true;
                int selectedIndex = 0;

                // Row of the last line to be printed (for easy return)
                int latestLine = 0;

                while (processing)
                {
                    // i == 1 corresponds to team A, then 2 -> team B
                    for(uint i = 0; i <= 1; i++)
                    {
                        if (i == 0) Utils.Write(ConsoleColor.White, $"{teamAName}: ");
                        else Utils.Write(ConsoleColor.White, $"{teamBName}: ");

                        // Iterate over each of the creature types
                        for(uint j = 0; j < creatureTypeCount; j++)
                        {
                            long currentIndex = (i * creatureTypeCount) + j;

                            // If the selected index matches the team and creature [i, j]
                            if (selectedIndex == currentIndex)
                            {
                                // Then print < > parentheses around the text and color it
                                Utils.Write(ConsoleColor.White, "< ");
                                Utils.Write(ConsoleColor.Yellow, $"{creatureDistribution[currentIndex]}");
                                Utils.Write(ConsoleColor.White, " > ");
                            }
                            else Utils.Write(ConsoleColor.White, $"{creatureDistribution[currentIndex]}");

                            Utils.Write(ConsoleColor.White, $" {CreatureTeam.creatureNamesPlural[j]}");
                            // If there are more creatures, use good grammar
                            if(j < creatureTypeCount - 1) Utils.Write(", ");
                        }
                        Utils.Write("\n");
                    }

                    // Subtract one because one more newline was written than was necessary
                    latestLine = Console.GetCursorPosition().Top - 1;

                    ReadKey:

                    switch (Console.ReadKey(true).Key)
                    {
                        case ConsoleKey.Escape:
                            done = true;
                            processing = false;
                            break;
                        case ConsoleKey.UpArrow:
                            creatureDistribution[selectedIndex]++;
                            break;
                        case ConsoleKey.DownArrow:
                            creatureDistribution[selectedIndex]--;
                            if (creatureDistribution[selectedIndex] < 0) creatureDistribution[selectedIndex] = 0;
                            break;
                        case ConsoleKey.LeftArrow:
                            selectedIndex--;
                            if (selectedIndex < 0) selectedIndex = (creatureTypeCount * 2) - 1;
                            break;
                        case ConsoleKey.RightArrow:
                            selectedIndex++;
                            if (selectedIndex >= creatureTypeCount * 2) selectedIndex = 0;
                            break;
                        case ConsoleKey.Enter:
                            processing = false;
                            break;
                        case ConsoleKey.Spacebar:
                            Console.SetCursorPosition(0, 0);
                            goto ReadKey;
                        default:
                            goto ReadKey;
                    }

                    // Return to end position
                    Console.SetCursorPosition(0, latestLine);

                    // Clear the two lines
                    Console.Write("\r" + new string(' ', Console.BufferWidth - 1) + "\r");
                    Console.SetCursorPosition(0, Console.CursorTop - 1);
                    Console.Write("\r" + new string(' ', Console.BufferWidth - 1) + "\r");
                }
            }

            return 0;
        }
    }
}