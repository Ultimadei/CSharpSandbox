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

        public static void Run(string teamAName, string teamBName, int teamARats, int teamASnakes, int teamAOgres, int teamBRats, int teamBSnakes, int teamBOgres)
        {
            Utils.Write(ConsoleColor.Yellow, " ~ Combat Simulator 2021 v0.5 ~\n\n", ConsoleColor.White);

            CreatureTeam teamA = new(teamAName);
            CreatureTeam teamB = new(teamBName);

            teamA.Construct(teamARats, teamASnakes, teamAOgres);
            teamB.Construct(teamBRats, teamBSnakes, teamBOgres);

            bool combat = true;
            int turnsElapsed = 0;

            Console.WriteLine($"Team {teamA.TeamName} consists of {teamARats} rats, {teamASnakes} snakes and {teamAOgres} ogres. They are up against {teamBRats} rats, {teamBSnakes} snakes and {teamBOgres} ogres in Team {teamB.TeamName}");

            while (combat)
            {
                Console.WriteLine($"\nTurn {turnsElapsed + 1} beginning:\n");
                Console.WriteLine($"{teamA.TeamName}: {teamA.RatCount} rats, {teamA.SnakeCount} snakes and {teamA.OgreCount} ogres still alive"); 
                Console.WriteLine($"{teamB.TeamName}: {teamB.RatCount} rats, {teamB.SnakeCount} snakes and {teamB.OgreCount} ogres still alive\n");

                Utils.Write(ConsoleColor.White, "~~~",  ConsoleColor.DarkYellow, $" {teamA.TeamName}'s TURN ", ConsoleColor.White, "~~~\n");
                
                Fight(teamA, teamB);
                teamA.Update();

                if (HandleVictory(teamA, teamB))
                {
                    combat = false;
                    goto EndCombat;
                }

                Utils.Write(ConsoleColor.White, "\n~~~", ConsoleColor.DarkYellow, $" {teamB.TeamName}'s TURN ", ConsoleColor.White, "~~~\n");

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

            int[] creatureDistribution = { 2, 0, 0, 0, 1, 0 };

            string teamAName = "Ratimusan";
            string teamBName = "Ogrimkero";

            Console.WindowWidth = 200;

            while (!done)
            {
                Console.Clear();

                Run(teamAName, teamBName, 
                    creatureDistribution[0], creatureDistribution[1], creatureDistribution[2], 
                    creatureDistribution[3], creatureDistribution[4], creatureDistribution[5]);
                
                Console.Write("ESC: Exit\nUp arrow / Down arrow: Increase / Decrease selected creature count\nLeft arrow / Right arrow: Switch selected creature\nSpacebar: Return to the top of the screen\nEnter: Replay\n\n");

                bool processing = true;
                int selectedIndex = 0;

                // Row of the last line to be printed (for easy return)
                int latestLine = 0;

                while (processing)
                {
                    switch (selectedIndex)
                    {
                        default:
                        case 0:
                            Utils.Write(ConsoleColor.White, 
                                $"{teamAName}: < ", ConsoleColor.Yellow, $"{creatureDistribution[0]}", ConsoleColor.White, $" > rats, {creatureDistribution[1]} snakes, {creatureDistribution[2]} ogres", "\n");

                            Utils.Write(ConsoleColor.White, $"{teamBName}: {creatureDistribution[3]} rats, {creatureDistribution[4]} snakes, {creatureDistribution[5]} ogres");
                            break;
                        case 1:
                            Utils.Write(ConsoleColor.White,
                                $"{teamAName}: {creatureDistribution[0]} rats, < ", ConsoleColor.Yellow, $"{creatureDistribution[1]}", ConsoleColor.White, $" > snakes, {creatureDistribution[2]} ogres", "\n");

                            Utils.Write(ConsoleColor.White, $"{teamBName}: {creatureDistribution[3]} rats, {creatureDistribution[4]} snakes, {creatureDistribution[5]} ogres");
                            break;
                        case 2:
                            Utils.Write(ConsoleColor.White,
                                $"{teamAName}: {creatureDistribution[0]} rats, {creatureDistribution[1]} snakes, < ", ConsoleColor.Yellow, $"{creatureDistribution[2]}", ConsoleColor.White, $" > ogres", "\n");

                            Utils.Write(ConsoleColor.White, $"{teamBName}: {creatureDistribution[3]} rats, {creatureDistribution[4]} snakes, {creatureDistribution[5]} ogres");
                            break;
                        case 3:
                            Utils.Write(ConsoleColor.White, $"{teamAName}: {creatureDistribution[0]} rats, {creatureDistribution[1]} snakes, {creatureDistribution[2]} ogres", "\n");
                            Utils.Write(ConsoleColor.White,
                                $"{teamBName}: < ", ConsoleColor.Yellow, $"{creatureDistribution[3]}", ConsoleColor.White, $" > rats, {creatureDistribution[4]} snakes, {creatureDistribution[5]} ogres");
                            break;
                        case 4:
                            Utils.Write(ConsoleColor.White, $"{teamAName}: {creatureDistribution[0]} rats, {creatureDistribution[1]} snakes, {creatureDistribution[2]} ogres", "\n");
                            Utils.Write(ConsoleColor.White,
                                $"{teamBName}: {creatureDistribution[3]} rats, < ", ConsoleColor.Yellow, $"{creatureDistribution[4]}", ConsoleColor.White, $" > snakes, {creatureDistribution[5]} ogres");
                            break;
                        case 5:
                            Utils.Write(ConsoleColor.White, $"{teamAName}: {creatureDistribution[0]} rats, {creatureDistribution[1]} snakes, {creatureDistribution[2]} ogres", "\n");
                            Utils.Write(ConsoleColor.White,
                                $"{teamBName}: {creatureDistribution[3]} rats, {creatureDistribution[4]} snakes, < ", ConsoleColor.Yellow, $"{creatureDistribution[5]}", ConsoleColor.White, $" > ogres");
                            break;
                    }

                    latestLine = Console.GetCursorPosition().Top;

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
                            if (selectedIndex < 0) selectedIndex = creatureDistribution.Length - 1;
                            break;
                        case ConsoleKey.RightArrow:
                            selectedIndex++;
                            if (selectedIndex >= creatureDistribution.Length) selectedIndex = 0;
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