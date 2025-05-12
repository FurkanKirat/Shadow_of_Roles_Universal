using TMPro;
using UnityEngine;
namespace UI
{
    public class PhaseCountdownUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI countdownText;
        private float remainingTime;
        private bool isCounting;

        public void StartCountdown(float seconds)
        {
            remainingTime = seconds;
            isCounting = true;
        }

        void Update()
        {
            if (!isCounting) return;

            remainingTime -= Time.deltaTime;

            if (remainingTime <= 0)
            {
                isCounting = false;
                countdownText.text = "00:00";
            }
            else
            {
                countdownText.text = FormatTime(remainingTime);
            }
        }

        private string FormatTime(float time)
        {
            int minutes = Mathf.FloorToInt(time / 60f);
            int seconds = Mathf.FloorToInt(time % 60f);
            return $"{minutes:D2}:{seconds:D2}";
        }
    }

}