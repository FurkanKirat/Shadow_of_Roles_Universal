using game.Constants;
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
            return TextCategory.RolePack.GetEnumTranslation(RolePack);
        }
        
    }
}