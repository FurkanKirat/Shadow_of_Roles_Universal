
using System.Collections.Generic;
using game.Constants;
using game.models.gamestate;
using game.models.player;
using game.Services;
using game.Services.RoleDistributor.GameRolesStrategy;
using Managers;
using Managers.enums;
using Networking.Client;
using Networking.Interfaces;
using Networking.Server;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

namespace SceneControllers.PlayerNames
{
    public class RolePackPanel : MonoBehaviour
    {
        [SerializeField] private Button startGameButton;
        [SerializeField] private RolePackContainer rolePackContainer;
        [SerializeField] private RolesInfoContainer rolesInfoContainer;
        [SerializeField] private GameObject onlineServerPrefab, onlineClientPrefab;
        [SerializeField] private CustomModeContainer customModeContainer;
        public List<Player> Players { get;} = new();
        private RolePackInfo _currentRolePackInfo;
        public List<RolePackBox> RolePackBoxes { get; } = new();
        public GameMode GameMode { get; set; }
        private int _selectedRolePackIndex = 0;

        public void Start()
        {
            rolePackContainer.Init();
            startGameButton.onClick.AddListener(StartGame);
            _currentRolePackInfo = RolePackCatalog.GetFirst();
            var roleBuilder = StrategyChooser.GetStrategy(_currentRolePackInfo.RolePack);
            var settings = new GameSettings(GameMode, _currentRolePackInfo.RolePack, 10);
            rolesInfoContainer.ChangeInfo(roleBuilder.Build(settings));
            ChangeRolePackInfo(_currentRolePackInfo, _selectedRolePackIndex);
        }

        public void ChangeRolePackInfo(RolePackInfo rolePackInfo, int index)
        {
            Debug.Log(rolePackInfo);
            customModeContainer.gameObject.SetActive(rolePackInfo.RolePack == RolePack.Custom);
            _currentRolePackInfo = rolePackInfo;
            _selectedRolePackIndex = index;
            for (int i = 0 ; i < RolePackBoxes.Count ; ++i)
            {
                if(i == index) continue;
                RolePackBoxes[i].GetComponentInChildren<Button>().GetComponent<Image>().color = UIConstants.Colors.White;
                RolePackBoxes[i].GetComponentInChildren<TextMeshProUGUI>().color = UIConstants.Colors.Aquamarine;
            }
            RolePackBoxes[_selectedRolePackIndex].GetComponentInChildren<TextMeshProUGUI>().color = UIConstants.Colors.White;
            RolePackBoxes[_selectedRolePackIndex].GetComponentInChildren<Button>().GetComponent<Image>().color = UIConstants.Colors.Aquamarine;
        }

        public void ChangeVisibility(bool isVisible)
        {
            if (isVisible)
            {
                gameObject.SetActive(true);
                customModeContainer.UpdatePlayerCount(Players.Count);
            }
            else
            {
                gameObject.SetActive(false);
            }
        }
        
        private void StartGame()
        {
            var gameSettings = new GameSettings(GameMode, _currentRolePackInfo.RolePack, Players.Count);
            IServer server = null;
            IClient client = null;
            bool success = false;
            if (_currentRolePackInfo.RolePack == RolePack.Custom)
            {
                StrategyChooser.CustomRoles = customModeContainer.StartGame();
            }
            switch (gameSettings.GameMode)
            {
                case GameMode.Local:
                    var localServer = new LocalServer();
                    var localClient = new LocalClient(localServer);
                    localServer.SetClient(localClient);
                    success = localServer.InitGameService(Players, gameSettings);
                    server = localServer;
                    client = localClient;
                    break;
                
                case GameMode.Online:
                    var onlineServerGo = Instantiate(onlineServerPrefab);
                    var onlineServerNetObj = onlineServerGo.GetComponent<NetworkObject>();
                    onlineServerNetObj.Spawn();
                    DontDestroyOnLoad(onlineServerGo);
                    var onlineServer = onlineServerGo.GetComponent<OnlineServer>();
                    success = onlineServer.InitGameService(Players, gameSettings);
                    
                    var onlineClientGo = Instantiate(onlineClientPrefab);
                    var onlineClientNetObj = onlineClientGo.GetComponent<NetworkObject>();
                    onlineClientNetObj.SpawnWithOwnership(NetworkManager.Singleton.LocalClientId);
                    DontDestroyOnLoad(onlineClientGo);
                    var onlineClient = onlineClientGo.GetComponent<OnlineClient>();
                    
                    server = onlineServer;
                    client = onlineClient;
                    break;
            }
            
            ServiceLocator.Register(client);
            ServiceLocator.Register(server);

            if (!success) return;
            
            var sceneChanger = ServiceLocator.Get<SceneChanger>();
            sceneChanger.LoadScene(SceneType.Game, gameSettings.GameMode == GameMode.Online);
        }
    }
}