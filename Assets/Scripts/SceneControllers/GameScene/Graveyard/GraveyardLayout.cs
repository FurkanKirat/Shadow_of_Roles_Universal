using System.Collections.Generic;
using game.models.player;
using Networking.DataTransferObjects;
using UnityEngine;

namespace SceneControllers.GameScene.Graveyard
{
    public class GraveyardLayout : MonoBehaviour
    {
        [SerializeField] private GameObject deathPlayerBoxPrefab;
        private List<PlayerDto> _players = new ();
        
        public void RefreshLayout(List<PlayerDto> deadPlayers)
        {

            if (_players.Count == deadPlayers.Count)
            {
                return;
            }
            foreach (var player in deadPlayers)
            {
                if(_players.Contains(player)) continue;
                
                var graveyardObject = Instantiate(deathPlayerBoxPrefab, gameObject.transform);
                var graveyardBox = graveyardObject.GetComponentInChildren<GraveyardBox>();
                graveyardBox.Init(player);
            }
            
            _players = new List<PlayerDto>(deadPlayers);
            
        }
    }
}