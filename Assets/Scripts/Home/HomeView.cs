using TMPro;
using UnityEngine;

namespace Home
{
    public class HomeView : MonoBehaviour
    {
        [SerializeField] private GameObject popUpSetting;
        [SerializeField] private TextMeshProUGUI levelText;
        [SerializeField] private TextMeshProUGUI coinText;
        public void SetPopUpSetting(bool status)
        {
            popUpSetting.SetActive(status);
        }
        public void SetLevelText(int level)
        {
            levelText.text = $"Level {level}";
        }
        public void SetCoinText(int coin)
        {
            coinText.text = coin.ToString();
        }
    }
}

