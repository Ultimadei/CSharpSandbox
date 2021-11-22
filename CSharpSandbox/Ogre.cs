namespace CSharpSandbox
{
    class Ogre : Creature
    {
        public Ogre(string name) : base(name) 
        {
            baseStrength = 8.0f;
            strength = baseStrength;
            health = 20.0f;
            criticalChance = 0.5f;
            criticalMultiplier = 2.5f;
            cleavageChance = 0.75f;
            cleavageMultiplier = 0.4f;
        }

        public void SpecialBattleRage(string deadOgreName)
        {
            float strengthMultiplier = 1.5f;
            float criticalChanceMultiplier = 1.5f;
            float healthMultiplier = 2.0f;
            
            Console.WriteLine(
                $"[SPECIAL~] {name} is driven into a frenzy over the death of {deadOgreName}!! Its stats are greatly increased " +
                $"({strengthMultiplier * 100.0f:0}% strength ; {criticalChanceMultiplier * 100.0f:0}% critical chance ; {healthMultiplier * 100.0f:0}% health)"
            );

            baseStrength *= strengthMultiplier;
            strength = baseStrength;
            criticalChance *= criticalChanceMultiplier;
            health *= healthMultiplier;
        }

        public void CleaveAttack(Creature target)
        {
            // Disable criticals on cleave attacks
            float initialCriticalChance = criticalChance;
            criticalChance = 0.0f;
            
            Attack(target);

            // Return criticalChance to its base value
            criticalChance = initialCriticalChance;
        }

        public override void Attack(Creature target)
        {
            /// Handle criticals
            
            bool isCritical = false;
            // Record strength so that criticals aren't permanent
            float noncriticalStrength = strength;
            if (RandomNum.RandomBool(criticalChance))
            {
                Console.WriteLine($"[CRITICAL] {name} preps for a critical hit ({criticalMultiplier * 100.0f:0}% damage)!");
                strength *= criticalMultiplier;
                isCritical = true;
            }

            /// Assign damage
            
            base.Attack(target);
            // Return strength to its noncritical value
            strength = noncriticalStrength;

            /// Handle cleavage

            float overkill = -target.Health;

            // Overkill is how much excess damage the target was dealt when it died
            // Critical hits will always cause cleavage, otherwise it has a % chance with cleavageChance 
            if (overkill * cleavageMultiplier > 0 && (isCritical || RandomNum.RandomBool(cleavageChance)))
            {
                strength = overkill * cleavageMultiplier;
                isCleaving = true;
                Console.WriteLine($"[PASSIVE~] {name} cleaved right through {target.Name}");
            } else
            {
                // Cleavage has ended, so restore currentStrength to the base value
                strength = baseStrength;
                isCleaving = false;
            }
        }

        protected float criticalChance;
        protected float criticalMultiplier;
        protected float cleavageChance;
        protected float cleavageMultiplier;
        protected bool isCleaving = false;

        public bool IsCleaving
        {
            get { return isCleaving; }
        }
    }
}
