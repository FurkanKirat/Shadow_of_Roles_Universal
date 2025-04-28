
using System.Collections.Generic;

namespace game.models.roles.properties
{
    public class RoleProperties
    {
        // Base stats
        public double Attack { get; set; } = 0;
        public double Defence { get; set; } = 0;
        public int VoteCount { get; set; } = 1;
        public int Money { get; set; } = -1;
        public int AbilityUsesLeft { get; set; } = -1;
        public int Cooldown { get; set; } = 0;
        public bool CanUseAbility => AbilityUsesLeft !=0 && Cooldown == 0;

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

        public void IncrementMoney(int money)
        {
            Money += money;
        }

        public void DecrementMoney(int money)
        {
            Money -= money;
        }

        public void DecrementAbilityUsesLeft()
        {
            AbilityUsesLeft--;
        }

        public RoleProperties SetAttack(double attack)
        {
            Attack = attack;
            return this;
        }

        public RoleProperties SetDefence(double defence)
        {
            Defence = defence;
            return this;
        }

        public RoleProperties SetVoteCount(int voteCount)
        {
            VoteCount = voteCount;
            return this;
        }

        public RoleProperties SetCooldown(int cooldown)
        {
            Cooldown = cooldown;
            return this;
        }

        public RoleProperties SetMoney(int money)
        {
            Money = money;
            return this;
        }

        public RoleProperties SetAbilityUsesLeft(int abilityUsesLeft)
        {
            AbilityUsesLeft = abilityUsesLeft;
            return this;
        }
        
    }
}
