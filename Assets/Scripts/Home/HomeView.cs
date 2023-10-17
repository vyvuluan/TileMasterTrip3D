using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Home
{
    public class HomeView : MonoBehaviour
    {
        [SerializeField] private GameObject popUpSetting;
        [SerializeField] private TextMeshProUGUI levelText;
        [SerializeField] private TextMeshProUGUI coinText;


        [SerializeField] private Slider sliderSound;
        [SerializeField] private Slider sliderMusic;
        public void SetLevelText(int level) => levelText.text = $"Level {level}";
        public void SetCoinText(int coin) => coinText.text = coin.ToString();
        public void SetSliderSound(float value) => sliderSound.value = value;
        public void SetSliderMusic(float value) => sliderMusic.value = value;
        public float GetValueSliderSound() => sliderSound.value;
        public float GetValueSliderMusic() => sliderMusic.value;
        public void SetStatusActiveSetting(bool status) => popUpSetting.SetActive(status);
    }
}

