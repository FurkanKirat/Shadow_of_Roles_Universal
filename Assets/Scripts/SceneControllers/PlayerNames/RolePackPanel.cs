
using System.Collections.Generic;
using game.models.gamestate;
using game.models.player;
using game.Services;
using game.Services.RoleDistributor.GameRolesStrategy;
using Managers;
using Managers.enums;
using Networking.Client;
using Networking.Interfaces;
using Networking.Server;
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
            var gameSettings = new GameSettings(GameMode, _currentRolePackInfo.RolePack, Players.Count);
            IServer server = null;
            IClient client = null;
            switch (gameSettings.GameMode)
            {
                case GameMode.Local:
                    var localServer = new LocalServer();
                    var localClient = new LocalClient(localServer);
                    localServer.SetClient(localClient);
                    localServer.InitGameService(Players, gameSettings);
                    server = localServer;
                    client = localClient;
                    break;
                
                case GameMode.Online:
                    var onlineServerGo = Instantiate(onlineServerPrefab);
                    var onlineServerNetObj = onlineServerGo.GetComponent<NetworkObject>();
                    onlineServerNetObj.Spawn();
                    DontDestroyOnLoad(onlineServerGo);
                    var onlineServer = onlineServerGo.GetComponent<OnlineServer>();
                    onlineServer.InitGameService(Players, gameSettings);
                    
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
            Debug.Log("Server started");
            
            var sceneChanger = ServiceLocator.Get<SceneChanger>();
            sceneChanger.LoadScene(SceneType.Game, gameSettings.GameMode == GameMode.Online);
        }
    }
}