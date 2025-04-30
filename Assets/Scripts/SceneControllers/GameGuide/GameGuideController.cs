using System;
using Managers;
using UnityEngine;
using UnityEngine.UI;

namespace SceneControllers.GameGuide
{
    public class GameGuideController : MonoBehaviour
    {

        [SerializeField] private Button rulesButton, rolesButton;
     
        private GameRulesController _rulesController;
        private RolesController _rolesController;
        private void Start()
        {
            _rulesController = GetComponentInChildren<GameRulesController>();
            _rolesController = GetComponentInChildren<RolesController>();
            
            rolesButton.onClick.AddListener(RolesClicked);
            rulesButton.onClick.AddListener(GameRulesClicked);
        }
        
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                gameObject.SetActive(false);
            }
        }

        private void RolesClicked()
        {
            _rulesController.gameObject.SetActive(false);
            _rolesController.gameObject.SetActive(true);
        }

        private void GameRulesClicked()
        {
            _rulesController.gameObject.SetActive(true);
            _rolesController.gameObject.SetActive(false);
        }
    }
}