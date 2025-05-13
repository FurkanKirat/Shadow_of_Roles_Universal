using System;
using System.Threading.Tasks;
using Managers;
using Networking.Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SceneControllers.OnlineMode
{
    public class OnlineModeController : MonoBehaviour
    {
        [SerializeField] private Button joinGameButton, hostGameButton;
        [SerializeField] private TMP_InputField gameCodeInputField;
        [SerializeField] private GameLobbyController gameLobbyController;
        [SerializeField] private GameObject lobbyManager, clientPrefab;
        private void Start()
        {
            joinGameButton.onClick.AddListener(() => _ = OnJoinGameClicked());
            hostGameButton.onClick.AddListener(() => _ = OnHostGameClicked());
        }
        
        private async Task OnHostGameClicked()
        {
            var managerObj = Instantiate(lobbyManager);
            var manager = managerObj.GetComponent<LobbyManager>();
            DontDestroyOnLoad(manager);

            try
            {
                Debug.Log("[HostGame] Lobi ve Relay oluşturuluyor...");
                await manager.CreateLobbyWithRelayAsync();

                if (manager.Lobby == null || string.IsNullOrEmpty(manager.Lobby.LobbyCode))
                {
                    Debug.LogError("[HostGame] Lobby oluşturulamadı veya lobbyCode boş!");
                    return;
                }

                Debug.Log($"[HostGame] Lobby oluşturuldu: {manager.Lobby.LobbyCode}");

                gameLobbyController.gameObject.SetActive(true);
                await gameLobbyController.JoinGame(manager, true);
                ServiceLocator.Register(manager);
            }
            catch (Exception ex)
            {
                Debug.LogError("[HostGame] Lobby oluşturulurken hata: " + ex.Message + "\n" + ex);
            }
        }

      private async Task OnJoinGameClicked()
        {
            string code = gameCodeInputField.text.Trim().ToUpper(); // Güvenli giriş
            if (string.IsNullOrEmpty(code))
            {
                Debug.LogWarning("[JoinGame] Lobi kodu boş.");
                return;
            }

            try
            {
                Debug.Log($"[JoinGame] Kullanıcı şu kod ile lobbyye katılmaya çalışıyor: {code}");

                var managerObj = Instantiate(lobbyManager);
                if (managerObj == null)
                {
                    Debug.LogError("[JoinGame] LobbyManager prefab instantiate edilemedi!");
                    return;
                }

                var manager = managerObj.GetComponent<LobbyManager>();
                if (manager == null)
                {
                    Debug.LogError("[JoinGame] LobbyManager component eksik!");
                    return;
                }

                DontDestroyOnLoad(manager);
                Debug.Log("[JoinGame] LobbyManager başlatılıyor...");

                await manager.JoinLobbyWithRelayAsync(code);

                if (manager.Lobby == null)
                {
                    Debug.LogError("[JoinGame] Lobiye katılım başarısız: Lobby null.");
                    return;
                }

                Debug.Log("[JoinGame] Lobbyye katılma başarılı!");
                
                // Lobi UI gösteriliyor
                gameLobbyController.gameObject.SetActive(true);
                await gameLobbyController.JoinGame(manager, false);

                ServiceLocator.Register(manager);
            }
            catch (Exception e)
            {
                Debug.LogError("[JoinGame] Hata bulundu zorttttt: " + e.Message + "\n" + e);
            }
        }
      
    }
}