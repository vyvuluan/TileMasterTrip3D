using System.Collections.Generic;
using UnityEngine;

namespace AudioSystem
{
    public class AudioController : MonoBehaviour
    {
        private static AudioController instance;
        private const string MusicVolumeKey = "mvl";
        private const string SoundVolumeKey = "svl";

        private const string soundObjectName = "Sound";

        [SerializeField] private List<Sound> sounds;
        [SerializeField] private Music music;
        [SerializeField] private GameObject musicObject;

        private Audio audioService;
        private GameObject soundObject;

        public GameObject MusicObject { get => musicObject; set => musicObject = value; }
        public GameObject SoundObject { get => soundObject; set => soundObject = value; }
        public Audio AudioService { get => audioService; set => audioService = value; }
        public static AudioController Instance { get => instance; }

        void Awake()
        {
            instance = this;
            // Instantie Audio
            //DontDestroyOnLoad(musicObject);

            soundObject = new(soundObjectName);
            //DontDestroyOnLoad(soundObject);
            audioService = new Audio(music, sounds, soundObject)
            {
                MusicVolume = GetMusicVolume(),
                SoundVolume = GetSoundVolume(),


                MusicOn = true,
                SoundOn = true
            };

            audioService.StopMusic();
            // ------------------------------------------------------------------

        }
        public float GetMusicVolume()
        {
            return PlayerPrefs.GetFloat(MusicVolumeKey, 1.0f);
        }
        public void SetMusicVolume(float volume)
        {
            PlayerPrefs.SetFloat(MusicVolumeKey, volume);
            //OnMusicVolumeChange?.Invoke(volume);
            audioService.SetMusicVolume(volume);
        }
        public float GetSoundVolume()
        {
            return PlayerPrefs.GetFloat(SoundVolumeKey, 1.0f);
        }
        public void SetSoundVolume(float volume)
        {
            PlayerPrefs.SetFloat(SoundVolumeKey, volume);
            //OnSoundVolumeChange?.Invoke(volume);
            audioService.SetSoundVolume(volume);
        }
    }
}


