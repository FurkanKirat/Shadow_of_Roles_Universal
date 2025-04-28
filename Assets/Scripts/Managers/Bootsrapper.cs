
using UI;
using UnityEngine;

namespace Managers
{
    public class Bootsrapper : MonoBehaviour
    {
        [SerializeField] private SceneChanger sceneChanger;
        [SerializeField] private AlphaThresholdManager alphaThresholdManager;
        private void Awake()
        {
            ServiceLocator.Register(new FilePathManager());
            ServiceLocator.Register(new LanguageManager());
            ServiceLocator.Register(new SettingsManager());
            ServiceLocator.Register(sceneChanger);
            ServiceLocator.Register(alphaThresholdManager);
        }
        
    }

}
