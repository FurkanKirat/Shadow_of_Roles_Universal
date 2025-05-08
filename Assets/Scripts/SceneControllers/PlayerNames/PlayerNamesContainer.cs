using System.Collections.Generic;
using game.Constants;
using Managers;
using SceneControllers.GameScene.Helper;
using UnityEngine;
using UnityEngine.UI;

namespace SceneControllers.PlayerNames
{
    public class PlayerNamesContainer : MonoBehaviour
    {
        
        [SerializeField] private GameObject objectPrefab;
        [SerializeField] private ScrollRect scrollRect;
        public List<PlayerNamesBoxScript> PlayerNames { get; } = new ();
        private void Start()
        {
            InitPlayerNames();
        }

        public bool AddPlayer()
        {
            int playerCount = PlayerNames.Count;
            if (playerCount >= GameConstants.MaxPlayerCount) return false;
            string playerNameTemp = TextManager.Translate("player_names.player");
            string playerName = string.Format(playerNameTemp, playerCount + 1);
                
            var newObject = Instantiate(objectPrefab,gameObject.transform);
                
            var boxScript = newObject.GetComponent<PlayerNamesBoxScript>();
            boxScript.SetIsAI(false);
            boxScript.SetPlayerName(playerName);
            PlayerNames.Add(boxScript);
            scrollRect.ScrollToBottom();
            return true;
        }

        public bool RemovePlayer()
        {
            int playerCount = PlayerNames.Count;
            if (playerCount <= GameConstants.MinPlayerCount) return false;
            var lastObject = PlayerNames[playerCount - 1];
            
            PlayerNames.Remove(lastObject);
            Destroy(lastObject.gameObject);
            scrollRect.ScrollToBottom();
            return true;
        }
        
        private void InitPlayerNames()
        {
            for (int i = 0; i < GameConstants.MinPlayerCount; i++)
            {
                AddPlayer();
            }
        }
        
        
    }

}
