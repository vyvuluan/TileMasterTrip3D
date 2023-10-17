using AudioSystem;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Home
{
    public class HomeController : MonoBehaviour
    {
        [SerializeField] private HomeView view;
        private AudioController audioController;
        private void Awake()
        {
            Application.targetFrameRate = 60;
            audioController = AudioController.Instance;
            int levelCurrent = PlayerPrefs.GetInt(Constanst.LevelPlayerPrefs, 1);
            int coinCurrent = PlayerPrefs.GetInt(Constanst.CoinPlayerPrefs, 0);
            view.SetSliderMusic(audioController.GetMusicVolume());
            view.SetSliderSound(audioController.GetSoundVolume());
            view.SetLevelText(levelCurrent);
            view.SetCoinText(coinCurrent);
        }
        public void Setting()
        {
            audioController.AudioService.PlayClicked();
            view.SetStatusActiveSetting(true);
        }
        public void CloseSetting()
        {
            audioController.AudioService.PlayClicked();
            view.SetStatusActiveSetting(false);
        }
        public void Play()
        {
            audioController.AudioService.PlayClicked();
            DontDestroyOnLoad(audioController.MusicObject);
            DontDestroyOnLoad(audioController.SoundObject);
            DontDestroyOnLoad(audioController);
            SceneManager.LoadScene(Constanst.GameplayScene);

        }
        public void QuitGame()
        {
            audioController.AudioService.PlayClicked();
            Application.Quit();
        }
        public void EventPointerDownSliderMusic()
        {
            float value = view.GetValueSliderMusic();
            audioController.SetMusicVolume(value);
        }
        public void EventPointerDownSliderSound()
        {
            float value = view.GetValueSliderSound();
            audioController.SetSoundVolume(value);
        }

    }
}

