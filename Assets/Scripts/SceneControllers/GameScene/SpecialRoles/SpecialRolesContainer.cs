using game.models;
using Game.Models.Roles.Enums;
using game.models.roles.Templates;
using game.models.roles.Templates.FolkRoles;
using game.models.roles.Templates.NeutralRoles;
using UnityEngine;

namespace SceneControllers.GameScene.SpecialRoles
{
    public class SpecialRolesContainer : MonoBehaviour
    {
        [SerializeField] private AbilityCooldownBox abilityCooldownBox;
        [SerializeField] private EntrepreneurBox entrepreneurBox;
        [SerializeField] private LorekeeperBox lorekeeperBox;
        [SerializeField] private GameObject parent;

        public void InitializePanel(IDataProvider dataProvider)
        {
            foreach (var player in dataProvider.GetAllPlayers())
            {
                if (player.Role.Template.RoleID == RoleId.LoreKeeper)
                {
                    lorekeeperBox.Initialize(player.Role.Template as LoreKeeper, 
                        dataProvider.GetGameSettings().RolePack, 6);
                    break;
                }
            }
        }
        
        public void ShowPanel(RoleTemplate role)
        {
            parent.SetActive(true);
            HideAllBoxes();

            switch (role.RoleID)
            {
                case RoleId.FolkHero:
                    abilityCooldownBox.UpdateBox(role.RoleProperties.Cooldown.Current);
                    abilityCooldownBox.gameObject.SetActive(true);
                    break;

                case RoleId.Entrepreneur:
                    entrepreneurBox.UpdateBox(role as Entrepreneur);
                    entrepreneurBox.gameObject.SetActive(true);
                    break;

                case RoleId.LoreKeeper:
                    lorekeeperBox.UpdateBox(role as LoreKeeper);
                    lorekeeperBox.gameObject.SetActive(true);
                    break;

                default:
                    parent.SetActive(false);
                    break;
            }
        }

        private void HideAllBoxes()
        {
            abilityCooldownBox.gameObject.SetActive(false);
            entrepreneurBox.gameObject.SetActive(false);
            lorekeeperBox.gameObject.SetActive(false);
        }

    }
}