using System;
using game.models.gamestate;

namespace game.Services.RoleDistributor.Hints
{
    public class ConditionalHint
    {
        public Func<GameSettings, bool> Condition{get;} 
        public IRoleHint Hint { get; }

        public ConditionalHint(Func<GameSettings, bool> condition, IRoleHint hint)
        {
            Condition = condition;
            Hint = hint;
        }
    }
}