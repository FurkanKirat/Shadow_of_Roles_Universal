﻿using game.Utils;
using Managers;

namespace game.models.gamestate
{
    public class RolePackInfo
    {
        public RolePack RolePack { get;}
        public bool IsPremium { get;}
        
        public RolePackInfo(RolePack rolePack, bool ısPremium)
        {
            RolePack = rolePack;
            IsPremium = ısPremium;
        }
        
        public string GetName()
        {
            return TextManager.Translate($"role_pack.{RolePack.FormatEnum()}");
        }

        public override string ToString()
        {
            return $"{nameof(RolePack)}: {RolePack}, {nameof(IsPremium)}: {IsPremium}";
        }
    }
}