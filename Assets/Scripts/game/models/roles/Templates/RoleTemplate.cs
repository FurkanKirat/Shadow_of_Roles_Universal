using System;
using System.Collections.Generic;
using System.Linq;
using game.Constants;
using game.models.player;
using Game.Models.Roles.Enums;
using game.models.roles.interfaces.abilities;
using game.models.roles.properties;
using game.Services;
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

        public RoleTemplate(RoleId roleID, RoleCategory roleCategory, RolePriority rolePriority, AbilityType abilityType, WinningTeam winningTeam)
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

        protected void SendAbilityMessage(string message, Player receiver, MessageService messageService){
            messageService.SendAbilityMessage(message, receiver);
        }

        protected void SendAbilityAnnouncement(string message, MessageService messageService){
            messageService.SendAbilityAnnouncement(message);
        }
        
        public abstract ChanceProperty GetChanceProperty();
        
        public string GetTeamText(){
            return TextCategory.Teams.GetEnumTranslation(WinningTeam.GetTeam());
        }
        
        public string GetName() {
            return TextManager.GetEnumCategoryTranslation(RoleID,"name");
        }

        public string GetAttributes() {
            return TextManager.GetEnumCategoryTranslation(RoleID,"attributes");
        }

        public string GetAbilities() {
            return TextManager.GetEnumCategoryTranslation(RoleID,"abilities");
        }
        
        public string GetCategoryText()
        {
            return TextCategory.RoleCategories.GetEnumTranslation(RoleCategory);
        }

        public string GetGoal(){
            return TextCategory.Goals.GetEnumTranslation(WinningTeam);
        }
        
    }
}