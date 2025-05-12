using System.Collections.Generic;
using System.Linq;
using game.models.gamestate;
using game.models.player;
using game.models.player.properties;
using Game.Models.Roles.Enums;
using game.models.roles.properties;
using game.models.roles.Templates.CorruptedRoles;
using game.models.roles.Templates.FolkRoles;
using game.models.roles.Templates.NeutralRoles;
using Networking.DataTransferObjects;
using Networking.DataTransferObjects.RoleViewStrategies;
using UnityEngine;
using Time = game.models.gamestate.Time;

namespace game.Services.GameServices
{
    public abstract class BaseGameService
    {
        public SortedDictionary<int,Player> AllPlayers { get; private set; } = new ();
        public List<int> AlivePlayers { get; } = new ();
        public VotingService VotingService { get; }
        public TimeService TimeService { get; }
        public MessageService MessageService { get; }
        public FinishGameService FinishGameService { get; }
        public AbilityService AbilityService { get; }
        public GameSettings GameSettings { get; }
        public DtoProvider DtoProvider { get; }

        protected BaseGameService(List<Player> players, GameSettings gameSettings)
        {
            TimeService = new TimeService(this);
            VotingService = new VotingService(this);
            MessageService = new MessageService(this);
            FinishGameService = new FinishGameService(this);
            AbilityService = new AbilityService(this);
            GameSettings = gameSettings;
            DtoProvider = new DtoProvider(this);
            InitializePlayers(players);
        }
        
        /**
         * Initializes the players and distributes their roles
         * @param players players' list
         */
        private void InitializePlayers(List<Player> players)
        {

            AllPlayers = new SortedDictionary<int, Player>(
                players.ToDictionary(player => player.Number, player => player)
            );
            UpdateAlivePlayers();
            
        }

        public abstract void ToggleDayNightCycle();

        public virtual void UpdateAlivePlayers()
        {
            AlivePlayers.Clear();
            foreach (var (num, player) in AllPlayers)
            {
                if (player.DeathProperties.IsAlive)
                {
                    AlivePlayers.Add(num);
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
                        AlivePlayers.Add(num);
                    }

                }

            }

        }

        public Player GetPlayer(int number)
        {
            if(number <= 0) return null;
            return AllPlayers[number];
        }

        /**
         * Updates the dead players
         * @return updated dead players
         */
        public List<Player> GetDeadPlayers()
        {
            List<Player> deadPlayersList = new();
            foreach (var (_,player) in AllPlayers)
            {
                if (!player.DeathProperties.IsAlive)
                {
                    deadPlayersList.Add(player);
                }
            }

            return deadPlayersList;
        }

        public void ChooseRandomPlayersForAI(List<Player> alivePlayers)
        {
            foreach (var player in alivePlayers)
            {
                if (player.PlayerType == PlayerType.AI)
                {
                    player.Brain.ChooseNightPlayer(player, alivePlayers);
                }
            }
        }

        protected bool DoesHumanPlayerExist()
        {
            foreach (var playerNum in AlivePlayers)
            {
                if (GetPlayer(playerNum).PlayerType == PlayerType.Human)
                {
                    return true;
                }
            }

            return false;

        }

        public List<int> CopyAlivePlayers()
        {
            return new List<int>(AlivePlayers);
        }

        public List<Player> GetAlivePlayersAsPlayerList()
        {
            return AllPlayers
                .Values
                .Where(player => AlivePlayers.Contains(player.Number))
                .ToList();
        }

        public virtual void ReceiveInfo(ClientInfoDto clientInfo)
        {
            Player player = GetPlayer(clientInfo.Number);
            player.Role.ChosenPlayer = clientInfo.TargetNumber;

            switch (player.Role.Template.RoleID)
            {
                case RoleId.Entrepreneur:
                    if (player.Role.Template is Entrepreneur entrepreneur)
                    {
                        if (clientInfo.ExtraData.TryGetValue(ExtraData.EntrepreneurTargetAbility, out var abilityObj) && abilityObj is Entrepreneur.ChosenAbility ability)
                        {
                            entrepreneur.TargetAbility = ability;
                        }
                    }
                    break;

                case RoleId.LoreKeeper:
                    if (player.Role.Template is LoreKeeper loreKeeper)
                    {
                        Debug.Log("1");
                        if (clientInfo.ExtraData.TryGetValue(ExtraData.LoreKeeperGuessedRole, out var roleObj) && roleObj is RoleId roleId)
                        {
                            loreKeeper.GuessedRole = roleId;
                            Debug.Log(2);
                        }
                    }
                    break;
            }
        }

    }
}