using game.models.player;
using Game.Models.Roles.Enums;
using game.models.roles.properties;
using game.Services.GameServices;
using game.Utils;
using Managers;

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
        }

        public override AbilityResult ExecuteAbility(Player roleOwner, Player choosenPlayer, BaseGameService gameService)
        {
            string message = TextManager.TranslateEnum(RoleID,"ability_message");
            SendAbilityMessage(message,roleOwner, gameService.MessageService);
            var players = gameService.CopyAlivePlayers();

            players.Remove(choosenPlayer.Number);

            choosenPlayer.Role.ChosenPlayer = players.GetRandomElement();

            SendAbilityMessage(TextManager.TranslateEnum(RoleID,"got_blinded_message"), choosenPlayer, gameService.MessageService);

            return AbilityResult.Success;
        }

        public override ChanceProperty GetChanceProperty()
        {
            return new ChanceProperty(25, ChanceProperty.NoMaxLimit);
        }
    }
}