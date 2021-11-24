namespace CSharpSandbox
{
    class Ogre : Creature
    {
        public Ogre(CreatureTeam allies, string name) : base(allies, name, 25.0f, 10.0f, 0.2f, 2.5f) 
        {
            baseCleaveChance = 0.8f;
            baseCleaveMultiplier = 0.75f;

            cleaveChance = baseCleaveChance;
            cleaveMultiplier = baseCleaveMultiplier;
        }

        public override void OnAllyDeath(Creature deadAlly)
        {
            TriggerBattleRage(deadAlly.Name);
        }

        protected override void ApplyBuffs(int durationPenalty = 1)
        {
            base.ApplyBuffs(durationPenalty);

            IList<Buff> updatedBuffs = new List<Buff>();

            foreach (Buff buff in buffs)
            {
                switch (buff.Type)
                {
                    case Buff.BuffType.FRENZY_TIMER:
                        // As long as the "buff" is active, frenzy cannot be reapplied
                        if (buff.Apply(0.0f, durationPenalty) != 1.0f)
                        {
                            isFrenzied = false;
                            continue;
                        }
                        break;
                    default:
                        break;
                }
                updatedBuffs.Add(buff);
            }
        }

        private void TriggerBattleRage(string deadOgreName)
        {
            // 40% chance to go into a frenzy if it's not already in it
            if (!isFrenzied && RandomNum.RandomBool(0.4f))
            {
                float strengthBonus = 0.5f;
                float criticalChanceBonus = 0.75f;
                float criticalDamageBonus = 1.5f;
                float healthBonus = 1.25f;

                Utils.Write(ConsoleColor.DarkGreen, "[TRIGGER ]", Utils.DefaultColor, 
                    $" {name} is driven into a frenzy over the death of {deadOgreName}. Its stats are greatly increased for a few turns!!\n<",
                    ConsoleColor.Cyan, $"+{strengthBonus * 100.0f:0}%", Utils.DefaultColor, " strength ; ",
                    ConsoleColor.Cyan, $"+{criticalChanceBonus * 100.0f:0}%", Utils.DefaultColor, " critical chance ; ",
                    ConsoleColor.Cyan, $"+{criticalDamageBonus * 100.0f:0}%", Utils.DefaultColor, " critical damage ; ",
                    ConsoleColor.Cyan, $"+{healthBonus * 100.0f:0}%", Utils.DefaultColor, " health>\n"
                );

                isFrenzied = true;

                AddBuff(new Buff(strengthBonus, 2, "Battle Rage", Buff.BuffType.STRENGTH, Buff.BuffOperator.PERCENTAGE));
                AddBuff(new Buff(criticalChanceBonus, 3, "Battle Rage", Buff.BuffType.CRITICAL_CHANCE, Buff.BuffOperator.PERCENTAGE));
                AddBuff(new Buff(criticalChanceBonus, 3, "Battle Rage", Buff.BuffType.CRITICAL_DAMAGE, Buff.BuffOperator.PERCENTAGE));
                AddBuff(new Buff(1.0f, 3, "Battle Rage", Buff.BuffType.FRENZY_TIMER, Buff.BuffOperator.FLAT));

                // Health bonus is instantaneous
                health += health * healthBonus;
            }
        }

        public override void Attack(CreatureTeam targets)
        {
            base.CleaveAttack(targets, cleaveChance, cleaveMultiplier);
        }

        protected readonly float baseCleaveChance;
        protected readonly float baseCleaveMultiplier;

        protected float cleaveChance;
        protected float cleaveMultiplier;

        private bool isFrenzied = false;
    }
}
