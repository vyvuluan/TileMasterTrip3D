using UnityEngine;

public class GameModel : MonoBehaviour
{
    [SerializeField] private MapConfigSO mapConfig;

    public MapConfigSO MapConfig { get => mapConfig; }
}
