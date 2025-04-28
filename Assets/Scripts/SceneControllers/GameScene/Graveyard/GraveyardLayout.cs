using System.Collections.Generic;
using game.models.player;
using UnityEngine;

namespace SceneControllers.GameScene.Graveyard
{
    public class GraveyardLayout : MonoBehaviour
    {
        [SerializeField] private GameObject deathPlayerBoxPrefab;
        private List<Player> _players = new ();
        
        public void RefreshLayout(List<Player> deadPlayers)
        {

            if (_players.Count == deadPlayers.Count)
            {
                return;
            }
            foreach (var player in deadPlayers)
            {
                if(_players.Contains(player)) continue;
                
                var timeObject = Instantiate(deathPlayerBoxPrefab, gameObject.transform);
                var timePeriodBox = timeObject.GetComponentInChildren<GraveyardBox>();
                timePeriodBox.Init(player);
            }
            
            _players = new List<Player>(deadPlayers);
            
        }
    }
}