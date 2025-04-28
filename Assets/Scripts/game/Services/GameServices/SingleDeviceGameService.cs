using System.Collections.Generic;
using game.models;
using game.models.gamestate;
using game.models.player;
using game.models.player.properties;

namespace game.Services.GameServices
{
    public class SingleDeviceGameService : BaseGameService , IDataProvider
    {
        private Player CurrentPlayer { get; set; }
        private int CurrentPlayerIndex { get; set; }

        public SingleDeviceGameService(List<Player> players, RolePack rolePack)  
            : base(players, new BaseTimeService(), new GameSettings(GameMode.Offline, rolePack, players.Count)){
        }

        
        public override void UpdateAlivePlayers(){

            base.UpdateAlivePlayers();
            if(AlivePlayers.Count != 0){
                MoveToFirstHumanPlayer();
            }

        }

        /**
         * Passes to the turn to the next player
         * @return true if time state is changed
         */
        public bool PassTurn() {

            if(!DoesHumanPlayerExist()){
                while(!FinishGameService.IsGameFinished){
                    ToggleDayNightCycle();
                }

                return true;
            }

            var firstTurn = true;

            do {
                CurrentPlayerIndex = (CurrentPlayerIndex + 1) % AlivePlayers.Count;
                CurrentPlayer = AlivePlayers[CurrentPlayerIndex];

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
                if(AlivePlayers[i].PlayerType == PlayerType.Human){
                    CurrentPlayerIndex = i;
                    CurrentPlayer = AlivePlayers[CurrentPlayerIndex];
                    break;
                }
            }
        }

        private int FindFirstHumanPlayer() {

            for(int i=0; i<AlivePlayers.Count ;i++){
                if(AlivePlayers[i].PlayerType == PlayerType.Human){
                    return i;
                }
            }
            return -1;
        }

        
        public Dictionary<TimePeriod, List<Message>> GetMessages() {
            return MessageService.GetPlayerMessages(GetCurrentPlayer());
        }

        public TimePeriod GetLastMessagePeriod()
        {
            return MessageService.GetLastMessagePeriod(CurrentPlayer);
        }

        // Getters
        public Player GetCurrentPlayer(){
            return CurrentPlayer;
        }

        public List<Player> GetAlivePlayers()
        {
            return AlivePlayers;
        }

        public List<Player> GetAllPlayers()
        {
            return AllPlayers;
        }

        public TimePeriod GetTimePeriod()
        {
            return TimeService.TimePeriod;
        }
        
        public GameSettings GetGameSettings()
        {
            return GameSettings;
        }

        public GameStatus GetGameStatus()
        {
            return FinishGameService.IsGameFinished ? GameStatus.Ended : GameStatus.Continues;
        }
    }
}