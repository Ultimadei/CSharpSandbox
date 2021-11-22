namespace CSharpSandbox
{
    class Rat : Creature
    {
        public Rat(string name) : base(name) 
        {
            baseStrength = 1.0f;
            strength = baseStrength;
            health = 3.0f;
        }

        public override void Attack(Creature target)
        {
            // Rat gets an attack buff if there are enough rats in the group
            // The minimum rats needed for the buff = 4 + twice this rat's health
            // The maximum requirement for the buff = 4 + triple this rat's strength & health
            int groupSizeRequirement = 7 + RandomNum.RandomInteger((int)health * 2, (int)(3 * (health + strength)));
            if (groupSize > groupSizeRequirement)
            {
                // Activate attack buff. Buff is based on how many more rats were in the group than the random requirement
                float strengthBuff = (float)Math.Log2(groupSize - groupSizeRequirement) * 0.25f;
                if(strengthBuff > 0.0f) Console.WriteLine($"[PASSIVE~] {name} is empowered by the rat army: +{strengthBuff:0.00} strength (until it dies)!");
                strength += strengthBuff;
            }
            base.Attack(target);
        }

        protected int groupSize = 0;
        public int GroupSize
        {
            get { return groupSize; }
            set { groupSize = value; }
        }
    }
}
