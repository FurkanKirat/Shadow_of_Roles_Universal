﻿using System.Collections.Generic;
using game.models;
using game.models.player;
using game.models.player.properties;
using game.Services.GameServices;
using game.Utils;

namespace game.Services
{
    public class VotingService
    {
        private readonly BaseGameService _gameService;
        private readonly Dictionary<Player,Player> _votes = new ();
        private Player _maxVoted;
        private int _maxVote;

        public VotingService(BaseGameService gameService) {
            _gameService = gameService;
        }

        /**
         * Casts a vote from the voter player to the voted player
         * @param voter voter player
         * @param voted voted player
         */
        private void Vote(Player voter, Player voted){
            _votes.Add(voter,voted);
        }

        /**
         *
         * @param player the desired player
         * @return player's vote count
         */
        public int GetVoteCount(Player player){
            int count = 0;
            foreach(var votedPlayer in _votes.Values){
                if(votedPlayer.IsSamePlayer(player)){
                    count++;
                }
            }
            return count;
        }

        /**
         * Updates the max voted player
         */
        private void UpdateMaxVoted(){

            var voteCounts = new Dictionary<Player, int>();

            foreach(var entry in _votes){
                int voteCount = entry.Key.Role.Template.RoleProperties.VoteCount.Current;
                Player votedPlayer = entry.Value;
                if (votedPlayer != null)
                {
                    voteCounts[votedPlayer] = voteCounts.GetValueOrDefault(votedPlayer) + voteCount;
                }
            }

            foreach(var entry in voteCounts){
                if(entry.Value > _maxVote){
                    _maxVoted = entry.Key;
                    _maxVote = entry.Value;
                }
            }

        }

        /**
         * After the day voting, executes the max voted player if they get more than half of the votes
         */
        public void ExecuteMaxVoted(){
            
            var alivePlayers = _gameService.GetAlivePlayersAsPlayerList();

            foreach(var player in alivePlayers) {
                if(player.PlayerType == PlayerType.AI){
                    player.Brain.ChooseVotingPlayer(player, alivePlayers);

                }
                Vote(player,_gameService.GetPlayer(player.Role.ChosenPlayer));
            }

            UpdateMaxVoted();
            if(_maxVote > alivePlayers.Count/2){
                foreach(var alivePlayer in alivePlayers){
                    if(_maxVoted.IsSamePlayer(alivePlayer)){

                        alivePlayer
                            .KillPlayer(
                                _gameService.TimeService.TimePeriod, 
                                CauseOfDeath.Hanging,
                                _gameService.GameSettings.GameMode
                            );
                        break;
                    }
                }
                
                if(_maxVoted!=null)
                {
                    var template = new MessageTemplate
                    {
                        MessageKey = "voting.vote_execute",
                        PlaceHolders = new Dictionary<string, string>
                        {
                            { "playerName", _maxVoted.GetNameAndNumber() },
                            { "roleId", _maxVoted.Role.Template.RoleID.FormatEnum() }
                        }
                    };
                    
                    _gameService.MessageService.SendPublicMessage(template);
                }

            }
            _gameService.UpdateAlivePlayers();

            foreach(var player in alivePlayers){
                
                if(player.PlayerType == PlayerType.Human){
                    SendVoteMessage(player);
                }

                player.Role.ChosenPlayer = -1;
            }
            ClearVotes();
        }

        private void SendVoteMessage(Player player)
        {
            Player chosenPlayer = _gameService.GetPlayer(player.Role.ChosenPlayer);
            
            if(chosenPlayer!=null)
            {
                var template = new MessageTemplate
                {
                    MessageKey = "voting.voted_for",
                    PlaceHolders = new Dictionary<string, string>
                    {
                        { "playerName", chosenPlayer.GetNameAndNumber() }
                    }
                };
                _gameService.MessageService.SendMessage(template, player,false);
                
            }else{
                
                var template = new MessageTemplate
                {
                    MessageKey = "voting.voted_for_none",
                };
                _gameService.MessageService.SendPrivateMessage(template, player);
            }

        }

        /**
         * Clears the votes after day is finished
         */
        public void ClearVotes(){
            _votes.Clear();
            _maxVoted = null;
            _maxVote = 0;
        }
        
    }
}