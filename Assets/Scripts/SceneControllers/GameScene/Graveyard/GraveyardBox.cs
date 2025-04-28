using game.models.player;
using TMPro;
using UnityEngine;

namespace SceneControllers.GameScene.Graveyard
{
    public class GraveyardBox : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI playerNameText, deathTimeText;
        private Player _player;

        public void Init(Player player)
        {
            _player = player;
            playerNameText.text = _player.Name;
            deathTimeText.text = _player.DeathProperties.GetDeathTimeAndDayCount();
        }
    }
}

