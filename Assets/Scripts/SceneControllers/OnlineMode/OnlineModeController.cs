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

        private void Start()
        {
            joinGameButton.onClick.AddListener(() => _ = OnJoinGameClicked());
            hostGameButton.onClick.AddListener(() => _ = OnHostGameClicked());
        }
        
        private async Task OnHostGameClicked()
        {
            var manager = new LobbyManager();
            try
            {
                await manager.CreateLobbyWithRelayAsync();
                Debug.Log("Lobby oluşturuldu: " + manager.Lobby.LobbyCode);

                gameLobbyController.gameObject.SetActive(true);
                gameLobbyController.JoinGame(manager);
                ServiceLocator.Register(manager);
            }
            catch (Exception ex)
            {
                Debug.LogError("Lobby oluşturulurken hata: " + ex.Message);
            }
        }

        private async Task OnJoinGameClicked()
        {
            string code = gameCodeInputField.text;
            if (string.IsNullOrEmpty(code)) return;
            try
            {
                var manager = new LobbyManager();
                await manager.JoinLobbyWithRelayAsync(code);
                Debug.Log("Lobbyye katıldın");
                gameLobbyController.gameObject.SetActive(true);
                gameLobbyController.JoinGame(manager);
                ServiceLocator.Register(manager);
            }
            catch (Exception e)
            {
                Debug.Log("Hata bulundu zorttttt: " + e);
                throw;
            }
            
        }
    }
}