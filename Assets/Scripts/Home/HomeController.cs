using UnityEngine;
using UnityEngine.SceneManagement;

namespace Home
{
    public class HomeController : MonoBehaviour
    {
        [SerializeField] private HomeView view;

        private void Awake()
        {
            Application.targetFrameRate = 60;
            int levelCurrent = PlayerPrefs.GetInt(Constants.LevelPlayerPrefs, 1);
            int coinCurrent = PlayerPrefs.GetInt(Constants.CoinPlayerPrefs, 0);
            view.SetLevelText(levelCurrent);
            view.SetCoinText(coinCurrent);
        }
        public void Setting()
        {
            view.SetPopUpSetting(true);
        }
        public void Play()
        {
            SceneManager.LoadScene(Constants.GameplayScene);
        }
        public void QuitGame()
        {
            Application.Quit();
        }

    }
}

