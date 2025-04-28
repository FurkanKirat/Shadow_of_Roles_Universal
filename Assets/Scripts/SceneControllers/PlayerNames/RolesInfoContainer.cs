using System.Collections.Generic;
using game.Services.RoleDistributor.Hints;
using TMPro;
using UnityEngine;

namespace SceneControllers.PlayerNames
{
    public class RolesInfoContainer : MonoBehaviour
    {
        [SerializeField] private GameObject prefab;
        [SerializeField] private Transform parent;
        private List<IRoleHint> _hints;
        

        public void ChangeInfo(List<IRoleHint> hints)
        {
            _hints = hints;
            
            foreach (Transform child in parent) 
                Destroy(child.gameObject);

            foreach (IRoleHint hint in _hints)
            {
                var newObject = Instantiate(prefab, parent);
                var boxText = newObject.GetComponentInChildren<TextMeshProUGUI>();
                boxText.text = hint.Describe();
            }
        }
    }
}