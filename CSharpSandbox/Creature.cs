namespace CSharpSandbox
{
    abstract class Creature 
    {
        protected Creature(string name, float health, float baseStrength = 0.0f, float baseCriticalChance = 0.0f, float baseCriticalMultiplier = 1.0f) 
        {
            this.name = name;
            this.health = health;
            this.baseStrength = baseStrength;
            this.baseCriticalChance = baseCriticalChance;
            this.baseCriticalMultiplier = baseCriticalMultiplier;

            strength = baseStrength;
            criticalChance = baseCriticalChance;
            criticalMultiplier = baseCriticalMultiplier;

            buffs = new();
        }

        public void AddBuff(Buff buff)
        {
            buffs.Add(buff);
        }

        protected virtual void ApplyBuffs(int durationPenalty = 1)
        {
            strength = baseStrength;
            criticalChance = baseCriticalChance;
            criticalMultiplier = baseCriticalMultiplier;
            
            // Health buffs are calculated based on this initial health, but are added to health
            float initialHealth = health;

            List<Buff> updatedBuffs = new();

            foreach (var buff in buffs)
            {
                switch (buff.Type)
                {
                    case Buff.BuffType.STRENGTH:
                        strength += buff.Apply(baseStrength, durationPenalty);
                        break;
                    case Buff.BuffType.HEALTH:
                        health += buff.Apply(initialHealth, 0);
                        // Health buffs are instantaneous, meaning they disappear after one use regardless of duration. So, it is not added to the new list
                        continue;
                    case Buff.BuffType.CRITICAL_CHANCE:
                        criticalChance += buff.Apply(baseCriticalChance, durationPenalty);
                        break;
                    case Buff.BuffType.CRITICAL_DAMAGE:
                        criticalMultiplier += buff.Apply(baseCriticalMultiplier, durationPenalty);
                        break;
                }

                if(buff.Duration > 0) updatedBuffs.Add(buff);
            }

            buffs = updatedBuffs;
        }

        // Public attack base (redirects to the appropriate variant)
        public virtual void Attack(List<Creature> targets)
        {
            Creature target = targets.Last();
            ApplyBuffs();

            // If the attack kills the target, remove it from the list and call any triggers
            if (Attack(target)) RegisterDeath(target, targets);
        }

        // Attack variant with Cleaving
        protected virtual void CleaveAttack(List<Creature> targets, float cleaveChance, float cleaveMultiplier)
        {
            bool cleaving = false;

            do
            {
                Creature target = targets.Last();

                // Disable critical damage on subsequent hits only
                if (cleaving) criticalChance = 0.0f;

                // Use main attack
                if (Attack(target)) RegisterDeath(target, targets);

                float overkill = -target.Health;
                float cleaveStrength = overkill * cleaveMultiplier;

                // Continue cleaving if the chance is satisfied and the damage is > 0
                // Cleaving can only continue if targets remain, however
                if (cleaveStrength > 0 && targets.Count > 0 && RandomNum.RandomBool(cleaveChance))
                {
                    cleaving = true;
                    strength = cleaveStrength;
                    Utils.Write(ConsoleColor.DarkGray, "[PASSIVE ]", ConsoleColor.Gray, 
                        $" {name} cleaved right through {target.Name}, with ", 
                        ConsoleColor.Cyan, $"{cleaveStrength:0.00}", ConsoleColor.Gray, 
                        " damage carrying over\n"
                    );
                } else
                {
                    cleaving = false;
                }
            } while (cleaving);
        }

        // Regular attack variant. Returns true if the target was killed
        protected virtual bool Attack(Creature target)
        {
            // Check for a critical hit
            if (RandomNum.RandomBool(criticalChance))
            {
                Utils.Write(ConsoleColor.DarkYellow, "[CRITICAL]", ConsoleColor.Gray,
                    $" {name} preps for a critical hit (",
                    ConsoleColor.DarkYellow, $"{criticalMultiplier * 100.0f:0}%", ConsoleColor.Gray,
                    " damage)!\n"
                );

                strength *= criticalMultiplier;
            }

            Utils.Write(ConsoleColor.Red, "[ ATTACK ]", ConsoleColor.Gray,
                $" {name} attacks {target.Name} with ",
                ConsoleColor.Cyan, $"{strength:0.00}", ConsoleColor.Gray,
                " strength\n"
            );

            target.OnDamageTaken(strength);

            return target.Dead;
        }

        // Remove the dead target from the list and notify everyone else 
        protected void RegisterDeath(Creature target, List<Creature> targets)
        {
            targets.Remove(target);
            foreach (Creature creature in targets)
            {
                creature.OnAllyDeath(target);
            }
        }

        public virtual void OnDamageTaken(float damage)
        {
            // Apply any buffs that might soften the blow (the duration is not affected)
            ApplyBuffs(0);

            health -= damage;


            
            if (health <= 0)
            {
                Utils.Write(ConsoleColor.Red, "[ DAMAGE ]", ConsoleColor.Gray,
                    $" {name} took ",
                    ConsoleColor.Red, $"{damage:0.00}", ConsoleColor.Gray,
                    " damage, leaving it with ",
                    ConsoleColor.DarkRed, $"{health:0.00}", ConsoleColor.Gray,
                    " health remaining\n"
                );

                Utils.Write(ConsoleColor.DarkMagenta, "[DEFEATED]", ConsoleColor.Gray,
                    $" {name} was killed in battle\n"
                );
                dead = true;
            } else
            {
                Utils.Write(ConsoleColor.Red, "[ DAMAGE ]", ConsoleColor.Gray,
                    $" {name} took ",
                    ConsoleColor.Red, $"{damage:0.00}", ConsoleColor.Gray,
                    " damage, leaving it with ",
                    ConsoleColor.Green, $"{health:0.00}", ConsoleColor.Gray,
                    " health remaining\n"
                );
            }
        }

        public virtual void OnAllyDeath(Creature deadAlly) { }

        protected readonly float baseStrength;
        protected readonly float baseCriticalChance;
        protected readonly float baseCriticalMultiplier;

        protected float strength = 0.0f;
        protected float criticalChance = 0.0f;
        protected float criticalMultiplier = 1.0f;

        protected float health = 0.0f;

        protected string name;
        private bool dead = false;

        protected List<Buff> buffs;
        
        public float Health { get { return health; } }
        public string Name { get { return name; } }
        public bool Dead { get { return dead; } }
    }
}
