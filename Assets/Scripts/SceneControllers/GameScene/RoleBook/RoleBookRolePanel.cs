using game.models.roles.Templates;
using TMPro;
using UnityEngine;

namespace SceneControllers.GameScene.RoleBook
{
    public class RoleBookRolePanel : MonoBehaviour
    {
        [SerializeField] private GameObject prefab;
        [SerializeField] private Transform parent;
        [SerializeField] private TextMeshProUGUI rName, rTeam, rCategory, rGoal, rAbilities, rAttributes; 
        private RoleTemplate _role;

        public void ChangeInfo(RoleTemplate role)
        {
            _role = role;

            rName.text = _role.GetName();
            rTeam.text = _role.GetTeamText();
            rGoal.text = _role.GetGoal();
            rCategory.text = _role.GetCategoryText();
            rAbilities.text = _role.GetAbilities();
            rAttributes.text = _role.GetAttributes();
        }
    }
}