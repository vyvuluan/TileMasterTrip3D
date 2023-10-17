using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace AudioSystem
{

    public class Music : MonoBehaviour
    {
        [SerializeField] private List<Sound> musics;

        private Audio audioService;

        // Cache
        private readonly Dictionary<string, AudioSource> audioSources = new Dictionary<string, AudioSource>();
        private Dictionary<string, float> musicVolumes = new Dictionary<string, float>();

        private void Awake()
        {
            foreach (var music in musics)
            {
                var audioSource = gameObject.AddComponent<AudioSource>();
                audioSource.clip = music.AudioClip;
                audioSource.playOnAwake = false;

                audioSources.Add(music.Name, audioSource);
                musicVolumes.Add(music.Name, music.Volume);
            }
        }

        private void Start()
        {
            foreach (var music in audioSources)
            {
                music.Value.volume = audioService.MusicVolume * musicVolumes[music.Key];
            }
        }

        private void OnDisable()
        {
            audioService.OnSoundChanged -= AudioService_OnMusicChanged;
            audioService.OnSoundVolumeChanged -= AudioService_OnMusicVolumeChanged;
        }

        private void AudioService_OnMusicChanged(bool isOn)
        {
            if (isOn == false)
            {
                foreach (var music in audioSources)
                {
                    music.Value.Stop();
                }
            }
        }

        private void AudioService_OnMusicVolumeChanged(float volume)
        {
            foreach (var music in audioSources)
            {
                music.Value.volume = volume * musicVolumes[music.Key];
                if (IsMusicPlaying(music.Key) == false && music.Value.volume > 0f)
                {
                    PlayMusic(music.Key);
                    music.Value.loop = true;
                }
            }
        }

        public void PlayMusic(string name)
        {
            foreach (var item in audioSources)
            {
                Debug.Log($"{item.Key} x {item.Value.GetComponent<AudioSource>().name} ");
            }
            Debug.Log("name " + name);
            if (audioSources.ContainsKey(name))
            {
                audioSources[name].Play();
                audioSources[name].loop = true;
            }
            else
            {
                Debug.Log($"Music: {name} not found!");
            }
        }
        public void FadeMusic(string name, float time)
        {
            if (audioSources.ContainsKey(name))
            {
                AudioSource audioSource = audioSources[name];
                if (audioSource.isPlaying == false)
                {
                    audioSource.Play();
                }
                StartCoroutine(FadeMusicCoroutine(audioSource, time));
            }
            else
            {
                Debug.Log($"Music: {name} not found!");
            }
        }
        private IEnumerator FadeMusicCoroutine(AudioSource audioSource, float time)
        {
            float deltaT = 0.02f;
            float volumeTemp = audioSource.volume;
            float step = time / deltaT;
            float stepVolume = volumeTemp / step;

            for (int i = 0; i < 150; i++)
            {
                yield return new WaitForSeconds(0.01f);
                audioSource.volume -= stepVolume;
            }

            audioSource.Stop();
            audioSource.volume = volumeTemp;
        }
        public void StopMusic(string name)
        {
            if (audioSources.ContainsKey(name))
            {
                audioSources[name].Stop();
            }
            else
            {
                Debug.Log($"Music: {name} not found!");
            }
        }

        public bool IsMusicPlaying(string name)
        {
            if (audioSources.ContainsKey(name))
            {
                if (audioSources[name].isPlaying == true)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            Debug.Log($"Music: {name} not found!");
            return false;
        }


        public void Initialized(Audio audioService)
        {
            this.audioService = audioService;

            audioService.OnMusicChanged += AudioService_OnMusicChanged;
            audioService.OnMusicVolumeChanged += AudioService_OnMusicVolumeChanged;
        }
    }
}
