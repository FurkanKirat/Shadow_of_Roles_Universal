using Game.Models.Roles.Enums;
using UnityEngine;

namespace SceneControllers.GameScene.Helper
{
    public class RoleTextColor
    {
        private WinningTeam _winningTeam;

        public RoleTextColor(WinningTeam winningTeam)
        {
            _winningTeam = winningTeam;
        }

        public Color GetColor()
        {
            return _winningTeam switch
            {
                WinningTeam.Folk => Color.green,
                WinningTeam.Corrupter => Color.red,
                _ => Color.cyan

            };
        }
    }
}