using System;
using System.Collections.Generic;
using System.Linq;
using game.models.player;
using Game.Models.Roles.Enums;
using game.models.roles.interfaces.abilities;
using game.models.roles.properties;
using game.Services.GameServices;
using Managers;

namespace game.models.roles.Templates
{
    public abstract class RoleTemplate : IEquatable<RoleTemplate>, IPerformAbility
    {
        public RoleId RoleID { get; }
        
        public RoleCategory RoleCategory { get; }

        public RolePriority RolePriority { get; set; }
        public AbilityType AbilityType { get; set; }

        public WinningTeam WinningTeam { get; set; }
        public RoleProperties RoleProperties { get; }
        public ChanceProperty ChanceProperty { get; protected set; }
        protected RoleTemplate(RoleId roleID, RoleCategory roleCategory, RolePriority rolePriority, AbilityType abilityType, WinningTeam winningTeam)
        {
            RoleID = roleID;
            RoleCategory = roleCategory;
            RolePriority = rolePriority;
            AbilityType = abilityType;
            WinningTeam = winningTeam;
            RoleProperties = new RoleProperties();
        }

        public bool Equals(RoleTemplate other)
        {
            if(other == null) return false;
            return RoleID == other.RoleID;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as RoleTemplate);
        }

        public override int GetHashCode()
        {
            return RoleID.GetHashCode();
        }
        
        public RoleTemplate Copy()
        {
            try
            {
                return (RoleTemplate)Activator.CreateInstance(GetType())!;
            }
            catch (Exception e)
            {
                throw new InvalidOperationException("Cannot create copy of Role", e);
            }
        }

        
        public List<Player> FilterChoosablePlayers(Player roleOwner, List<Player> players){
            return players.Where(player =>
                    roleOwner.Role.Template.AbilityType.CanUseAbility(roleOwner,player)).ToList();
            
        }

        public abstract AbilityResult ExecuteAbility(Player roleOwner, Player choosenPlayer, BaseGameService gameService);
        public virtual AbilityResult PerformAbility(Player roleOwner, Player choosenPlayer, BaseGameService gameService){
            return ((IPerformAbility) this).DefaultPerformAbility(roleOwner,choosenPlayer,gameService);
        }
        
        
        public string GetName() {
            return RoleID.GetName();
        }

        public string GetAttributes() {
            return RoleID.GetAttributes();
        }

        public string GetAbilities() {
            return RoleID.GetAbilities();
        }
        
        public string GetCategoryText()
        {
            return RoleCategory.GetCategoryText();
        }
        
        public string GetTeamText()
        {
            return WinningTeam.GetTeamText();
        }
        
        public string GetGoal(){
            return WinningTeam.GetGoal();
        }
        
    }
}