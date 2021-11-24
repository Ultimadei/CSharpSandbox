namespace CSharpSandbox
{
    class CreatureTeam
    {
        public CreatureTeam(string teamName)
        {
            this.teamName = teamName;

            ratList = new List<Rat>();
            snakeList = new List<Snake>();
            ogreList = new List<Ogre>();
            leechList = new List<Leech>();
            goblinList = new List<Goblin>();

            creatureList = new List<Creature>();
        }

        public void Shuffle()
        {
            Utils.Shuffle(ref creatureList);
        }

        public void Construct(int[] creatureDistribution)
        {
            int ratCount = creatureDistribution[0];
            int ogreCount = creatureDistribution[1];
            int snakeCount = creatureDistribution[2];
            int leechCount = creatureDistribution[3];
            int goblinCount = creatureDistribution[4];

            int total = ratCount + snakeCount + ogreCount;

            for (int i = 0; i < ratCount; i++)
            {
                ratList.Add(new Rat(this, $"{teamName} Rat #{i + 1}", total));
            }

            for (int i = 0; i < snakeCount; i++)
            {
                snakeList.Add(new Snake(this, $"{teamName} Snake #{i + 1}"));
            }

            for (int i = 0; i < ogreCount; i++)
            {
                ogreList.Add(new Ogre(this, $"{teamName} Ogre #{i + 1}"));
            }

            for (uint i = 0; i < leechCount; i++)
            {
                leechList.Add(new Leech(this, $"{teamName} Leech #{i + 1}"));
            }

            for (uint i = 0; i < goblinCount; i++)
            {
                goblinList.Add(new Goblin(this, $"{teamName} Goblin #{i + 1}"));
            }

            UpdateListContents();
        }

        public void RegisterDeath(Creature deadCreature)
        {
            // Remove the creature from the appropriate list
            if (ratList.Contains(deadCreature)) ratList.Remove((Rat)deadCreature);
            else if (snakeList.Contains(deadCreature)) snakeList.Remove((Snake)deadCreature);
            else if (ogreList.Contains(deadCreature)) ogreList.Remove((Ogre)deadCreature);
            else if (leechList.Contains(deadCreature)) leechList.Remove((Leech)deadCreature);
            else if (goblinList.Contains(deadCreature)) goblinList.Remove((Goblin)deadCreature);

            // And remove it from the main list
            bool test = creatureList.Remove(deadCreature);

            // Then let everyone know it's dead

            foreach(Creature creature in creatureList)
            {
                creature.OnAllyDeath(deadCreature);
            }
        }

        public void Update()
        {
            UpdateListContents();

            foreach(Creature creature in creatureList)
            {
                creature.OnTeamUpdate();
            }
        }

        private void UpdateListContents()
        {
            creatureList.Clear();
            creatureList = creatureList.Concat(ratList).ToList();
            creatureList = creatureList.Concat(snakeList).ToList();
            creatureList = creatureList.Concat(ogreList).ToList();
            creatureList = creatureList.Concat(leechList).ToList();
            creatureList = creatureList.Concat(goblinList).ToList();
        }

        private readonly string teamName;

        private IList<Rat> ratList;
        private IList<Snake> snakeList;
        private IList<Ogre> ogreList;
        private IList<Leech> leechList;
        private IList<Goblin> goblinList;

        public IList<Creature> creatureList;

        public static string[] creatureNamesPlural = { "rats", "ogres", "snakes", "leeches", "goblins" };
        public static string[] creatureNamesSingular = { "Rat", "Ogre", "Snake", "Leech", "Goblin" };
        public static int creatureTypeCount = creatureNamesSingular.Length;

        public string TeamName { get { return teamName; } }
        public int Count { get { return creatureList.Count;} }
        public int RatCount { get { return ratList.Count; } }
        public int SnakeCount { get { return snakeList.Count; } }
        public int OgreCount { get { return ogreList.Count; } }
        public int LeachCount { get { return leechList.Count; } }
        public int GoblinCount { get { return goblinList.Count; } }
    }
}
