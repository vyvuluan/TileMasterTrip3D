using UnityEngine;

namespace GamePlay
{
    public class GameModel : MonoBehaviour
    {
        [SerializeField] private float timeFreeze = 10f;
        [SerializeField] private MapConfigSO mapConfig;

        public MapConfigSO MapConfig { get => mapConfig; }
        public float TimeFreeze { get => timeFreeze; }
    }
}
