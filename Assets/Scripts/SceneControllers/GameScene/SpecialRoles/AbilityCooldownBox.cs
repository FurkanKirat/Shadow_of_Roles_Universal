using Managers;
using TMPro;
using UnityEngine;

namespace SceneControllers.GameScene.SpecialRoles
{
    public class AbilityCooldownBox : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI abilityCooldownText;
        
        public void UpdateBox(int cooldown)
        {
            abilityCooldownText.text = string.Format(
                TextManager.Translate("abilities.cooldown"), cooldown);
        }
    }
}