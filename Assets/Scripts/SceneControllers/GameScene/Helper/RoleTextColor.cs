using game.Constants;
using Game.Models.Roles.Enums;
using UnityEngine;

namespace SceneControllers.GameScene.Helper
{
    public class RoleTextColor
    {
        private readonly WinningTeam _winningTeam;

        public RoleTextColor(WinningTeam winningTeam)
        {
            _winningTeam = winningTeam;
        }

        public Color GetColor()
        {
            return _winningTeam switch
            {
                WinningTeam.Folk => UIConstants.Colors.Green,
                WinningTeam.Corrupter => UIConstants.Colors.Red,
                _ => UIConstants.Colors.Aquamarine

            };
        }
    }
}