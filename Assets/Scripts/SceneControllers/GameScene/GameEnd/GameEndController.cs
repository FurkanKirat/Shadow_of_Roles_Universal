using System.Collections.Generic;
using System.Linq;
using game.Constants;
using game.models;
using game.models.gamestate;
using Game.Models.Roles.Enums;
using game.Services.GameServices;
using game.Utils;
using Managers;
using Managers.enums;
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
        private IDataProvider _dataProvider;

        private void Start()
        {
            _dataProvider = ServiceLocator.Get<StartGameManager>().GameService;
            mainMenuButton.onClick.AddListener(GoToMainMenu);
            
            InitWinningTeam();
            InitTable();
            InitBackground();
            InitWinningTeamText();

            mainMenuButton.GetComponentInChildren<TextMeshProUGUI>().text 
                = TextCategory.Alerts.GetTranslation("go_back_to_main_menu");
            
        }

        private void InitTable()
        {
            var rows = _dataProvider.GetAllPlayers()
                .Select(player => new TableRowData(
                    player.Number.ToString(),
                    player.Name,
                    player.Role.Template.GetName(),
                    player.WinStatus.ToString(),
                    player.DeathProperties.IsAlive.ToString(),
                    player.DeathProperties.GetCausesOfDeathAsString()
                    ))
                .ToList();
            
            gameEndTable.PopulateTable(rows);
        }

        private void InitWinningTeam()
        {
            if (_dataProvider.GetGameSettings().GameMode == GameMode.Offline)
            {
                var gameService = _dataProvider as SingleDeviceGameService;
                _winningTeam = gameService.FinishGameService.GetHighestPriorityWinningTeam();
                
            }
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
                winnerTeamText = TextCategory.GameEnd.GetTranslation("draw");
            }
            else
            {
                winnerTeamText = TextCategory.GameEnd.GetTranslation("winner_team_text");
                string winnerTeamStr = TextCategory.GameEnd.GetTranslation(_winningTeam.FormatEnum());
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

