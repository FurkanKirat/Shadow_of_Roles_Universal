using System;
using System.Collections.Generic;
using System.Linq;
using game.models.gamestate;
using game.models.player;
using game.models.player.properties;
using Game.Models.Roles.Enums;
using game.models.roles.interfaces;
using game.models.roles.interfaces.abilities;
using game.models.roles.properties;
using game.Services.GameServices;
using game.Utils;

namespace game.models.roles.Templates.FolkRoles
{
    public class Entrepreneur : RoleTemplate, IPriorityChangingRole, IRoleAIBehavior
        ,IInvestigativeAbility, IAttackAbility, IProtectiveAbility
    {
        private ChosenAbility _targetAbility;
        
        public Entrepreneur() : base(RoleId.Entrepreneur, RoleCategory.FolkUnique, 
            RolePriority.None, AbilityType.ActiveAll, WinningTeam.Folk)
        {
            RoleProperties
                .SetAttack(1)
                .SetMoney(5)
                .AddAttribute(RoleAttribute.HasAttackAbility)
                .AddAttribute(RoleAttribute.HasHealingAbility)
                .AddAttribute(RoleAttribute.CanKill1V1);
            _targetAbility = ChosenAbility.None;
        }
        
        public ChosenAbility TargetAbility
        {
            get => _targetAbility;
            set
            {
                _targetAbility = value;
                RolePriority = value.RolePriority;
            }
        }

        public override AbilityResult PerformAbility(Player roleOwner, Player choosenPlayer, BaseGameService gameService)
        {
            if(gameService.TimeService.TimePeriod.DayCount > 1){
                RoleProperties.Money.IncrementCurrent(2); //Passive income
            }

            return base.PerformAbility(roleOwner, choosenPlayer, gameService);
        }

        public override AbilityResult ExecuteAbility(Player roleOwner, Player choosenPlayer, BaseGameService gameService)
        {
            if(TargetAbility.Price > RoleProperties.Money.Current){
                return InsufficientMoney(roleOwner, gameService);
            }
            
            AbilityResult result;

            if (TargetAbility == ChosenAbility.Attack)
            {
                result = ((IAttackAbility)this).Attack(roleOwner, choosenPlayer, gameService, CauseOfDeath.Entrepreneur);
            }
            else if (TargetAbility == ChosenAbility.Heal)
            {
                result = ((IProtectiveAbility)this).Heal(roleOwner, choosenPlayer, gameService);
            }
            else if (TargetAbility == ChosenAbility.Info)
            {
                result = GatherInfo(roleOwner, choosenPlayer, gameService);
            }
            else
            {
                result = AbilityResult.NoAbilitySelected;
            }


            RoleProperties.Money.DecrementCurrent(TargetAbility.Price);

            TargetAbility = ChosenAbility.None;
            return result;
        }
        
        private AbilityResult GatherInfo(Player roleOwner, Player chosenPlayer, BaseGameService gameService){
            IInvestigativeAbility investigativeAbility = this;
            int randNum = RandomUtils.GetRandomNumber(1, 5);
            return randNum switch
            {
                1 => investigativeAbility.DarkSeerAbility(roleOwner, gameService),
                2 => investigativeAbility.DetectiveAbility(roleOwner, chosenPlayer, gameService),
                3 => investigativeAbility.ObserverAbility(roleOwner, chosenPlayer, gameService),
                4 => investigativeAbility.StalkerAbility(roleOwner, chosenPlayer, gameService),
                5 => investigativeAbility.DarkRevealerAbility(roleOwner, chosenPlayer, gameService),
                _ => throw new ArgumentOutOfRangeException(nameof(investigativeAbility), investigativeAbility, null)
            };
        }
        
        private AbilityResult InsufficientMoney(Player roleOwner, BaseGameService gameService)
        {
            var messageTemplate = new MessageTemplate
            {
                MessageKey = StringFormatter.Combine(RoleID, "money_insufficient"),
                PlaceHolders = new Dictionary<string, string>
                {
                    { "abilityName", StringFormatter.Combine(RoleID, TargetAbility.Name) }
                }
            };
            
            gameService.MessageService.SendPrivateMessage(messageTemplate, roleOwner);
            return AbilityResult.InsufficientMoney;
        }

        public void ChangePriority(RolePack rolePack)
        {
            RolePriority = TargetAbility.RolePriority;
        }

        public void ChooseRoleSpecificValues(List<Player> choosablePlayers)
        {
            bool randBool = RandomUtils.GetRandomBoolean();
            TargetAbility = randBool ? ChosenAbility.Heal : ChosenAbility.Attack;
        }

        public override ChanceProperty GetChanceProperty()
        {
            return ChancePropertyFactory.Unique(15);
        }
        
        
        public class ChosenAbility
        {
            public static readonly ChosenAbility Attack = new ("attack", RolePriority.None, 4);
            public static readonly ChosenAbility Heal = new ("heal", RolePriority.Heal, 3);
            public static readonly ChosenAbility Info = new ("info", RolePriority.None, 2);
            public static readonly ChosenAbility None = new ("none", RolePriority.None, 0);

            public string Name { get; }
            public RolePriority RolePriority { get; }
            public int Price { get; }

            private ChosenAbility(string name, RolePriority rolePriority, int price)
            {
                Name = name;
                RolePriority = rolePriority;
                Price = price;
            }

            public override string ToString() => Name;

            public static IEnumerable<ChosenAbility> Values => new[] { None, Attack, Heal, Info };

            public static int GetIndex(ChosenAbility chosenAbility)
            {
                return Values.ToList().IndexOf(chosenAbility);
            }
            public override bool Equals(object obj) => obj is ChosenAbility other && Name == other.Name;
            public override int GetHashCode() => Name.GetHashCode();

            public static bool operator ==(ChosenAbility a, ChosenAbility b) => a?.Equals(b) ?? b is null;
            public static bool operator !=(ChosenAbility a, ChosenAbility b) => !(a == b);

        }
        
    }
}