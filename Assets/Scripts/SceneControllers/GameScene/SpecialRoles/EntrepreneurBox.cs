using System.Collections.Generic;
using System.Linq;
using Game.Models.Roles.Enums;
using game.models.roles.Templates.FolkRoles;
using Managers;
using Networking.DataTransferObjects;
using Networking.DataTransferObjects.RoleViewStrategies;
using Networking.Interfaces;
using TMPro;
using UnityEngine;

namespace SceneControllers.GameScene.SpecialRoles
{
    public class EntrepreneurBox : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI moneyText, selectAbilityText;
        [SerializeField] private TMP_Dropdown abilityDropdown;

        public void Start()
        {
            selectAbilityText.text = TextManager.TranslateEnum(RoleId.Entrepreneur, "selected");

            var abilities = Entrepreneur.ChosenAbility.Values;
            var options = new List<TMP_Dropdown.OptionData>();

            foreach (var ability in abilities)
            {
                options.Add(
                    new TMP_Dropdown.OptionData(
                        TextManager.TranslateEnum(RoleId.Entrepreneur, ability.Name)));
            }
            abilityDropdown.options = options;
        }

        public void UpdateBox(RoleDto roleDto)
        {
            moneyText.text = string.Format(
                TextManager.TranslateEnum(RoleId.Entrepreneur, "current_money"), roleDto.Money
                );
            abilityDropdown.onValueChanged.RemoveAllListeners();
            abilityDropdown.onValueChanged.AddListener(AbilitySelected);
            abilityDropdown.value 
                = Entrepreneur.ChosenAbility.GetIndex((Entrepreneur.ChosenAbility)roleDto.ExtraData[ExtraData.EntrepreneurTargetAbility]);
        }

        private void AbilitySelected(int index)
        {
            var ability = Entrepreneur.ChosenAbility.Values.ElementAt(index);
            ServiceLocator.Get<IClient>().GetCurrentClientInfo().ExtraData[ExtraData.EntrepreneurTargetAbility] = ability;
        }
    }
}