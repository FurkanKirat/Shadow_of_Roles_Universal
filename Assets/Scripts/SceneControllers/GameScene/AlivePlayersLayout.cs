using System.Collections;
using System.Collections.Generic;
using game.models.player;
using Managers;
using Networking.DataTransferObjects;
using Networking.Interfaces;
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
        private IClient _client;

        private void Start()
        {
            _client = ServiceLocator.Get<IClient>();
            StartCoroutine(WaitForGameInformation());
        }

        private IEnumerator WaitForGameInformation()
        {
            while (_client.GetCurrentGameInformation() == null)
            {
                yield return null;
            }

            InitializeLayout();
        }

        private void InitializeLayout()
        {
            var alivePlayers = _client.GetCurrentGameInformation().AlivePlayers;

            PlayerClick playerClick = (currentPlayer, targetPlayer, index) =>
            {
                var chosenPlayerNum = _client.GetCurrentClientInfo().TargetNumber;
                bool isChosen = chosenPlayerNum >= 0 && targetPlayer.IsSamePlayer(chosenPlayerNum);
                _client.GetCurrentClientInfo().TargetNumber = isChosen ? -1 : targetPlayer.Number;

                bool isSelected = _boxes[index].IsSelected;
                for (int i = 0; i < _boxes.Count; i++)
                {
                    _boxes[i].IsSelected = (i == index && !isSelected);
                }
            };

            for (int i = 0; i < alivePlayers.Count; i++)
            {
                var newBox = Instantiate(playerPrefab, transform);
                var boxScript = newBox.GetComponent<AlivePlayersBox>();
                boxScript.Init(
                    _client.GetCurrentGameInformation().CurrentPlayer,
                    alivePlayers[i],
                    _client.GetCurrentGameInformation().TimePeriod.Time,
                    playerClick,
                    i
                );
                _boxes.Add(boxScript);
            }
        }

        public void RefreshAllBoxes(PlayerDto newCurrentPlayer)
        {
            var alivePlayers = _client.GetCurrentGameInformation().AlivePlayers;
            foreach (var box in _boxes)
            {
                box.UpdatePlayer(newCurrentPlayer, alivePlayers[box.Index]);
            }
            playersScrollRect.ScrollToTop();
        }
        
        public void RefreshLayout(PlayerDto currentPlayer, Time time)
        {
            List<AlivePlayersBox> boxesToRemove = new ();
            int index = 0;
            var alivePlayers = _client.GetCurrentGameInformation().AlivePlayers;
            foreach (var box in _boxes)
            {
                if (!box.IsPlayerAlive())
                {
                    Destroy(box.gameObject);
                    boxesToRemove.Add(box);
                }
                else
                {
                    box.UpdateTime(currentPlayer, alivePlayers[index], time);
                    ++index;
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
    public delegate void PlayerClick(PlayerDto currentPlayer, PlayerDto targetPlayer, int index);
    
    
}