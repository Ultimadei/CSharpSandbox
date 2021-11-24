namespace CSharpSandbox
{
    class Rat : Creature
    {
        public Rat(CreatureTeam allies, string name, int groupSize) : base(allies, name, 3.0f, 1.0f) 
        { 
            baseGroupSize = groupSize;
            this.groupSize = groupSize;
        }

        public override void Attack(CreatureTeam targets)
        {
            // Rat gets an attack buff if there are enough rats in the group
            // The minimum rats needed for the buff = 60% of the original group size
            // The maximum requirement for the buff = 150% of the original group size 
            int groupSizeRequirement = RandomNum.RandomInteger((int)(baseGroupSize * 0.6f), (int)(baseGroupSize * 1.5f));
            if (groupSize >= groupSizeRequirement)
            {
                // Activate attack buff. Buff is based on how big the group is and how lucky this rat was
                float luckFactor = (float)(baseGroupSize / 2) / groupSizeRequirement;
                float strengthBonus = (float)Math.Log2(1.0 + Math.Pow(groupSize, luckFactor)) * 0.25f;

                if (strengthBonus > 0.0f)
                {
                    WriteGenericTwoColors(PassiveLabelColor, "[PASSIVE ]", $"{name} is empowered by the rat army. Its strength is increased permanently! <",
                        StatValueColor, $"+{strengthBonus:0.00}", "strength >");
                    AddBuff(new Buff(strengthBonus, Buff.DURATION_MAX, "Strength in Numbers", Buff.BuffType.STRENGTH, Buff.BuffOperator.FLAT));
                }
            }

            base.Attack(targets);
        }

        public override void OnTeamUpdate()
        {
            base.OnTeamUpdate();
            groupSize = allies.Count;
        }

        public override void OnAllyDeath(Creature deadAlly)
        {
            groupSize = allies.Count;
        }

        protected readonly int baseGroupSize = 0;
        protected int groupSize = 0;
    }
}
