using game.models.roles;
using Game.Models.Roles.Enums;
using game.models.roles.Templates;
using game.models.roles.Templates.FolkRoles;
using game.models.roles.Templates.NeutralRoles;
using game.Services;

namespace Networking.DataTransferObjects.RoleViewStrategies
{
    public static class RoleDataDtoBuilder
    {
        public static RoleDto Build(Role role, RoleInfoLevel level, int playerCount)
        {
            RoleTemplate roleTemplate = role.Template;
            var dto = new RoleDto();

            switch (level)
            {
                case RoleInfoLevel.None:
                    return null;

                case RoleInfoLevel.OthersInfo:
                    
                    if (role.IsRevealed)
                    {
                        dto.IsRevealed = role.IsRevealed;
                        dto.RoleId = role.Template.RoleID;
                    }
                    break;

                case RoleInfoLevel.RoleRevealedInfo:
                    dto.IsRevealed = role.IsRevealed;
                    dto.RoleId = roleTemplate.RoleID;
                    break;

                case RoleInfoLevel.SelfInfo:
                case RoleInfoLevel.FullInfo:
                    dto.IsRevealed = role.IsRevealed;
                    dto.RoleId = roleTemplate.RoleID;
                    dto.Money = roleTemplate.RoleProperties.Money.Current;
                    dto.AbilityUsesLeft = roleTemplate.RoleProperties.AbilityUsesLeft.Current;
                    dto.Cooldown = roleTemplate.RoleProperties.Cooldown.Current;
                    
                    switch (roleTemplate.RoleID)
                    {
                        case RoleId.Entrepreneur:
                            var entrepreneur = roleTemplate as Entrepreneur;
                            dto.ExtraData[ExtraData.EntrepreneurTargetAbility] = entrepreneur.TargetAbility;
                            break;
                        case RoleId.LoreKeeper:
                            var lorekeeper = roleTemplate as LoreKeeper;
                            dto.ExtraData[ExtraData.LoreKeeperAlreadyChosenPlayers] = lorekeeper.AlreadyChosenPlayers;
                            dto.ExtraData[ExtraData.LoreKeeperCurrentGuess] = lorekeeper.TrueGuessCount;
                            dto.ExtraData[ExtraData.LoreKeeperTargetGuess] = AbilityService.GetLoreKeeperWinningCount(playerCount);
                            break;
                    }
                    
                    break;
            }

            return dto;
        }
    }
    
}