namespace CSharpSandbox
{
    class Ogre : Creature
    {
        public Ogre(string name) : base(name, 25.0f, 10.0f, 0.2f, 2.5f) 
        {
            cleaveChance = 0.8f;
            cleaveMultiplier = 0.75f;
        }

        public override void OnAllyDeath(Creature deadAlly)
        {
            TriggerBattleRage(deadAlly.Name);
        }

        private void TriggerBattleRage(string deadOgreName)
        {
            // 30% chance to go into a frenzy
            if (RandomNum.RandomBool(0.3f))
            {
                float strengthBonus = 0.5f;
                float criticalChanceBonus = 0.75f;
                float criticalDamageBonus = 1.5f;
                float healthBonus = 1.25f;

                Utils.Write(ConsoleColor.DarkGreen, "[TRIGGER ]", ConsoleColor.Gray, 
                    $" {name} is driven into a frenzy over the death of {deadOgreName}. Its stats are greatly increased for a few turns!!\n<",
                    ConsoleColor.Cyan, $"+{strengthBonus * 100.0f:0}%", ConsoleColor.Gray, " strength ; ",
                    ConsoleColor.Cyan, $"+{criticalChanceBonus * 100.0f:0}%", ConsoleColor.Gray, " critical chance ; ",
                    ConsoleColor.Cyan, $"+{criticalDamageBonus * 100.0f:0}%", ConsoleColor.Gray, " critical damage ; ",
                    ConsoleColor.Cyan, $"+{healthBonus * 100.0f:0}%", ConsoleColor.Gray, " health>\n"
                );

                AddBuff(new Buff(strengthBonus, 2, Buff.BuffType.STRENGTH, Buff.BuffOperator.PERCENTAGE));
                AddBuff(new Buff(criticalChanceBonus, 3, Buff.BuffType.CRITICAL_CHANCE, Buff.BuffOperator.PERCENTAGE));
                AddBuff(new Buff(criticalChanceBonus, 3, Buff.BuffType.CRITICAL_DAMAGE, Buff.BuffOperator.PERCENTAGE));
                AddBuff(new Buff(healthBonus, 1, Buff.BuffType.HEALTH, Buff.BuffOperator.PERCENTAGE));

                // Applies the buffs without reducing the duration (in order to obtain the new health)
                ApplyBuffs(0);
            }            
        }

        public override void Attack(List<Creature> targets)
        {
            base.CleaveAttack(targets, cleaveChance, cleaveMultiplier);
        }

        private readonly float cleaveChance;
        private readonly float cleaveMultiplier;
    }
}
