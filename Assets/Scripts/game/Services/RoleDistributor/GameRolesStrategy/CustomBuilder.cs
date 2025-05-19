using System.Collections.Generic;
using game.models.gamestate;
using Game.Models.Roles.Enums;
using game.Services.RoleDistributor.Hints;

namespace game.Services.RoleDistributor.GameRolesStrategy
{
    public class CustomBuilder : IRoleBuilderStrategy
    {
        private readonly Dictionary<RoleId, int> roles;

        public CustomBuilder(Dictionary<RoleId, int> roles)
        {
            this.roles = roles;
        }

        public List<IRoleHint> Build(GameSettings gameSettings)
        {
            var builder = new RolesBuilder(gameSettings);

            foreach (var (roleId, count) in roles)
            {
                for (int i = 0; i < count; ++i)
                {
                    builder.AddHint(new RoleIdHint(roleId));
                }
            }

            return builder.Build();
        }
    }
}