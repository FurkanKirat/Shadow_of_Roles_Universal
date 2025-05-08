using Networking.DataTransferObjects;
using TMPro;
using UnityEngine;

namespace SceneControllers.GameScene.Graveyard
{
    public class GraveyardBox : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI playerNameText, deathTimeText;
        private PlayerDto _player;

        public void Init(PlayerDto player)
        {
            _player = player;
            playerNameText.text = _player.Name;
            deathTimeText.text = _player.DeathProperties.GetDeathTimeAndDayCount();
        }
    }
}

