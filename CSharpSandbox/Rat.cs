namespace CSharpSandbox
{
    class Rat : Creature
    {
        public Rat(string name, int groupSize) : base(name, 3.0f, 1.0f) 
        { 
            baseGroupSize = groupSize;
            this.groupSize = groupSize;
        }

        public override void Attack(List<Creature> targets)
        {
            // Apply buffs without reducing duration, to ensure the strength is up to date
            ApplyBuffs(0);

            // Rat gets an attack buff if there are enough rats in the group
            // The minimum rats needed for the buff = 50% of the original group size
            // The maximum requirement for the buff = 125% of the original group size 
            int groupSizeRequirement = (baseGroupSize / 2) + RandomNum.RandomInteger(0, (int)(baseGroupSize * 1.25f));
            if (groupSize >= groupSizeRequirement)
            {
                // Activate attack buff. Buff is based on how big the group is and how lucky this rat was
                float luckFactor = (float)(baseGroupSize / 2) / groupSizeRequirement;
                float strengthBonus = (float)Math.Log2(1.0 + Math.Pow(groupSize, luckFactor)) * 0.25f;

                if (strengthBonus > 0.0f)
                {
                    Utils.Write(ConsoleColor.DarkGray, "[PASSIVE ]", ConsoleColor.Gray,
                        $" {name} is empowered by the rat army. Its strength is increased permanently! <",
                        ConsoleColor.Cyan, $"+{strengthBonus:0.00}", ConsoleColor.Gray,
                        " strength>\n"
                    );
                    AddBuff(new Buff(strengthBonus, Buff.DURATION_MAX, Buff.BuffType.STRENGTH, Buff.BuffOperator.FLAT));
                }
            }
            base.Attack(targets);
        }

        public override void OnAllyDeath(Creature deadAlly)
        {
            if (groupSize > 1) groupSize--;
        }

        protected readonly int baseGroupSize = 0;
        protected int groupSize = 0;
    }
}
