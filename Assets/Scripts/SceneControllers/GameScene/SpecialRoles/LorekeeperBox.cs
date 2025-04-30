using System.Collections.Generic;
using game.models.gamestate;
using Game.Models.Roles.Enums;
using game.models.roles.Templates;
using game.models.roles.Templates.NeutralRoles;
using game.Services;
using Managers;
using TMPro;
using UnityEngine;

namespace SceneControllers.GameScene.SpecialRoles
{
    public class LorekeeperBox : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI selectedText, currentGuessCountText, aimGuessCountText;
        [SerializeField] private TMP_Dropdown rolesDropdown;
        private List<RoleTemplate> _roles;
        private LoreKeeper _loreKeeper;
        private string _currentGuessTemplate, _aimGuessTemplate;
        private int _loreKeeperAimGuessCount;

        public void Initialize(LoreKeeper loreKeeper, RolePack rolePack, int loreKeeperAimGuessCount)
        {
            _loreKeeperAimGuessCount = loreKeeperAimGuessCount;
            selectedText.text = TextManager.GetEnumCategoryTranslation(RoleId.LoreKeeper, "guessed_role");
            _currentGuessTemplate = TextManager.GetEnumCategoryTranslation(RoleId.LoreKeeper, "current_true_guess_count");
            _aimGuessTemplate = TextManager.GetEnumCategoryTranslation(RoleId.LoreKeeper, "winning_true_guess_count");
            
            var options = new List<TMP_Dropdown.OptionData>();
            _roles = RoleCatalog.GetAllRoles(rolePack);
            foreach (var role in _roles)
            {
                options.Add(new TMP_Dropdown.OptionData(role.GetName()));
            }
            
            rolesDropdown.options = options;
            rolesDropdown.onValueChanged.AddListener(RoleSelected);
            
            UpdateBox(loreKeeper);
            
        }
        
        private void RoleSelected(int index)
        {
            _loreKeeper.GuessedRole = _roles[index];
        }
        
        public void UpdateBox(LoreKeeper loreKeeper)
        {
            _loreKeeper = loreKeeper;
            currentGuessCountText.text = string.Format(_currentGuessTemplate, _loreKeeper.TrueGuessCount);
            aimGuessCountText.text = string.Format(_aimGuessTemplate, _loreKeeperAimGuessCount);
        }
    }
}