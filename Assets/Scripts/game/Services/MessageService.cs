using System.Collections.Generic;
using System.Linq;
using game.models;
using game.models.gamestate;
using game.models.player;
using Game.Models.Roles.Enums;
using game.models.roles.properties;
using game.Services.GameServices;

namespace game.Services
{
    public class MessageService
    {
        private readonly BaseGameService _gameService;
        private readonly List<Message> _messages = new ();

        public MessageService(BaseGameService gameService)
        {
            _gameService = gameService;
        }

        public void ResetMessages()
        {
            _messages.Clear();
        }

        public void SendMessage(MessageTemplate messageTemplate, Player receiver, bool isPublic)
        {
            TimePeriod messageTimePeriod = _gameService.TimeService.TimePeriod.GetPrevious(_gameService.GameSettings.GameMode);

            int receiverNumber = receiver?.Number ?? -1;
            _messages.Add(new Message(messageTemplate, messageTimePeriod, receiverNumber, isPublic));
        }
        

        public void SendSpecificRoleMessages(Player alivePlayer)
        {
            if (alivePlayer.Role.Template.RoleProperties.HasAttribute(RoleAttribute.RoleBlockImmune)
                && !alivePlayer.Role.CanPerform
                && !alivePlayer.Role.IsImmune)
            {
                var template = new MessageTemplate
                {
                    MessageKey = "role_block.rb_immune_message",
                    PlaceHolders = null
                };
                SendMessage(template, alivePlayer, false);
            }

            Player chosenPlayer = _gameService.GetPlayer(alivePlayer.Role.ChosenPlayer);
            if (chosenPlayer == null)
            {
                return;
            }
            
            if (chosenPlayer.Role.IsImmune &&
                alivePlayer.Role.Template.RolePriority <= RolePriority.RoleBlock
                && !alivePlayer.Role.Template.RoleProperties.HasAttribute(RoleAttribute.HasImmuneAbility))
            {
                var template = new MessageTemplate
                {
                    MessageKey = "role_block.immune_message",
                    PlaceHolders = null
                };
                SendMessage(template, alivePlayer, false);
            }
        }

        public List<Message> GetPlayerMessages(Player player)
        {
            return GetPlayerMessages(player.Number);
        }
        
        public List<Message> GetPlayerMessages(int number)
        {
            return _messages
                .Where(message => message.IsPublic || message.ReceiverNumber == number)
                .ToList();
        }

        public List<Message> GetDailyAnnouncements()
        {
            return GetDailyAnnouncements(_messages, _gameService.TimeService.TimePeriod, _gameService.GameSettings.GameMode);
        }

        public static List<Message> GetDailyAnnouncements(List<Message> messages, TimePeriod currentPeriod, GameMode gameMode)
        {

            var timePeriod = currentPeriod.GetPrevious(gameMode);
            return messages
                .Where(m => m.IsPublic && m.TimePeriod == timePeriod)
                .ToList();
        }

        public TimePeriod GetLastMessagePeriod(Player player)
        {
            return GetPlayerMessages(player)
                .Where(message => message.IsPublic || player.IsSamePlayer(message.ReceiverNumber))
                .OrderByDescending(message => message.TimePeriod)
                .Select(message => message.TimePeriod.Clone() as TimePeriod)
                .FirstOrDefault() ?? TimePeriod.Default();
        }

        public void SendPrivateMessage(MessageTemplate template, Player receiver)
        {
            SendMessage(template, receiver, false);
        }

        public void SendPublicMessage(MessageTemplate template)
        {
            SendMessage(template,null, true);
        }
    }
}
