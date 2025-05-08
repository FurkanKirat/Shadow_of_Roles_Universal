using game.models;
using Game.Models.Roles.Enums;
using Networking.DataTransferObjects;
using UnityEngine;
using Time = game.models.gamestate.Time;
namespace SceneControllers.GameScene.SpecialRoles
{
    public class SpecialRolesContainer : MonoBehaviour
    {
        [SerializeField] private AbilityCooldownBox abilityCooldownBox;
        [SerializeField] private EntrepreneurBox entrepreneurBox;
        [SerializeField] private LorekeeperBox lorekeeperBox;
        [SerializeField] private GameObject parent;

        public void InitializePanel(IGameInformation gameInformation)
        {
            lorekeeperBox.Initialize(gameInformation.GameSettings.RolePack);
        }
        
        public void ShowPanel(RoleDto roleDto, Time time)
        {
            parent.SetActive(true);
            HideAllBoxes();

            if (time != Time.Night)
            {
                parent.SetActive(false);
                return;
            }
            
            switch (roleDto.RoleId)
            {
                case RoleId.FolkHero:
                    abilityCooldownBox.UpdateBox(roleDto.Cooldown);
                    abilityCooldownBox.gameObject.SetActive(true);
                    break;

                case RoleId.Entrepreneur:
                    entrepreneurBox.UpdateBox(roleDto);
                    entrepreneurBox.gameObject.SetActive(true);
                    break;

                case RoleId.LoreKeeper:
                    lorekeeperBox.UpdateBox(roleDto);
                    lorekeeperBox.gameObject.SetActive(true);
                    break;

                default:
                    parent.SetActive(false);
                    break;
            }
        }

        public void ResetBoxes()
        {
            lorekeeperBox.ResetBox();
        }

        private void HideAllBoxes()
        {
            abilityCooldownBox.gameObject.SetActive(false);
            entrepreneurBox.gameObject.SetActive(false);
            lorekeeperBox.gameObject.SetActive(false);
        }

    }
}