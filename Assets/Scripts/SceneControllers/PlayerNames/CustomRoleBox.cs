using Game.Models.Roles.Enums;
using game.Services;
using Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SceneControllers.PlayerNames
{
    public class CustomRoleBox : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI roleNameText, countText;
        [SerializeField] private Button minusButton, plusButton;
        public RoleId RoleId { private set; get; }
        public int Count { get; private set; }
        
        public void Initialize(RoleId role)
        {
            RoleId = role;
            roleNameText.text = role.GetName();
            
            minusButton.onClick.RemoveAllListeners();
            plusButton.onClick.RemoveAllListeners();
            
            minusButton.onClick.AddListener(MinusClicked);
            plusButton.onClick.AddListener(PlusClicked);
        }

        private void PlusClicked()
        {
            var role = RoleCatalog.GetRole(RoleId);
            if (Count >= role.ChanceProperty.MaxNumber) return;
            Count++;
            UpdateCountText();
        }
        private void MinusClicked()
        {
            if (Count <= 0) return;
            Count--;
            UpdateCountText();
        }

        private void UpdateCountText()
        {
            countText.text = Count.ToString();
        }
    }
}