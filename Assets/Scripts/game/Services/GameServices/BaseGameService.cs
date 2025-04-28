using System;
using System.Collections.Generic;
using game.models.gamestate;
using game.models.player;
using game.models.player.properties;
using game.models.roles.properties;
using game.models.roles.Templates.CorruptedRoles;

namespace game.Services.GameServices
{
    public abstract class BaseGameService
    {
        public List<Player> AllPlayers { get; } = new ();
        public List<Player> AlivePlayers { get; } = new ();
        public List<Player> DeadPlayers { get; } = new ();
        public VotingService VotingService { get; }
        public BaseTimeService TimeService { get; }
        public MessageService MessageService { get; }
        public FinishGameService FinishGameService { get; }
        public AbilityService AbilityService { get; }
        public GameSettings GameSettings { get; }

        protected BaseGameService(List<Player> players, BaseTimeService timeService, GameSettings gameSettings)
        {
            TimeService = timeService;
            VotingService = new VotingService(this);
            MessageService = new MessageService(this);
            FinishGameService = new FinishGameService(this);
            AbilityService = new AbilityService(this);
            GameSettings = gameSettings;

            InitializePlayers(players);
        }


        /**
         * Initializes the players and distributes their roles
         * @param players players' list
         */
        private void InitializePlayers(List<Player> players)
        {

            AllPlayers.AddRange(players);
            UpdateAlivePlayers();

        }

        public void ToggleDayNightCycle()
        {
            TimeService.ToggleTimeCycle();
            Time time = TimeService.TimePeriod.Time;
            switch (time)
            {
                case Time.Day:
                    AbilityService.PerformAllAbilities();
                    break;
                case Time.Night:
                    VotingService.ExecuteMaxVoted();
                    break;
                
                case Time.Voting:
                    // No operation during voting time
                    break;
                default:
                    throw new InvalidOperationException("Unknown time phase.");
            }
            
            FinishGameService.FinishGame();
        }

        public virtual void UpdateAlivePlayers()
        {
            AlivePlayers.Clear();
            foreach (var player in AllPlayers)
            {

                if (player.DeathProperties.IsAlive)
                {
                    AlivePlayers.Add(player);
                }

                else
                {

                    /* If players role is last joke, player is dead and player has not used ability
                     * yet adds the player to the alive players to use their ability */
                    if (!player.Role.Template.RoleProperties.HasAttribute(RoleAttribute.HasPostDeathEffect))
                    {
                        continue;
                    }
                    
                    LastJoke lastJoker = (LastJoke)player.Role.Template;
                    if (lastJoker.CanUseAbility() && TimeService.TimePeriod.Time == Time.Night)
                    {
                        AlivePlayers.Add(player);
                    }

                }

            }

        }


        /**
         * Updates the dead players
         * @return updated dead players
         */
        public List<Player> GetDeadPlayers()
        {
            foreach (var player in AllPlayers)
            {
                if (!player.DeathProperties.IsAlive && !DeadPlayers.Contains(player))
                {
                    DeadPlayers.Add(player);
                }
            }

            return DeadPlayers;
        }

        public void ChooseRandomPlayersForAI(List<Player> players)
        {
            foreach (var player in players)
            {
                if (player.PlayerType == PlayerType.AI)
                {
                    player.Brain.ChooseNightPlayer(player, AlivePlayers);
                }
            }
        }

        protected bool DoesHumanPlayerExist()
        {
            foreach (var alivePlayer in AlivePlayers)
            {
                if (alivePlayer.PlayerType == PlayerType.Human)
                {
                    return true;
                }
            }

            return false;

        }

        public List<Player> CopyAlivePlayers()
        {
            return new List<Player>(AlivePlayers);
        }

    }
}