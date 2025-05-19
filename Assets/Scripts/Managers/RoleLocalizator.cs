using Game.Models.Roles.Enums;
using game.Utils;

namespace Managers
{
    public static class RoleLocalizator
    {
        public static string GetName(this RoleId roleId) {
            return TextManager.TranslateEnum(roleId,"name");
        }

        public static string GetAttributes(this RoleId roleId) {
            return TextManager.TranslateEnum(roleId,"attributes");
        }

        public static string GetAbilities(this RoleId roleId) {
            return TextManager.TranslateEnum(roleId,"abilities");
        }
        
        public static string GetCategoryText(this RoleCategory roleCategory)
        {
            return TextManager.Translate($"role_categories.{roleCategory.FormatEnum()}");
        }
        
        public static string GetTeamText(this WinningTeam winningTeam){
            return TextManager.TranslateEnum(winningTeam,"name");
        }

        public static string GetGoal(this WinningTeam winningTeam){
            return TextManager.TranslateEnum(winningTeam,"goal");
        }
    }
}