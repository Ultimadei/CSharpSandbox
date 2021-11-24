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

            creatureList = new List<Creature>();
        }

        public void Shuffle()
        {
            Utils.Shuffle(ref creatureList);
        }

        public void Construct(int ratCount, int snakeCount, int ogreCount)
        {
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

            UpdateListContents();
        }

        public void RegisterDeath(Creature deadCreature)
        {
            // Remove the creature from the appropriate list
            if (ratList.Contains(deadCreature)) ratList.Remove((Rat)deadCreature);
            else if (snakeList.Contains(deadCreature)) snakeList.Remove((Snake)deadCreature);
            else if (ogreList.Contains(deadCreature)) ogreList.Remove((Ogre)deadCreature);

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
        }

        private readonly string teamName;

        private IList<Rat> ratList;
        private IList<Snake> snakeList;
        private IList<Ogre> ogreList;

        public IList<Creature> creatureList;

        public string TeamName { get { return teamName; } }
        public int Count { get { return creatureList.Count;} }
        public int RatCount { get { return ratList.Count; } }
        public int SnakeCount { get { return snakeList.Count; } }
        public int OgreCount { get { return ogreList.Count; } }
    }
}
