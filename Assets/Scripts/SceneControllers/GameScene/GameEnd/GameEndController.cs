using System.Linq;
using Game.Models.Roles.Enums;
using game.Services;
using game.Utils;
using Managers;
using Managers.enums;
using Networking.Interfaces;
using SceneControllers.GameScene.Helper;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SceneControllers.GameScene.GameEnd
{
    public class GameEndController : MonoBehaviour
    {

        [SerializeField] private Image background;
        [SerializeField] private GridLayoutGroup table;
        [SerializeField] private Button mainMenuButton;
        [SerializeField] private GameEndTable gameEndTable;
        [SerializeField] private TextMeshProUGUI gameEndText;
        private WinningTeam _winningTeam;
        private IClient _client;

        private void Start()
        {
            _client = ServiceLocator.Get<IClient>();
            mainMenuButton.onClick.AddListener(GoToMainMenu);
            
            InitWinningTeam();
            InitTable();
            InitBackground();
            InitWinningTeamText();

            mainMenuButton.GetComponentInChildren<TextMeshProUGUI>().text 
                = TextManager.Translate("alerts.go_back_to_main_menu");
            
        }

        private void InitTable()
        {
            var rows = _client.GetCurrentGameInformation().AllPlayers
                .Select(player => new TableRowData(
                    player.Number.ToString(),
                    player.Name,
                    RoleCatalog.GetRole(player.RoleDto.RoleId).GetName(),
                    player.WinStatus.ToString(),
                    player.DeathProperties.IsAlive.ToString(),
                    player.DeathProperties.GetCausesOfDeathAsString()
                    ))
                .ToList();
            
            gameEndTable.PopulateTable(rows);
        }

        private void InitWinningTeam()
        {
            _winningTeam = _client.GetCurrentGameInformation().WinnerTeam;
        }
        private void InitBackground()
        {
            background.sprite = _winningTeam switch
            {
                WinningTeam.Folk => Resources.Load<Sprite>("Canvas/GameEnd/folk"),
                WinningTeam.Corrupter => Resources.Load<Sprite>("Canvas/GameEnd/corrupt"),
                _ => Resources.Load<Sprite>("Canvas/GameEnd/neutral")
            };
        }

        private void InitWinningTeamText()
        {
            string winnerTeamText;
            if (_winningTeam == WinningTeam.Unknown)
            {
                winnerTeamText = TextManager.Translate("game_end.draw");
            }
            else
            {
                winnerTeamText = TextManager.Translate("game_end.winner_team_text");
                string winnerTeamStr = TextManager.Translate($"game_end.{_winningTeam.FormatEnum()}");
                winnerTeamText = winnerTeamText.Replace("{team}", winnerTeamStr);
            }

            gameEndText.text = winnerTeamText;
        }

        private void GoToMainMenu()
        {
            SceneChanger sceneChanger = ServiceLocator.Get<SceneChanger>();
            sceneChanger.LoadScene(SceneType.MainMenu);
        }
    }
}

