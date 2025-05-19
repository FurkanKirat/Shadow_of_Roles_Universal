using System.Collections.Generic;
using System.Linq;
using game.models.gamestate;

namespace game.Services
{
    public static class RolePackCatalog
    {
        public static readonly Dictionary<RolePack, RolePackInfo> RolePackInfos = new()
        {
            { RolePack.Basic, new RolePackInfo(RolePack.Basic, false)},
            { RolePack.Classic, new RolePackInfo(RolePack.Classic, false)},
            { RolePack.Complex, new RolePackInfo(RolePack.Complex, false)},
            { RolePack.DarkChaos, new RolePackInfo(RolePack.DarkChaos, false)},
            { RolePack.Custom, new RolePackInfo(RolePack.Custom, false)},
        };
        
        
        public static RolePackInfo GetInfo(RolePack rolePack) => RolePackInfos[rolePack];

        public static List<RolePackInfo> GetAllModes(bool includePremium, bool hasPremiumAccess)
        {
            return RolePackInfos.Values
                .Where(info => !info.IsPremium || hasPremiumAccess || includePremium)
                .ToList();
        }

        public static RolePackInfo GetFirst()
        {
            return RolePackInfos.Values.First();
        }
    }
}