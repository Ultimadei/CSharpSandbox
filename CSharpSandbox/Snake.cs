namespace CSharpSandbox
{
    class Snake : Creature
    {
        public Snake(CreatureTeam allies, string name) : base(allies, name, 8.0f, 4.5f, 0.1f, 4.0f) 
        {
            basePoisonChance = 0.6f;
            basePoisonDamage = strength;
            basePoisonDuration = 3;

            poisonChance = basePoisonChance;
            poisonDamage = basePoisonDamage;
            poisonDuration = basePoisonDuration;
        }

        /*
        public override void Attack(CreatureTeam targets)
        {
            int hitCount = RandomNum.RandomInteger(1 + (int)Math.Floor(targets.Count * 0.1f), 1 + (int)Math.Floor(targets.Count * 0.4f));

            WriteGenericTwoColors(AttackLabelColor, "[ ATTACK ]", $"{name} spews out deadly venom")

            base.Attack(targets);
        }
        */

        protected override void OnDamageTaken(Creature source, float damage)
        {
            ApplyBuffs(0);

            if(damage > 0 && source != null && RandomNum.RandomBool(poisonChance))
            {
                WriteGenericThreeColors(TriggerLabelColor, "[TRIGGER ]", $"{name} managed to poison {source.Name} when it was attacked. {source.Name} will take",
                    DamageValueColor, $"{poisonDamage}", "damage per turn for", 
                    StatValueColor, $"{poisonDuration}", "turns (damage inflicted when it attacks)");

                source.AddBuff(new(-poisonDamage, poisonDuration, "Snake Poison", Buff.BuffType.HEALTH, Buff.BuffOperator.FLAT));
            }
            base.OnDamageTaken(source, damage);
        }

        protected readonly float basePoisonChance;
        protected readonly float basePoisonDamage;
        protected readonly int basePoisonDuration;

        protected float poisonChance;
        protected float poisonDamage;
        protected int poisonDuration;
    }
}
