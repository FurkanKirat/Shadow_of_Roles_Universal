
using System.Collections.Generic;
using game.models.gamestate;
using game.Services;
using game.Services.RoleDistributor.GameRolesStrategy;
using game.Utils;
using Managers;
using Managers.enums;
using Networking.Client;
using Networking.Interfaces;
using Networking.Server;
using UnityEngine;
using UnityEngine.UI;

namespace SceneControllers.PlayerNames
{
    public class RolePackPanel : MonoBehaviour
    {
        [SerializeField] private Button startGameButton;
        [SerializeField] private RolePackContainer rolePackContainer;
        [SerializeField] private RolesInfoContainer rolesInfoContainer;
        [SerializeField] private PlayerNamesController playerNamesController;
        private RolePackInfo _currentRolePackInfo;
        public List<RolePackBox> RolePackBoxes { get; } = new();
        private int _selectedRolePackIndex = 0;

        public void Start()
        {
            rolePackContainer.Init();
            startGameButton.onClick.AddListener(StartGame);
            _currentRolePackInfo = RolePackCatalog.GetFirst();
            var roleBuilder = StrategyChooser.GetStrategy(_currentRolePackInfo.RolePack);
            var settings = new GameSettings(GameMode.Local, _currentRolePackInfo.RolePack, 10);
            rolesInfoContainer.ChangeInfo(roleBuilder.Build(settings));
            ChangeRolePackInfo(_currentRolePackInfo, _selectedRolePackIndex);
        }

        public void ChangeRolePackInfo(RolePackInfo rolePackInfo, int index)
        {
            _currentRolePackInfo = rolePackInfo;
            _selectedRolePackIndex = index;
            for (int i = 0 ; i < RolePackBoxes.Count ; ++i)
            {
                if(i == index) continue;
                RolePackBoxes[i].GetComponentInChildren<Button>().GetComponent<Image>().color = Color.white;
            }
            
            RolePackBoxes[_selectedRolePackIndex].GetComponentInChildren<Button>().GetComponent<Image>().color = Color.cyan;
        }
        
        private void StartGame()
        {
            var players = playerNamesController.Players;
            var gameSettings = new GameSettings(GameMode.Local, _currentRolePackInfo.RolePack, players.Count);

            var server = new LocalServer();
            var client = new LocalClient(server);
            
            server.SetClient(client);
            server.InitGameService(players, gameSettings);
            
            ServiceLocator.Register<IServer>(server);
            ServiceLocator.Register<IClient>(client);
            
            var sceneChanger = ServiceLocator.Get<SceneChanger>();
            sceneChanger.LoadScene(SceneType.Game);
        }
    }
}