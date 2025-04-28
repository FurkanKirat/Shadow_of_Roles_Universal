using System;
using System.Collections.Generic;
using game.models.gamestate;
using Game.Models.Roles.Enums;

namespace game.Services.RoleDistributor
{
    public static class RoleFilterByRuleSet
    {
        private static readonly Dictionary<RolePack, HashSet<RoleId>> AllowedRoles = new()
        {
            {
                RolePack.All, new HashSet<RoleId>((RoleId[])Enum.GetValues(typeof(RoleId)))
            },
            
            {
                RolePack.Classic, new HashSet<RoleId>
                {
                    RoleId.ChillGuy,
                    RoleId.Detective,
                    RoleId.Observer,
                    RoleId.SoulBinder,
                    RoleId.Stalker,
                    RoleId.Psycho,
                    RoleId.DarkRevealer,
                    RoleId.Interrupter,
                    RoleId.SealMaster,
                    RoleId.Assassin,
                    RoleId.Clown,
                    
                }
            },

            {
                RolePack.Basic, new HashSet<RoleId>
                {
                    RoleId.Psycho,
                    RoleId.SoulBinder,
                    RoleId.ChillGuy
                }
            },

            {
                RolePack.Complex, new HashSet<RoleId>
                {
                    RoleId.Detective,
                    RoleId.Observer,
                    RoleId.SoulBinder,
                    RoleId.Stalker,
                    RoleId.Psycho,
                    RoleId.DarkRevealer,
                    RoleId.Interrupter,
                    RoleId.SealMaster,
                    RoleId.Assassin,
                    RoleId.LastJoke,
                    RoleId.Clown,
                    RoleId.Disguiser,
                    RoleId.DarkSeer,
                    RoleId.Blinder,
                    RoleId.FolkHero,
                    RoleId.Entrepreneur,
                    RoleId.LoreKeeper
                }
            },
            {
                RolePack.DarkChaos, new HashSet<RoleId>
                {
                    RoleId.ChillGuy,
                    RoleId.Detective,
                    RoleId.Observer,
                    RoleId.SoulBinder,
                    RoleId.Stalker,
                    RoleId.Psycho,
                    RoleId.DarkRevealer,
                    RoleId.Interrupter,
                    RoleId.SealMaster,
                    RoleId.Assassin,
                    RoleId.LastJoke,
                    RoleId.Clown,
                    RoleId.Disguiser,
                    RoleId.DarkSeer,
                    RoleId.Blinder,
                    RoleId.FolkHero,
                    RoleId.Entrepreneur,
                    RoleId.LoreKeeper
                }
                
            },
            
            
            
        };

        public static bool IsAllowed(RoleId roleId, RolePack rolePack)
        {
            return AllowedRoles.TryGetValue(rolePack, out var allowedSet) && allowedSet.Contains(roleId);
        }

        public static HashSet<RoleId> GetAllowedRoles(RolePack rolePack)
        {
            return AllowedRoles.TryGetValue(rolePack, out var allowedSet)
                ? allowedSet
                : new HashSet<RoleId>();
        }
    }
}