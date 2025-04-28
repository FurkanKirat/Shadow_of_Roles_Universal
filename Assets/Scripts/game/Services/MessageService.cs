using System.Collections.Generic;
using System.Linq;
using game.Constants;
using game.models;
using game.models.gamestate;
using game.models.player;
using Game.Models.Roles.Enums;
using game.models.roles.properties;
using game.Services.GameServices;
using Managers;
using UnityEngine;
using Time = game.models.gamestate.Time;

namespace game.Services
{
    public class MessageService
    {
        private readonly BaseGameService _gameService;
        private readonly Dictionary<TimePeriod, List<Message>> _messages = new ();

        public MessageService(BaseGameService gameService)
        {
            _gameService = gameService;
        }

        public void ResetMessages()
        {
            _messages.Clear();
        }

        public void SendMessage(string message, Player receiver, bool isPublic)
        {
            TimePeriod messageTimePeriod = _gameService.TimeService.TimePeriod.GetPrevious();
            if (!_messages.ContainsKey(messageTimePeriod))
            {
                _messages[messageTimePeriod] = new List<Message>();
            }

            _messages[messageTimePeriod].Add(new Message(messageTimePeriod, message, receiver, isPublic));
        }
        

        public void SendSpecificRoleMessages(Player alivePlayer)
        {
            const TextCategory category = TextCategory.RoleBlock;
            if (alivePlayer.Role.Template.RoleProperties.HasAttribute(RoleAttribute.RoleBlockImmune)
                && !alivePlayer.Role.CanPerform
                && !alivePlayer.Role.IsImmune)
            {
                SendMessage(category.GetTranslation("rb_immune_message"), alivePlayer, false);
            }

            if (alivePlayer.Role.ChosenPlayer == null)
            {
                return;
            }

            if (alivePlayer.Role.ChosenPlayer.Role.IsImmune &&
                alivePlayer.Role.Template.RolePriority <= RolePriority.RoleBlock
                && !alivePlayer.Role.Template.RoleProperties.HasAttribute(RoleAttribute.HasImmuneAbility))
            {
                SendMessage(category.GetTranslation("immune_message"), alivePlayer, false);
            }
        }

        public Dictionary<TimePeriod, List<Message>> GetPlayerMessages(Player player)
        {
            var sendMap = new Dictionary<TimePeriod, List<Message>>();

            foreach (var entry in _messages)
            {
                var filteredMessages = entry.Value
                    .Where(message => message.IsPublic || message.Receiver.Number == player.Number)
                    .ToList();

                sendMap[entry.Key] = filteredMessages;
            }

            return sendMap;
        }

        public Dictionary<TimePeriod, List<Message>> GetDailyAnnouncements()
        {
            return GetDailyAnnouncements(_messages, _gameService.TimeService.TimePeriod);
        }

        public static Dictionary<TimePeriod, List<Message>> GetDailyAnnouncements(Dictionary<TimePeriod, List<Message>> messages, TimePeriod currentPeriod)
        {
            var sendMap = new Dictionary<TimePeriod, List<Message>>();

            var timePeriod = currentPeriod.GetPrevious();
            var messageList = messages.ContainsKey(timePeriod) ? messages[timePeriod]
                .Where(m => m.IsPublic)
                .ToList() : new List<Message>();

            sendMap[timePeriod] = messageList;

            return sendMap;
        }

        public TimePeriod GetLastMessagePeriod(Player player)
        {
            return GetPlayerMessages(player)
                .Where(pair => pair.Key != null && pair.Value.Count > 0)
                .OrderByDescending(pair => pair.Key)
                .Select(pair => pair.Key.Clone() as TimePeriod)
                .FirstOrDefault() ?? TimePeriod.Default();
        }

        public void SendAbilityMessage(string message, Player receiver)
        {
            SendMessage(message, receiver, false);
        }

        public void SendAbilityAnnouncement(string message)
        {
            SendMessage(message, null, true);
        }
    }
}
