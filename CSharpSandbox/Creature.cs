using System;

namespace CSharpSandbox
{   
    class Creature 
    {
        protected Creature(string name) 
        {
            this.name = name;
        }

        public virtual void Attack(Creature target)
        {
            Console.WriteLine($"[~ATTACK~] {name} attacks {target.Name} with {strength:0.00} strength");
            target.TakeDamage(strength);
        }

        public virtual void TakeDamage(float damage)
        {
            health -= damage;
            Console.WriteLine($"[~DAMAGE~] {name} took {damage:0.00} damage, leaving it with {health:0.00} health remaining");
            if(health <= 0)
            {
                Console.WriteLine($"[DEFEATED] {name} was killed in battle!");
                dead = true;
            }
        }

        protected float baseStrength = 0.0f;
        protected float strength = 0.0f;
        protected float health = 0.0f;
        protected string name;
        private bool dead = false;

        public float Health
        {
            get { return health; }
        }
        public string Name { get { return name; } }
        public bool Dead { get { return dead; } }
    }
}
