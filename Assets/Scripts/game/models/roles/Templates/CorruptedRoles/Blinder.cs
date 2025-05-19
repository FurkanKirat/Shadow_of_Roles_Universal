using game.models.player;
using Game.Models.Roles.Enums;
using game.models.roles.properties;
using game.Services.GameServices;
using game.Utils;

namespace game.models.roles.Templates.CorruptedRoles
{
    public class Blinder : RoleTemplate
    {
        public Blinder() : base(RoleId.Blinder, RoleCategory.CorrupterSupport, 
            RolePriority.Blinder, AbilityType.OtherThanTeamMembers, WinningTeam.Corrupter)
        {
            RoleProperties
                .AddAttribute(RoleAttribute.KnowsTeamMembers)
                .AddAttribute(RoleAttribute.HasBlindAbility);

            ChanceProperty = ChancePropertyFactory.Unlimited(25);
        }

        public override AbilityResult ExecuteAbility(Player roleOwner, Player choosenPlayer, BaseGameService gameService)
        {
            var players = gameService.CopyAlivePlayers();
            players.Remove(choosenPlayer.Number);
            choosenPlayer.Role.ChosenPlayer = players.GetRandomElement();

            var selfMessage = new MessageTemplate
            {
                MessageKey = StringFormatter.Combine(RoleID,"ability_message")
            };
            
            var targetMessage = new MessageTemplate
            {
                MessageKey = StringFormatter.Combine(RoleID, "got_blinded_message")
            };
            
            gameService.MessageService.SendPrivateMessage(selfMessage, roleOwner);
            gameService.MessageService.SendPrivateMessage(targetMessage, choosenPlayer);

            return AbilityResult.Success;
        }
        
    }
}