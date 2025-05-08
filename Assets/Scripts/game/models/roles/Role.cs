using game.models.roles.Templates;

namespace game.models.roles
{
    public class Role
    {
        public RoleTemplate Template {get; set;}
        public int ChosenPlayer { get; set; }
        public bool CanPerform { get; set; }
        public bool IsImmune { get; set; }
        public bool IsRevealed { get; set; }

        public Role(RoleTemplate template) {

            Template = template;
            IsRevealed = false;
            ResetStates();
        }
        
        public void ResetStates(){
            ChosenPlayer = -1;
            Template.RoleProperties.Defence.Reset();
            Template.RoleProperties.Attack.Reset();
            
            var cooldown = Template.RoleProperties.Cooldown;
            if (cooldown.Default > 0 && cooldown.Current > 0)
            {
                Template.RoleProperties.Cooldown.DecrementCurrent();
            }
            
            CanPerform = true;
            IsImmune = false;
        }

        public override string ToString()
        {
            return $"Role{{" +
                   $"template={Template}, " +
                   $"choosenPlayer={ChosenPlayer}, " +
                   $"canPerform={CanPerform}, " +
                   $"isImmune={IsImmune}, " +
                   $"isRevealed={IsRevealed}," +
                   $"}}";
        }

    }
}