using System.Collections.Generic;
using System.Linq;
using game.Constants;
using game.models.gamestate;
using game.models.player;
using game.models.player.properties;
using Game.Models.Roles.Enums;
using game.models.roles.properties;
using game.models.roles.Templates;
using game.models.roles.Templates.NeutralRoles;
using game.Services.GameServices;
using Unity.VisualScripting;

namespace game.Services
{
    public class FinishGameService
    {
        [DoNotSerialize] private readonly BaseGameService _gameService;
        private readonly SortedSet<WinningTeam> _winningTeams = new ();
        private readonly HashSet<WinningTeam> _drawTeams = new ();
        public bool IsGameFinished {get; private set;}
        public FinishGameService(BaseGameService gameService){
            _gameService = gameService;
        }
        
        /**
         * Checks the game if it is finished and finishes the game
         * @return game is finished or not
         */
        private GameEndResult CheckGameFinished(){

            List<Player> alivePlayers = _gameService.GetAlivePlayersAsPlayerList();

            SortedSet<WinningTeam> currentTeams = new SortedSet<WinningTeam>(
                alivePlayers.Select(player => player.Role.Template.WinningTeam));

            int playerCount = alivePlayers.Count;
            // Finishes the game if only 1 player is alive or nobody is alive
            if(playerCount == 0){
                return new GameEndResult(true, GameEndReason.NoPlayersAlive, WinStatus.Tied);
            }

            if(playerCount == 1){
                return new GameEndResult(true, GameEndReason.SinglePlayerRemains, WinStatus.Won);
            }

            if (currentTeams.Count < 2 && (currentTeams.Contains(WinningTeam.Folk) || 
                                    currentTeams.Contains(WinningTeam.Corrupter))) {
                return new GameEndResult(true, GameEndReason.AllSameTeam, WinStatus.Won);
            }


            // If only 2 players are alive checks the game if it is finished
            if(playerCount == 2){

                Player player1 = _gameService.GetPlayer(_gameService.AlivePlayers[0]);
                Player player2 = _gameService.GetPlayer(_gameService.AlivePlayers[1]);


                // If one of the players' role can win with other teams finishes the game
                bool canWinTogether = player1.Role.Template.WinningTeam
                        .CanWinWith(player2.Role.Template.WinningTeam);


                // Finishes the game if the last two players cannot kill each other
                bool playersCanKill = (player2.Role.Template.RoleProperties.Attack.Current > player1.Role.Template.RoleProperties.Defence.Current
                                       &&player2.Role.Template.RoleProperties.HasAttribute(RoleAttribute.CanKill1V1))
                                      ||(player1.Role.Template.RoleProperties.Attack.Current > player2.Role.Template.RoleProperties.Defence.Current
                                         &&player1.Role.Template.RoleProperties.HasAttribute(RoleAttribute.CanKill1V1));

                // Finishes the game if one of the last two players can role block and the other is not immune to role block
                bool p1CanBlock = player1.Role.Template.RoleProperties.HasAttribute(RoleAttribute.CanRoleBlock);
                bool p1BlockImmune = player1.Role.Template.RoleProperties.HasAttribute(RoleAttribute.RoleBlockImmune);

                bool p2CanBlock = player2.Role.Template.RoleProperties.HasAttribute(RoleAttribute.CanRoleBlock);
                bool p2BlockImmune = player2.Role.Template.RoleProperties.HasAttribute(RoleAttribute.RoleBlockImmune);

                bool roleBlockCheck = (!p1BlockImmune && p2CanBlock) || (!p2BlockImmune && p1CanBlock);


                if(canWinTogether){
                    return new GameEndResult(true, GameEndReason.OnlyTwoCanWinTogether, WinStatus.Won);
                }

                if(!playersCanKill || roleBlockCheck){
                    return new GameEndResult(true, GameEndReason.OnlyTwoPlayersCannotKillEachOther, WinStatus.Tied);
                }


            }

            TimePeriod currentPeriod = _gameService.TimeService.TimePeriod;
            if(currentPeriod.Time == Time.Day){
                TimePeriod lastKillPeriod = FindLastKilledTime();
                int daysWithoutKilling = currentPeriod.Subtract(lastKillPeriod) - 1;
                if(daysWithoutKilling > GameConstants.MaxNightsWithoutKills)
                    return new GameEndResult(true, GameEndReason.NoKillsInMultipleNights, WinStatus.Tied);
            }

            return new GameEndResult(false, GameEndReason.None, WinStatus.Unknown);
        }

