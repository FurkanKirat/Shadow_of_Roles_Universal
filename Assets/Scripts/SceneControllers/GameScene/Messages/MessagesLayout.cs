using System.Collections.Generic;
using game.models;
using game.models.gamestate;
using game.models.player;
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
        private Player _player;
        public void RefreshLayout(Player player, Dictionary<TimePeriod, List<Message>> messages)
        {
            if (_player != null && player.IsSamePlayer(_player) && messages.Count == _timePeriods.Count) return;
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