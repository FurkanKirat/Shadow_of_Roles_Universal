using System;
using System.Collections.Generic;
using game.models.gamestate;
using game.Services.RoleDistributor.Hints;

namespace game.Services.RoleDistributor
{
    public class RolesBuilder
    {
        private readonly List<IRoleHint> _alwaysHints = new();
        private readonly List<ConditionalHint> _conditionalHints = new();
        private readonly GameSettings _gameSettings;

        public RolesBuilder(GameSettings gameSettings)
        {
            _gameSettings = gameSettings;
        }
        public RolesBuilder AddHint(IRoleHint hint)
        {
            _alwaysHints.Add(hint);
            return this;
        }

        public RolesBuilder AddConditionalHint(Func<GameSettings, bool> condition, IRoleHint hint)
        {
            _conditionalHints.Add(new ConditionalHint(condition, hint));
            return this;
        }

        public List<IRoleHint> Build()
        {
            var hints = new List<IRoleHint>(_alwaysHints);

            foreach (var condHint in _conditionalHints)
            {
                if (condHint.Condition(_gameSettings))
                {
                    hints.Add(condHint.Hint);
                }
            }
            
            return hints;
        }
    }
}