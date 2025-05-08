using game.models.gamestate;
using game.Services.RoleDistributor.GameRolesStrategy;
using SceneControllers.GameScene.Helper;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SceneControllers.PlayerNames
{
    public class RolePackBox : MonoBehaviour
    {
        [SerializeField] private Button rolePackButton;
        private ScrollRect _rolesScrollRect;
        private RolesInfoContainer _rolesInfoContainer;
        private RolePackInfo _rolePackInfo;
        private RolePackPanel _rolePackPanel;
        private int _index;

        public void Initialize(RolePackInfo rolePackInfo, RolesInfoContainer rolesInfoContainer
            , RolePackPanel rolePackPanel, ScrollRect rolesScrollRect, int index)
        {
            _rolePackInfo = rolePackInfo;
            _rolesInfoContainer = rolesInfoContainer;
            _rolePackPanel = rolePackPanel;
            _rolesScrollRect = rolesScrollRect;
            _index = index;
            
            rolePackButton.GetComponentInChildren<TextMeshProUGUI>().text = _rolePackInfo.GetName();
            rolePackButton.onClick.AddListener(BoxSelected);
        }

        private void BoxSelected()
        {
            var builder = StrategyChooser.GetStrategy(_rolePackInfo.RolePack);
            _rolesInfoContainer.ChangeInfo(builder.Build(new GameSettings(GameMode.Local, _rolePackInfo.RolePack, 10)));
            
            _rolePackPanel.ChangeRolePackInfo(_rolePackInfo, _index);
            _rolesScrollRect.ScrollToTop();
        }
        
    }
}