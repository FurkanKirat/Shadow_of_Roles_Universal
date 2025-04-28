using game.models.roles.Templates;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SceneControllers.GameScene.RoleBook
{
    public class RoleBookRoleBox : MonoBehaviour
    {
        [SerializeField] private Button roleButton;
        private RoleTemplate _role;
        private RoleBookPanel _roleBookPanel;
        private int _index;

        public void Initialize(RoleTemplate role, RoleBookPanel roleBookPanel, int index)
        {
            _role = role;
            _roleBookPanel = roleBookPanel;
            _index = index;
            
            roleButton.GetComponentInChildren<TextMeshProUGUI>().text = _role.GetName();
            roleButton.onClick.AddListener(BoxSelected);
        }

        private void BoxSelected()
        {
            _roleBookPanel.SelectRole(_role, _index);
        }
    }
}