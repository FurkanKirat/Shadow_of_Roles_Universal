using System.Collections.Generic;

namespace game.models.roles.properties
{
    public class RoleProperties
    {
        // Base stats
        public Stat Attack { get; private set; } = new (0);
        public Stat Defence { get; private set; } = new (0);
        public Stat Cooldown { get; private set; } = new (0);
        public Stat VoteCount { get; private set; } = new (1);
        public Stat Money { get; private set; } = new (0);
        public Stat AbilityUsesLeft { get; private set; } = new (-1);
        
        public bool CanUseAbility => AbilityUsesLeft.Current !=0 && Cooldown.Current == 0;

        private HashSet<RoleAttribute> RoleAttributes { get; } = new ();

        public RoleProperties() {}
        
        public RoleProperties AddAttribute(RoleAttribute attribute)
        {
            RoleAttributes.Add(attribute);
            return this;
        }

        public RoleProperties RemoveAttribute(RoleAttribute attribute)
        {
            RoleAttributes.Remove(attribute);
            return this;
        }

        public bool HasAttribute(RoleAttribute attribute)
        {
            return RoleAttributes.Contains(attribute);
        }
        
        public RoleProperties SetAbilityUsesLeft(int value)
        {
            AbilityUsesLeft = new Stat(value);
            return this;
        }

        public RoleProperties SetAttack(int attack)
        {
            Attack = new Stat(attack);
            return this;
        }

        public RoleProperties SetDefence(int defence)
        {
            Defence = new Stat(defence);
            return this;
        }

        public RoleProperties SetVoteCount(int voteCount)
        {
            VoteCount = new Stat(voteCount);
            return this;
        }

        public RoleProperties SetCooldown(int cooldown)
        {
            Cooldown = new Stat(cooldown);
            return this;
        }

        public RoleProperties SetMoney(int money)
        {
            Money = new Stat(money);
            return this;
        }
        
        
    }
}
