using System.Collections.Generic;
using System.Linq;
using game.models.Settings;
using Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SceneControllers
{
    public class SettingsController : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI languageText, playerNameText, versionText;
        [SerializeField] private TMP_Dropdown languageDropdown;
        [SerializeField] private TMP_InputField playerNameInput;
        [SerializeField] private Button closeButton;
        private SettingsManager _settingsManager;
        private SceneChanger _sceneChanger;
        private void Start()
        {
            _settingsManager = ServiceLocator.Get<SettingsManager>();
            _sceneChanger = ServiceLocator.Get<SceneChanger>();
            InitLanguageSettings();
            closeButton.onClick.AddListener(SaveSettings);

            versionText.text = _settingsManager.MetaData.version;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                _sceneChanger.GoBack();
            }
        }

        private void InitLanguageSettings()
        {
            var options = new List<TMP_Dropdown.OptionData>();
            foreach(var option in Language.Values())
            {
                options.Add(new TMP_Dropdown.OptionData(option.ToString()));
            }
            languageDropdown.options = options;
            languageDropdown.value = 0;
            languageText.text = TextManager.Translate("settings.language");
            InitSelectedLanguage();
            languageDropdown.onValueChanged.AddListener((value) => SelectLanguage(value));
            
            playerNameText.text = TextManager.Translate("settings.username");
            playerNameInput.text = _settingsManager.UserSettings.Username;
        }

        private void InitSelectedLanguage()
        {
            var selectedLanguage = _settingsManager.UserSettings.Language;
            int index = languageDropdown.options.FindIndex(option => option.text == selectedLanguage.Text);
            languageDropdown.value = index;
        }

        private void SelectLanguage(int value)
        {
            string selectedLanguage = languageDropdown.options[value].text;
            var language = Language.Values().FirstOrDefault(x => x.Text == selectedLanguage);
            _settingsManager.ChangeLanguage(language);
            _sceneChanger.RefreshScene();
        }

        private void SaveSettings()
        {
            _settingsManager.UserSettings.Username = playerNameInput.text;
            _settingsManager.SaveSettings();
        }
    
    }
}

