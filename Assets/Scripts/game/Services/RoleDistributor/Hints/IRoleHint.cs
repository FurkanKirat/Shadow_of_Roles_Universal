using System.Collections.Generic;
using game.models.gamestate;
using Game.Models.Roles.Enums;
using game.models.roles.Templates;
using game.Utils;
using Managers;

namespace game.Services.RoleDistributor.Hints
{
    public interface IRoleHint
    {
        public RoleTemplate SelectRole(Dictionary<RoleTemplate,int> currentRoles, RolePack rolePack);
        public string Describe();
    }

    public class RoleIdHint : IRoleHint
    {
        private readonly RoleId _roleId;

        public RoleIdHint(RoleId roleId)
        {
            _roleId = roleId;
        }

        public RoleTemplate SelectRole(Dictionary<RoleTemplate, int> currentRoles, RolePack rolePack) 
            => RoleCatalog.GetRole(_roleId);
        
        public string Describe() => RoleCatalog.GetRole(_roleId).GetName();
 
    }
    
    public class MultipleRoleIdHint : IRoleHint
    {
        private readonly RoleId[] _roleIds;

        public MultipleRoleIdHint(RoleId[] roleIds)
        {
            _roleIds = roleIds;
        }

        public RoleTemplate SelectRole(Dictionary<RoleTemplate, int> currentRoles, RolePack rolePack)
            => DistributionHelper.GetRoleInGivenRolesWithProbability(currentRoles, _roleIds, rolePack);
        
        public string Describe() => _roleIds.Join(" & ", id => RoleCatalog.GetRole(id).GetName());
    }

    public class TeamHint : IRoleHint
    {
        private readonly Team _team;

        public TeamHint(Team team)
        {
            _team = team;
        }

        public RoleTemplate SelectRole(Dictionary<RoleTemplate, int> currentRoles, RolePack rolePack)
            => DistributionHelper.GetRoleByTeamWithProbability(currentRoles,_team,rolePack);
        
        public string Describe() => TextManager.Translate($"{_team.FormatEnum()}.name");
    }
    
    public class MultipleTeamHint : IRoleHint
    {
        private readonly Team[] _teams;

        public MultipleTeamHint(Team[] teams)
        {
            _teams = teams;
        }

        public RoleTemplate SelectRole(Dictionary<RoleTemplate, int> currentRoles, RolePack rolePack)
            => DistributionHelper.GetRoleByTeamWithProbability(currentRoles,_teams,rolePack);
        
        public string Describe() => _teams.Join(" & ", team => TextManager.Translate($"{team.FormatEnum()}.name"));
    }

    public class CategoryHint : IRoleHint
    {
        private readonly RoleCategory _category;

        public CategoryHint(RoleCategory category)
        {
            _category = category;
        }

        public RoleTemplate SelectRole(Dictionary<RoleTemplate, int> currentRoles, RolePack rolePack)
        {
            return DistributionHelper.GetRoleByCategoryWithProbability(currentRoles,_category,rolePack);
        }
        
        public string Describe() => TextManager.Translate($"role_categories.{_category.FormatEnum()}");
    }
    
    public class MultipleCategoryHint : IRoleHint
    {
        private readonly RoleCategory[] _categories;

        public MultipleCategoryHint(RoleCategory[] categories)
        {
            _categories = categories;
        }
        
        public RoleTemplate SelectRole(Dictionary<RoleTemplate, int> currentRoles, RolePack rolePack)
            => DistributionHelper.GetRoleByCategoryWithProbability(currentRoles,_categories,rolePack);
        
        public string Describe() => _categories.Join(" & ", category => TextManager.Translate($"role_categories.{category.FormatEnum()}"));
    }

    public class NoHint : IRoleHint
    {
        public RoleTemplate SelectRole(Dictionary<RoleTemplate, int> currentRoles, RolePack rolePack)
            => DistributionHelper.GetRoleWithProbability(currentRoles,rolePack);
        
        public string Describe() => TextManager.Translate("general.any");
    }

    public class SameChanceHint : IRoleHint
    {
        private readonly List<IRoleHint> _hints;

        public SameChanceHint(List<IRoleHint> hints)
        {
            _hints = hints;
        }

        public RoleTemplate SelectRole(Dictionary<RoleTemplate, int> currentRoles, RolePack rolePack)
        {
            return _hints.GetRandomElement().SelectRole(currentRoles,rolePack);
        }

        public string Describe()
        {
            return _hints.Join(" & ", hint => hint.Describe());
        }
    }

   
    
}