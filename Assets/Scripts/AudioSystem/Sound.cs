using UnityEngine;
namespace AudioSystem
{
    [System.Serializable]
    public struct Sound
    {
        public string Name;
        public AudioClip AudioClip;
        [Range(0f, 1f)]
        public float Volume;
    }
}
