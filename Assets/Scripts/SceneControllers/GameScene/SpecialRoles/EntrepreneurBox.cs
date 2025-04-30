using System.Collections.Generic;
using System.Linq;
using Game.Models.Roles.Enums;
using game.models.roles.Templates.FolkRoles;
using Managers;
using TMPro;
using UnityEngine;

namespace SceneControllers.GameScene.SpecialRoles
{
    public class EntrepreneurBox : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI moneyText, selectAbilityText;
        [SerializeField] private TMP_Dropdown abilityDropdown;
        private Entrepreneur _entrepreneur;

        public void Start()
        {
            selectAbilityText.text = TextManager.GetEnumCategoryTranslation(RoleId.Entrepreneur, "selected");

            var abilities = Entrepreneur.ChosenAbility.Values;
            var options = new List<TMP_Dropdown.OptionData>();

            foreach (var ability in abilities)
            {
                options.Add(
                    new TMP_Dropdown.OptionData(
                        TextManager.GetEnumCategoryTranslation(RoleId.Entrepreneur, ability.Name)));
            }
            abilityDropdown.options = options;
        }

        public void UpdateBox(Entrepreneur entrepreneur)
        {
            _entrepreneur = entrepreneur;
            moneyText.text = string.Format(
                TextManager.GetEnumCategoryTranslation(RoleId.Entrepreneur, "current_money"), 
                entrepreneur.RoleProperties.Money);
            abilityDropdown.onValueChanged.RemoveAllListeners();
            abilityDropdown.onValueChanged.AddListener(AbilitySelected);
            abilityDropdown.value = Entrepreneur.ChosenAbility.GetIndex(entrepreneur.TargetAbility);
        }

        private void AbilitySelected(int index)
        {
            var ability = Entrepreneur.ChosenAbility.Values.ElementAt(index);
            _entrepreneur.TargetAbility = ability;
        }
    }
}