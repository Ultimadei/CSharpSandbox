namespace CSharpSandbox
{
    class Leech : Creature
    {
        public Leech(CreatureTeam allies, string name) : base(allies, name, 5.0f, 2.5f, 0.5f, 3.0f) 
        {
            baseLifestealChance = 1.0f;
            baseLifestealAmount = 1.0f;

            lifestealChance = baseLifestealChance;
            lifestealAmount = baseLifestealAmount;
        }

        protected override void OnDamageDealt(Creature target, float damage)
        {
            if (RandomNum.RandomBool(lifestealChance))
            {
                float healthBonus = lifestealAmount * damage;

                WriteGenericThreeColors(TriggerLabelColor, "[TRIGGER ]", $"{name} managed to steal life equal to",
                    StatValueColor, $"{lifestealAmount * 100.0f:0}%", "of its damage (", 
                    HealthValueColor, $"{healthBonus:0.00}", ")"
                );

                health += healthBonus;
            }

            base.OnDamageDealt(target, damage);
        }

        protected float baseLifestealChance;
        protected float baseLifestealAmount;

        protected float lifestealChance;
        protected float lifestealAmount;
    }
}