        /**
         * Finishes the game if the end conditions are taken place
         */
        public void FinishGame(){

            GameEndResult gameEndResult = CheckGameFinished();

            if(!gameEndResult.GameFinished){
                return;
            }

            IsGameFinished = true;

            List<Player> allPlayers = _gameService.AllPlayers.Values.ToList();
            List<Player> alivePlayers = _gameService.GetAlivePlayersAsPlayerList();

            Player player1, player2;
            RoleTemplate role;
            switch (gameEndResult.Reason){
                case GameEndReason.SinglePlayerRemains:
                    player1 = alivePlayers[0];
                    role = player1.Role.Template;
                    if(!role.RoleProperties
                            .HasAttribute(RoleAttribute.HasOtherWinCondition)){
                        _winningTeams.Add(role.WinningTeam);
                        player1.SetWinStatus(WinStatus.Won);
                    }
                    break;

                case GameEndReason.AllSameTeam:
                    player1 = alivePlayers[0];
                    role = player1.Role.Template;
                    _winningTeams.Add(role.WinningTeam);
                    break;

                case GameEndReason.OnlyTwoCanWinTogether:
                    player1 = alivePlayers[0];
                    player2 = alivePlayers[1];

                    if(player1.Role.Template.WinningTeam
                            < player2.Role.Template.WinningTeam){
                        _winningTeams.Add(player1.Role.Template.WinningTeam);
                        player1.SetWinStatus(WinStatus.Won);
                    }
                    else{
                        _winningTeams.Add(player2.Role.Template.WinningTeam);
                        player2.SetWinStatus(WinStatus.Won);
                    }
                    break;

                case GameEndReason.OnlyTwoPlayersCannotKillEachOther:
                    player1 = alivePlayers[0];
                    player2 = alivePlayers[1];
                    _drawTeams.Add(player1.Role.Template.WinningTeam);
                    _drawTeams.Add(player2.Role.Template.WinningTeam);
                    player1.SetWinStatus(WinStatus.Tied);
                    player2.SetWinStatus(WinStatus.Tied);
                    break;

                case GameEndReason.NoPlayersAlive:
                    var lastLivingPlayers = FindLastLivingPlayers();
                    HandleDraw(lastLivingPlayers);
                    break;

                case GameEndReason.NoKillsInMultipleNights:
                    HandleDraw(alivePlayers);
                    break;

            }

            foreach(var player in allPlayers){
                if(player.Role.Template.RoleProperties.HasAttribute(RoleAttribute.WinsAlone)) continue;

                if(_winningTeams.Contains(player.Role.Template.WinningTeam)){
                    player.SetWinStatus(WinStatus.Won);
                } else if (_drawTeams.Contains(player.Role.Template.WinningTeam)) {
                    player.SetWinStatus(WinStatus.Tied);
                }
                

            }


            foreach(var player in allPlayers){

                switch (player.Role.Template.WinningTeam) {

                    case WinningTeam.Clown:
                        if (!player.DeathProperties.IsAlive 
                            && !player.DeathProperties.HasCause(CauseOfDeath.Hanging)) {
                            player.SetWinStatus(WinStatus.Won);
                            AddWinningTeam(WinningTeam.Clown);
                        }
                        break;

                    case WinningTeam.LoreKeeper:
                        LoreKeeper lorekeeper = (LoreKeeper) player.Role.Template;
                        int winCount  = _gameService.AbilityService.GetLoreKeeperWinningCount();

                        if (lorekeeper.TrueGuessCount >= winCount) {
                            player.SetWinStatus(WinStatus.Won);
                            AddWinningTeam(WinningTeam.LoreKeeper);
                        }
                        break;
                    
                    
                }

            }

            ResetGame();
        }


        private void HandleDraw(List<Player> players)
        {
            foreach(Player player in players){
                        
                if(player.Role.Template.RoleProperties.HasAttribute(RoleAttribute.HasOtherWinCondition)) continue;
                        
                _drawTeams.Add(player.Role.Template.WinningTeam);
                player.SetWinStatus(WinStatus.Tied);
                        
            }
        }
        
        private void ResetGame(){
            _gameService.MessageService.ResetMessages();

            var multiDeviceGameService = _gameService as OnlineGameService;
            multiDeviceGameService?.TurnTimerService.StopTimer();
        }

        private List<Player> FindLastLivingPlayers(){
            if(_gameService.AlivePlayers.Count != 0){
                return _gameService.GetAlivePlayersAsPlayerList();
            }
            TimePeriod timePeriod = FindLastKilledTime();
            List<Player> players = new List<Player>();

            foreach(var pair in _gameService.AllPlayers){
                Player player = pair.Value;
                if(player.DeathProperties.DeathTimePeriod.Equals(timePeriod)){
                    players.Add(player);
                }
            }
            return players;
        }

        private TimePeriod FindLastKilledTime(){
            TimePeriod timePeriod = TimePeriod.Default();

            foreach(var pair in _gameService.AllPlayers){
                Player player = pair.Value;
                if(player.DeathProperties.DeathTimePeriod.IsAfter(timePeriod)){
                    timePeriod = player.DeathProperties.DeathTimePeriod;
                }
            }
            return timePeriod;
        }
        
        public WinningTeam GetHighestPriorityWinningTeam(){
            
            return _winningTeams.FirstOrDefault();
        }
        
        private void AddWinningTeam(WinningTeam winningTeam){
            _winningTeams.Add(winningTeam);
        }
    }
}