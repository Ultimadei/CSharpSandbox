namespace CSharpSandbox
{
    abstract class Creature
    {
        protected Creature(CreatureTeam allies, string name, float health, float baseStrength = 0.0f, float baseCriticalChance = 0.0f, float baseCriticalMultiplier = 1.0f)
        {
            this.name = name;
            this.health = health;
            this.baseStrength = baseStrength;
            this.baseCriticalChance = baseCriticalChance;
            this.baseCriticalMultiplier = baseCriticalMultiplier;

            this.allies = allies;

            strength = baseStrength;
            criticalChance = baseCriticalChance;
            criticalMultiplier = baseCriticalMultiplier;

            buffs = new List<Buff>();
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

            IList<Buff> updatedBuffs = new List<Buff>();

            foreach (var buff in buffs) if (!ApplyBuff(buff, durationPenalty)) updatedBuffs.Add(buff);

            buffs = updatedBuffs;
        }

        // Applies a single buff and returns true if that buff has expired
        // Derived classes should add handling for unique buff types
        protected virtual bool ApplyBuff(Buff buff, int durationPenalty = 1)
        {
            switch (buff.Type)
            {
                case Buff.BuffType.STRENGTH:
                    strength += buff.Apply(baseStrength, durationPenalty);
                    break;
                case Buff.BuffType.HEALTH:
                    // Health buffs can't apply when durationPenalty is set to 0
                    if (durationPenalty > 0 && buff.Value != 0.0f)
                    {
                        float healthBonus = buff.Apply(health, durationPenalty);

                        // Buff may be a heal or a damage effect (positive or negative)
                        if (buff.Value > 0)
                        {
                            WriteGenericTwoColors(TriggerLabelColor, "[TRIGGER ]", $"{name} healed",
                                HealthValueColor, $"{healthBonus}", $"health from {buff.Description}");
                        }
                        else
                        {
                            WriteDamage(name, healthBonus, health + healthBonus, $"from {buff.Description}");
                        }

                        health += healthBonus;

                        if (health <= 0) OnDeath($"died from {buff.Description}");
                    }
                    break;
                case Buff.BuffType.CRITICAL_CHANCE:
                    criticalChance += buff.Apply(baseCriticalChance, durationPenalty);
                    break;
                case Buff.BuffType.CRITICAL_DAMAGE:
                    criticalMultiplier += buff.Apply(baseCriticalMultiplier, durationPenalty);
                    break;
                default:
                    // Unhandled buff types are ignored, as are their durations. Derived classes may implement handlers for other buff types
                    break;
            }

            return buff.Duration <= 0;
        }

        // Public attack base (redirects to the appropriate variant)
        public virtual void Attack(CreatureTeam targets)
        {
            Creature target = targets.creatureList.Last();
            ApplyBuffs();

            // In case of poison death
            if (dead) return;

            // Attack the target
            Attack(target);
        }

        // Attack variant with Cleaving
        protected virtual void CleaveAttack(CreatureTeam targets, float cleaveChance, float cleaveMultiplier)
        {
            bool cleaving = false;

            do
            {
                Creature target = targets.creatureList.Last();

                // Disable critical damage on subsequent hits only
                if (cleaving) criticalChance = 0.0f;

                // Use main attack
                Attack(target);

                float overkill = -target.Health;
                float cleaveStrength = overkill * cleaveMultiplier;

                // Continue cleaving if the chance is satisfied and the damage is > 0
                // Cleaving can only continue if targets remain, however
                if (cleaveStrength > 0 && targets.Count > 0 && RandomNum.RandomBool(cleaveChance))
                {
                    cleaving = true;
                    strength = cleaveStrength;
                    WriteGenericTwoColors(PassiveLabelColor, "[PASSIVE ]", $"{name} cleaved right through {target.Name}, with",
                        DamageValueColor, $"{cleaveStrength:0.00}", "damage carrying over");
                } else
                {
                    cleaving = false;
                }
            } while (cleaving);
        }

        // Regular attack variant
        protected virtual void Attack(Creature target)
        {
            // Check for a critical hit
            if (RandomNum.RandomBool(criticalChance))
            {
                WriteGenericTwoColors(
                    CriticalLabelColor, "[CRITICAL]", $"{name} preps for a critical hit (", 
                    CriticalValueColor, $"{criticalMultiplier * 100.0f:0}%", "damage )!");

                strength *= criticalMultiplier;
            }

            WriteGenericTwoColors(AttackLabelColor, "[ ATTACK ]", $"{name} attacks {target.Name} with", StatValueColor, $"{strength:0.00}", "strength");

            target.OnDamageTaken(this, strength);
        }

        public virtual void OnTeamUpdate() { }

        public virtual void OnDamageTaken(Creature source, float damage)
        {
            // Apply any buffs that might soften the blow (the duration is not affected)
            ApplyBuffs(0);

            health -= damage;

            WriteDamage(name, damage, health);

            if (health <= 0)
            {
                OnDeath("died in battle");  
            }
        }

        public virtual void OnDeath(string reason)
        {
            WriteGenericOneColor(DefeatedLabelColor, "[DEFEATED]", $"{name} {reason}");
            dead = true;

            allies.RegisterDeath(this);
        }

        protected static void WriteGenericOneColor(ConsoleColor openingColor, string opening, string closing)
        {
            Utils.Write(openingColor, opening, DefaultColor, " ", closing, "\n");
        }

        // General template for Utils.Write
        protected static void WriteGenericTwoColors(
            ConsoleColor openingColor, string opening, string neutral, 
            ConsoleColor color, string colored, string closing)
        {
            Utils.Write(
                openingColor, opening, DefaultColor, " ", neutral, " ", 
                color, colored, DefaultColor, " ", closing, "\n");
        }

        protected static void WriteGenericThreeColors(
            ConsoleColor openingColor, string opening, string firstNeutral, 
            ConsoleColor firstColor, string firstColored, string secondNeutral,
            ConsoleColor secondColor, string secondColored, string closing) 
        {
            Utils.Write(
                openingColor, opening, DefaultColor, " ", firstNeutral, " ",
                firstColor, firstColored, DefaultColor, " ", secondNeutral, " ",
                secondColor, secondColored, DefaultColor, " ", closing, "\n");
        }

        protected static void WriteDamage(string name, float damage, float healthRemaining, string reason = "")
        {
            WriteGenericThreeColors(
                DamageLabelColor, "[ DAMAGE ]", $"{name} took", DamageValueColor, $"{damage:0.00}", $"damage{(reason.Length > 0 ? " " : ",")}{reason} leaving it with", 
                healthRemaining < 0.0f ? DamageValueColor : HealthValueColor, $"{healthRemaining:0.00}", "health remaining");
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

        protected CreatureTeam allies;
        protected IList<Buff> buffs;

        protected const ConsoleColor DamageLabelColor = ConsoleColor.Red;
        protected const ConsoleColor AttackLabelColor = ConsoleColor.Red;
        protected const ConsoleColor TriggerLabelColor = ConsoleColor.DarkGreen;
        protected const ConsoleColor DefeatedLabelColor = ConsoleColor.DarkMagenta;
        protected const ConsoleColor CriticalLabelColor = ConsoleColor.DarkYellow;
        protected const ConsoleColor PassiveLabelColor = ConsoleColor.DarkGray;

        protected const ConsoleColor DamageValueColor = ConsoleColor.DarkRed;
        protected const ConsoleColor CriticalValueColor = ConsoleColor.DarkYellow;
        protected const ConsoleColor HealthValueColor = ConsoleColor.Green;
        protected const ConsoleColor StatValueColor = ConsoleColor.Cyan;

        protected const ConsoleColor DefaultColor = Utils.DefaultColor;

        public float Health { get { return health; } }
        public string Name { get { return name; } }
        public bool Dead { get { return dead; } }
    }
}
