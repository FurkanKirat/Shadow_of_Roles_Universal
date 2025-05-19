using System.Collections.Generic;
using System.Linq;
using game.models.gamestate;
using Game.Models.Roles.Enums;
using game.models.roles.Templates;

namespace game.Services.RoleDistributor
{
    public static class DistributionHelper
    {
        /**
         *
         * @param currentRoles the hash map of the roles that is created currently
         * @param roleCategory desired category
         * @return the role that is generated from the category list
         */
        public static RoleTemplate GetRoleByCategoryWithProbability(Dictionary<RoleTemplate,int> currentRoles, RoleCategory roleCategory, RolePack rolePack){
            var roles = new List<RoleTemplate>(RoleCatalog.GetRolesByCategory(roleCategory, rolePack));
            RemoveMaxCount(currentRoles,roles);
            return RoleCatalog.GetRoleWithProbability(roles);
        }

        /**
         *
         * @param currentRoles the hash map of the roles that is created currently
         * @param roleCategories desired categories
         * @return the role that is generated from the categories list
         */
        public static RoleTemplate GetRoleByCategoryWithProbability(Dictionary<RoleTemplate,int> currentRoles, RoleCategory[] roleCategories, RolePack rolePack){
            var roles = new List<RoleTemplate>();
            foreach(var roleCategory in roleCategories){

                roles.AddRange(RoleCatalog.GetRolesByCategory(roleCategory, rolePack));
            }
            RemoveMaxCount(currentRoles,roles);
            return RoleCatalog.GetRoleWithProbability(roles);
        }

        /**
         *
         * @param currentRoles the hash map of the roles that is created currently
         * @param team desired team
         * @return the role that is generated from the team list
         */
        public static RoleTemplate GetRoleByTeamWithProbability(Dictionary<RoleTemplate,int> currentRoles, Team team, RolePack rolePack){

            var roles = new List<RoleTemplate>(RoleCatalog.GetRolesByTeam(team, rolePack));
            RemoveMaxCount(currentRoles,roles);
            return RoleCatalog.GetRoleWithProbability(roles);
        }
        
        /**
         *
         * @param currentRoles the hash map of the roles that is created currently
         * @param team desired team
         * @return the role that is generated from the team list
         */
        public static RoleTemplate GetRoleByTeamWithProbability(Dictionary<RoleTemplate,int> currentRoles, Team[] teams, RolePack rolePack){

            var roles = new List<RoleTemplate>();
            foreach (Team team in teams)
            {
                roles.AddRange(RoleCatalog.GetRolesByTeam(team, rolePack));
            }
            RemoveMaxCount(currentRoles,roles);
            return RoleCatalog.GetRoleWithProbability(roles);
        }

        public static RoleTemplate GetRoleInGivenRolesWithProbability(Dictionary<RoleTemplate,int> currentRoles, RoleId[] roleIds, RolePack rolePack)
        {
            var roles = roleIds
                .Select(id => RoleCatalog.GetRole(id))
                .ToList();
            RemoveMaxCount(currentRoles,roles);
            return RoleCatalog.GetRoleWithProbability(roles);
        }
        
        public static RoleTemplate GetRoleWithProbability(Dictionary<RoleTemplate,int> currentRoles, RolePack rolePack)
        {
            return RoleCatalog.GetRoleWithProbability(RoleCatalog.GetAllRoles(rolePack));
        }

        /**
         * Removes the roles that are already in their max count
         * @param currentRoles the hash map of the roles that is created currently
         * @param randomRoleList the list that consists of desired roles
         */
        private static void RemoveMaxCount(Dictionary<RoleTemplate,int> currentRoles, List<RoleTemplate> randomRoleList){
            foreach(var entry in currentRoles){
                if(entry.Key.ChanceProperty.MaxNumber <= entry.Value){
                    randomRoleList.Remove(entry.Key);
                }
            }
        }
    }
}