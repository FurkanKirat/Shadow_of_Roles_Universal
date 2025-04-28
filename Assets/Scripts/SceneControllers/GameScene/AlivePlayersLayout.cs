using System.Collections.Generic;
using game.models;
using game.models.player;
using Managers;
using SceneControllers.GameScene.Helper;
using UnityEngine;
using UnityEngine.UI;
using Time = game.models.gamestate.Time;

namespace SceneControllers.GameScene
{
    public class AlivePlayersLayout : MonoBehaviour
    {
        [SerializeField] private GameObject playerPrefab;
        [SerializeField] private ScrollRect playersScrollRect;
        private readonly List<AlivePlayersBox> _boxes = new ();
        private IDataProvider _dataProvider;

        private void Awake()
        {
            var startGameManager = ServiceLocator.Get<StartGameManager>();
            _dataProvider = startGameManager.GameService;
            var alivePlayers = _dataProvider.GetAlivePlayers();
            
            PlayerClick playerClick = (currentPlayer, targetPlayer, index) =>
            {
                var chosenPlayer = currentPlayer.Role.ChosenPlayer;
                bool isChosen = chosenPlayer != null && chosenPlayer == targetPlayer;
                currentPlayer.Role.ChosenPlayer = isChosen ? null : targetPlayer;
                
                bool isSelected = _boxes[index].IsSelected;
                for (int i = 0; i < _boxes.Count; i++)
                {
                    _boxes[i].IsSelected = (i == index && !isSelected);
                }
            };
            
            for (int i=0 ; i < alivePlayers.Count; i++)
            {
                var newBox = Instantiate(playerPrefab, transform);
                var boxScript = newBox.GetComponent<AlivePlayersBox>();
                boxScript.Init(_dataProvider.GetCurrentPlayer(), alivePlayers[i], 
                    _dataProvider.GetTimePeriod().Time, playerClick,i);
                _boxes.Add(boxScript);
            }
        }
        
        public void RefreshAllBoxes(Player newCurrentPlayer)
        {
            foreach (var box in _boxes)
            {
                box.UpdatePlayer(newCurrentPlayer);
            }
            playersScrollRect.ScrollToTop();
        }
        
        public void RefreshLayout(Player currentPlayer, Time time)
        {
            List<AlivePlayersBox> boxesToRemove = new ();

            foreach (var box in _boxes)
            {
                if (!box.IsPlayerAlive())
                {
                    Destroy(box.gameObject);
                    boxesToRemove.Add(box);
                }
                else
                {
                    box.UpdateTime(currentPlayer, time);
                }
            }
            
            foreach (var box in boxesToRemove)
            {
                _boxes.Remove(box);
            }

            for (int i = 0; i < _boxes.Count; i++)
            {
                _boxes[i].Index = i;
            }
            playersScrollRect.ScrollToTop();
        }

    }
    
    

    public delegate void PlayerClick(Player currentPlayer, Player targetPlayer, int index);
    
    
}