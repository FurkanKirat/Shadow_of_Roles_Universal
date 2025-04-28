using game.models.player;
using Game.Models.Roles.Enums;
using game.models.roles.Templates;

namespace game.models.roles
{
    public class Role
    {
        public RoleTemplate Template {get; set;}
        
        public Player ChosenPlayer { get; set; }
        public bool CanPerform { get; set; }
        public AbilityResult AbilityResult { get; set; }

        public double Attack { get; set; }
        public double Defence {get;set;}
        public bool IsImmune { get; set; }
        public bool IsRevealed { get; set; }

        public Role(RoleTemplate template) {

            Template = template;
            IsRevealed = false;
            ResetStates();
        }
        
        public void ResetStates(){
            ChosenPlayer = null;
            Defence = Template.RoleProperties.Defence;
            Attack = Template.RoleProperties.Attack;
            CanPerform = true;
            IsImmune = false;
        }

        public override string ToString()
        {
            return $"Role{{" +
                   $"template={Template}, " +
                   $"choosenPlayer={ChosenPlayer}, " +
                   $"canPerform={CanPerform}, " +
                   $"abilityResult={AbilityResult}, " +
                   $"attack={Attack}, " +
                   $"defence={Defence}, " +
                   $"isImmune={IsImmune}, " +
                   $"isRevealed={IsRevealed}," +
                   $"}}";
        }

    }
}