using System.Collections.Generic;
using System.Linq;
using game.models.gamestate;
using Game.Models.Roles.Enums;
using game.models.roles.Templates;
using game.models.roles.Templates.CorruptedRoles;
using game.models.roles.Templates.FolkRoles;
using game.models.roles.Templates.NeutralRoles;
using game.Services.RoleDistributor;
using game.Utils;

namespace game.Services
{
    public static class RoleCatalog
    {
  
        private static readonly List<RoleTemplate> AllRoles;
        private static readonly Dictionary<RoleId, RoleTemplate> RolesDictionary = new();
        
        // Adds all roles to the catalog
        static RoleCatalog(){
            AllRoles = new List<RoleTemplate>{
                new Detective(), new Psycho(), new Observer(), new SoulBinder(), new Stalker(),
                new DarkRevealer(), new Interrupter(), new SealMaster(), new Assassin(),
                new ChillGuy(), new Clown(), new Disguiser(), new DarkSeer(), new Blinder(),
                new LastJoke(), new FolkHero(), new Entrepreneur(), new LoreKeeper()
            };
            AddRoles(AllRoles);
        }
        
        /**
     * Adds role to the catalog
     * @param roles the roles to be added to the role catalog
     */
        private static void AddRoles(List<RoleTemplate> roles){
            foreach(var role in roles)
            {
                RolesDictionary.Add(role.RoleID, role);
            }
        }
        
        
        /**
         *
         * @param team the desired team
         * @return a list that consist of the desired team
         */
        public static List<RoleTemplate> GetRolesByTeam(Team team, RolePack rolePack)
        {
            var allRolesForTeam = RolesDictionary.Values.Where(r => r.WinningTeam.GetTeam() == team);
            var allowedIDs = RoleFilterByRuleSet.GetAllowedRoles(rolePack);
    
            return allRolesForTeam
                .Where(role => allowedIDs.Contains(role.RoleID))
                .ToList();
        }


        /**
         *
         * @param roleCategory the desired category
         * @return a list that consist of the desired category
         */
        public static List<RoleTemplate> GetRolesByCategory(RoleCategory roleCategory, RolePack rolePack)
        {
            var allRolesForTeam = RolesDictionary.Values.Where(r => r.RoleCategory == roleCategory);
            var allowedIDs = RoleFilterByRuleSet.GetAllowedRoles(rolePack);
            
            return allRolesForTeam
                .Where(role => allowedIDs.Contains(role.RoleID))
                .ToList();;
        }
        /**
         *
         * @return a copy list of all roles
         */
        public static List<RoleTemplate> GetAllRoles(RolePack rolePack){
            return AllRoles
                .Where(template => RoleFilterByRuleSet.IsAllowed(template.RoleID, rolePack))
                .ToList();
        }

        public static List<RoleTemplate> GetAllRoles()
        {
            return AllRoles;
        }

        /**
         *
         * @param otherRole the role that is not wanted to return
         * @return a random role other than the parameter role
         */
        public static RoleTemplate GetRandomRole(RoleTemplate otherRole){
            var otherRoles = new List<RoleTemplate>(AllRoles);
            otherRoles.Remove(otherRole);
            return otherRoles.GetRandomElement().Copy();
        }
        
        
        /**
         *
         * @return a random role in the catalog
         */
        public static RoleTemplate GetRandomRole(){
            return AllRoles.GetRandomElement().Copy();
        }

        public static RoleTemplate GetRole(RoleId roleId)
        {
            if (roleId == RoleId.None) return null;
            return RolesDictionary[roleId];
        }
        
        /**
         * @param randomRoleList the list that consists of desired roles
         * @return a generated role from the randomRoleList with the probability of the roles
         */
        public static RoleTemplate GetRoleWithProbability(List<RoleTemplate> randomRoleList){

            int sum = randomRoleList.Select(template => template.ChanceProperty.Chance).Sum();
            int randNum = RandomUtils.GetRandomNumber(0,sum);
            int currentSum = 0;

            foreach (RoleTemplate role in randomRoleList) {
                currentSum += role.ChanceProperty.Chance;

                if (currentSum >= randNum) {
                    return role.Copy();
                }
            }
            return null;
        }

    }
}