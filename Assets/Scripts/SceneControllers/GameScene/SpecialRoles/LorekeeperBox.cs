using System.Collections.Generic;
using System.Linq;
using game.models.gamestate;
using Game.Models.Roles.Enums;
using game.Services;
using Managers;
using Networking.DataTransferObjects;
using Networking.DataTransferObjects.RoleViewStrategies;
using Networking.Interfaces;
using TMPro;
using UnityEngine;

namespace SceneControllers.GameScene.SpecialRoles
{
    public class LorekeeperBox : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI selectedText, currentGuessCountText, aimGuessCountText;
        [SerializeField] private TMP_Dropdown rolesDropdown;
        private List<RoleId> _roleIds;
        private RoleDto _roleDto;
        private string _currentGuessTemplate, _aimGuessTemplate;

        public void Initialize(RolePack rolePack)
        {
            selectedText.text = TextManager.TranslateEnum(RoleId.LoreKeeper, "guessed_role");
            _currentGuessTemplate = TextManager.TranslateEnum(RoleId.LoreKeeper, "current_true_guess_count");
            _aimGuessTemplate = TextManager.TranslateEnum(RoleId.LoreKeeper, "winning_true_guess_count");
            
            var roles = RoleCatalog.GetAllRoles(rolePack);
            _roleIds = new List<RoleId> { RoleId.None };
            _roleIds.AddRange(roles.Select(r => r.RoleID));
            
            var options = new List<TMP_Dropdown.OptionData>
            {
                new (TextManager.TranslateEnum(RoleId.None, "name"))
            };
            options.AddRange(roles.Select(r => new TMP_Dropdown.OptionData(r.GetName())));
            
            rolesDropdown.options = options;
            rolesDropdown.onValueChanged.AddListener(RoleSelected);
            
        }
        
        public void ResetBox()
        {
            rolesDropdown.value = 0;
            ServiceLocator.Get<IClient>().GetCurrentClientInfo().ExtraData[ExtraData.LoreKeeperGuessedRole] = RoleId.None;
        }

        private void RoleSelected(int index)
        {
            ServiceLocator.Get<IClient>().GetCurrentClientInfo().ExtraData[ExtraData.LoreKeeperGuessedRole] = _roleIds[index];
        }
        
        public void UpdateBox(RoleDto roleDto)
        {
            _roleDto = roleDto;
            
            if (!_roleDto.ExtraData.TryGetValue(ExtraData.LoreKeeperGuessedRole, out object guessedObj) || guessedObj is not RoleId guessedRole)
            {
                guessedRole = RoleId.None;
            }
            int index = _roleIds.IndexOf(guessedRole);
            if (index >= 0) rolesDropdown.value = index;
            
            var currentGuess = _roleDto.ExtraData[ExtraData.LoreKeeperCurrentGuess];
            currentGuessCountText.text = string.Format(_currentGuessTemplate, currentGuess);
            aimGuessCountText.text = string.Format(_aimGuessTemplate, _roleDto.ExtraData[ExtraData.LoreKeeperTargetGuess]);
        }
    }
}