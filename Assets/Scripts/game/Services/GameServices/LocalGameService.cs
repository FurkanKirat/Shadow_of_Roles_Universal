using System.Collections.Generic;
using game.models.gamestate;
using game.models.player;
using game.models.player.properties;
using Networking.DataTransferObjects;

namespace game.Services.GameServices
{
    public class LocalGameService : BaseGameService
    {
        public Player CurrentPlayer { get; private set; }
        private int CurrentPlayerIndex { get; set; }
        
        public LocalGameService(List<Player> players, RolePack rolePack)  
            : base(players, new TimeService(), new GameSettings(GameMode.Local, rolePack, players.Count))
        {
            
        }
        
        public override void UpdateAlivePlayers(){

            base.UpdateAlivePlayers();
            if(AlivePlayers.Count != 0){
                MoveToFirstHumanPlayer();
            }

        }

        public override void ReceiveInfo(ClientInfoDto clientInfo)
        {
            base.ReceiveInfo(clientInfo);
            PassTurn();
        }

        /**
         * Passes to the turn to the next player
         * @return true if time state is changed
         */
        private bool PassTurn() {

            if(!DoesHumanPlayerExist()){
                while(!FinishGameService.IsGameFinished){
                    ToggleDayNightCycle();
                }

                return true;
            }

            var firstTurn = true;

            do {
                CurrentPlayerIndex = (CurrentPlayerIndex + 1) % AlivePlayers.Count;
                CurrentPlayer = GetPlayer(AlivePlayers[CurrentPlayerIndex]);

                if (CurrentPlayerIndex == FindFirstHumanPlayer()) {
                    ToggleDayNightCycle();
                    firstTurn = false;
                    break;
                }

            } while (CurrentPlayer.PlayerType == PlayerType.AI);
            return !firstTurn;
        }

        /**
         * Moves the turn to the first human player, skipping AI players.
         */
        private void MoveToFirstHumanPlayer() {

            for(int i=0; i<AlivePlayers.Count ; i++){
                Player player = GetPlayer(AlivePlayers[i]);
                if(player.PlayerType == PlayerType.Human){
                    CurrentPlayerIndex = i;
                    CurrentPlayer = player;
                    break;
                }
            }
        }

        private int FindFirstHumanPlayer() {

            for(int i=0; i<AlivePlayers.Count ;i++){
                Player player = GetPlayer(AlivePlayers[i]);
                if(player.PlayerType == PlayerType.Human){
                    return i;
                }
            }
            return -1;
        }
        
    }
}