namespace CSharpSandbox
{
    class MainProgram
    {
        public static void Main()
        {
            Console.WriteLine("~ Combat Simulator 2021 v0.2 ~\n");

            int initialRatCount = 50;
            int initialOgreCount = 5;

            int ratCount = initialRatCount;
            int ogreCount = initialOgreCount;

            List<Rat> rats = new(ratCount);
            List<Ogre> ogres = new(ogreCount);

            for(int i = 0; i < ratCount; i++)
            {
                rats.Add(new Rat($"Rat #{i + 1}"));
                rats[i].GroupSize = ratCount;
            }

            for (int i = 0; i < ogreCount; i++)
            {
                ogres.Add(new Ogre($"Ogre #{i + 1}"));
            }

            bool combat = true;
            int turnsElapsed = 0;

            Console.WriteLine($"There are {ratCount} rats, up against {ogreCount} ogres. This might get gruesome :O\n\n");
            while (combat)
            {
                Console.WriteLine($"\nTurn {turnsElapsed + 1} beginning ~\n");

                // Let the rats take their turn
                foreach(Rat rat in rats)
                {
                    // Add some spacing
                    Console.Write("\n");

                    Ogre target = ogres[ogreCount - 1];
                    // Attack the closest target (last in the list)
                    rat.Attack(target);

                    if(target.Dead)
                    {
                        // Store the name of the dead ogre for the the next one's special
                        string deadOgreName = target.Name;

                        ogres.RemoveAt(ogreCount - 1);
                        ogreCount--;
                        // Check for victory
                        if (ogreCount == 0)
                        {
                            Console.WriteLine($"\n\nThe rats have won!!! {initialRatCount - ratCount} rats died for the cause.");
                            combat = false;
                            goto End;
                        } else
                        {
                            ogres[ogreCount - 1].SpecialBattleRage(deadOgreName);
                        }
                    }
                }

                // Now the ogres
                foreach (Ogre ogre in ogres)
                {
                    Console.Write("\n");
                    do
                    {
                        Rat target = rats[ratCount - 1];
                        // Cleave attack is configured to disable critical hits
                        if (ogre.IsCleaving) ogre.CleaveAttack(target);
                        else ogre.Attack(target);

                        if (target.Dead)
                        {
                            rats.RemoveAt(ratCount - 1);
                            ratCount--;
                            // Update the rat count
                            foreach (Rat rat in rats)
                            {
                                rat.GroupSize = ratCount;
                            }
                            // Check for victory
                            if (ratCount == 0)
                            {
                                Console.WriteLine($"\n\nThe ogres have won!!! {initialOgreCount - ogreCount} ogres died for the cause.");
                                combat = false;
                                goto End;
                            }
                        }
                    } while (ogre.IsCleaving);                    
                }

                // Increment the turn counter
                turnsElapsed++;

                // Console.WriteLine("\nPress any key to continue the fight...");
                // Console.ReadKey();
            }

            End:

            Console.WriteLine($"The war ended after {turnsElapsed + 1} gruelling turns");

            // Before finishing off
            Console.ReadKey();
        }
    }
}