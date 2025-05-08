using System.Collections.Generic;
using game.models;
using game.models.gamestate;
using game.models.player;
using Networking.DataTransferObjects;
using SceneControllers.GameScene.Helper;
using UnityEngine;
using UnityEngine.UI;

namespace SceneControllers.GameScene.Messages
{
    public class MessagesLayout : MonoBehaviour
    {
        [SerializeField] private GameObject messagePrefab, timePeriodPrefab;
        [SerializeField] private ScrollRect scrollRect;
        private readonly List<TimePeriod> _timePeriods = new ();
        private PlayerDto _player;
        public void RefreshLayout(PlayerDto player, Dictionary<TimePeriod, List<Message>> messages)
        {
            bool isSamePlayer = _player != null && player.IsSamePlayer(_player);
            bool messagesAreSame = messages.Count == _timePeriods.Count;
            if (isSamePlayer && messagesAreSame) return;
            
            foreach (Transform child in gameObject.transform) 
                Destroy(child.gameObject);
            _timePeriods.Clear();
            
            foreach (var (key, value) in messages)
            {
                if(_timePeriods.Contains(key)) continue;
                
                var timeObject = Instantiate(timePeriodPrefab, gameObject.transform);
                var timePeriodBox = timeObject.GetComponentInChildren<TimePeriodBox>();
                timePeriodBox.Init(key);
                _timePeriods.Add(key);
                foreach (var message in value)
                {
                    var messageBox = Instantiate(messagePrefab, gameObject.transform);
                    messageBox.GetComponentInChildren<MessageBox>().Init(message);
                }
            }
            _player = player;
            
            scrollRect.ScrollToBottom();
        }
        
    }
}