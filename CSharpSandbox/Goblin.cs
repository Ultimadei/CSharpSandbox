namespace CSharpSandbox
{
    class Goblin : Creature
    {
        public Goblin(CreatureTeam allies, string name) : base(allies, name, 20.0f, 2.5f, 0.1f, 2.5f)
        {
            baseCleaveChance = 1.0f;
            baseCleaveMultiplier = 0.4f;
            baseAngerStrengthBonus = 1.0f;
            baseAngerCriticalMultiplierBonus = 0.5f;
            
            cleaveChance = baseCleaveChance;
            cleaveMultiplier = baseCleaveMultiplier;
            angerStrengthBonus = baseAngerStrengthBonus;
            angerCriticalMultiplierBonus = baseAngerCriticalMultiplierBonus;
        }

        public override void Attack(CreatureTeam targets)
        {
            base.CleaveAttack(targets, cleaveChance, cleaveMultiplier);
        }

        protected override void OnDamageTaken(Creature source, float damage)
        {
            Utils.Write(PassiveLabelColor, "[PASSIVE ]", Utils.DefaultColor,
                $" {name} is angered from being hit. Its stats are increased permanently! <",
                StatValueColor, $"+{baseAngerStrengthBonus:0.00}", Utils.DefaultColor, " strength ; ",
                StatValueColor, $"+{angerCriticalMultiplierBonus * 100.0f:0}%", Utils.DefaultColor, " critical chance>\n"
            );

            AddBuff(new Buff(angerStrengthBonus, Buff.DURATION_MAX, "Goblin Anger", Buff.BuffType.STRENGTH, Buff.BuffOperator.FLAT));
            AddBuff(new Buff(angerCriticalMultiplierBonus, Buff.DURATION_MAX, "Goblin Anger", Buff.BuffType.CRITICAL_DAMAGE, Buff.BuffOperator.PERCENTAGE));

            base.OnDamageTaken(source, damage);
        }

        protected readonly float baseCleaveChance;
        protected readonly float baseCleaveMultiplier;
        protected readonly float baseAngerStrengthBonus;
        protected readonly float baseAngerCriticalMultiplierBonus;

        protected float cleaveChance;
        protected float cleaveMultiplier;
        protected float angerStrengthBonus;
        protected readonly float angerCriticalMultiplierBonus;
    }
}