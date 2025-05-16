using System.Collections.Generic;
using game.models.player;
using Game.Models.Roles.Enums;
using game.models.roles.interfaces;
using game.models.roles.interfaces.abilities;
using game.models.roles.properties;
using game.Services;
using game.Services.GameServices;
using game.Utils;

namespace game.models.roles.Templates.NeutralRoles
{
    public class LoreKeeper : RoleTemplate, IRoleAIBehavior
    {
        public HashSet<int> AlreadyChosenPlayers { get; } = new ();
        public RoleId GuessedRole {get; set;} = RoleId.None;
        public int TrueGuessCount { get; private set; } = 0;
        public LoreKeeper() : base(RoleId.LoreKeeper, RoleCategory.NeutralGood, 
            RolePriority.LoreKeeper, AbilityType.ActiveOthers, WinningTeam.LoreKeeper)
        {
            RoleProperties
                .AddAttribute(RoleAttribute.CanWinWithAnyTeam)
                .AddAttribute(RoleAttribute.HasOtherWinCondition)
                .AddAttribute(RoleAttribute.WinsAlone)
                .AddAttribute(RoleAttribute.RoleBlockImmune);
        }

        public override AbilityResult PerformAbility(Player roleOwner, Player choosenPlayer, BaseGameService gameService)
        {
            return ((IPerformAbility) this).PerformAbilityLoreKeeper(roleOwner, choosenPlayer, gameService, GuessedRole);
        }
        public override AbilityResult ExecuteAbility(Player roleOwner, Player choosenPlayer, BaseGameService gameService)
        {
            AlreadyChosenPlayers.Add(choosenPlayer.Number);

            if(choosenPlayer.Role.Template.RoleID == GuessedRole){
                TrueGuessCount++;

                var messageTemplate = new MessageTemplate
                {
                    MessageKey = StringFormatter.Combine(RoleID, "ability_message"),
                    PlaceHolders = new Dictionary<string, string>
                    {
                        { "playerName", choosenPlayer.GetNameAndNumber() },
                        { "roleId", choosenPlayer.Role.Template.RoleID.FormatEnum() }
                    }
                };
                
                gameService.MessageService.SendPublicMessage(messageTemplate);
                choosenPlayer.Role.IsRevealed = true;
            }
            GuessedRole = RoleId.None;
            return AbilityResult.Success;
        }
        
        public override ChanceProperty GetChanceProperty()
        {
            return ChancePropertyFactory.Unique(30);
        }

        public void ChooseRoleSpecificValues(List<Player> choosablePlayers)
        {
            GuessedRole = RoleCatalog.GetRandomRole().RoleID;
            choosablePlayers.RemoveAll(player => AlreadyChosenPlayers.Contains(player.Number));
        }
    }
}