using UnityEngine;

public class TileSpawner : MonoBehaviour
{
    [SerializeField] private GameObject tilePrefab;


    private void Awake()
    {
        for (int i = 0; i < 10; i++)
        {
            Vector3 randomPosition = Random.insideUnitSphere * 5;
            randomPosition.y = 5;
            SpawnTile(randomPosition);
        }
    }

    private void SpawnTile(Vector3 position)
    {
        GameObject tileGo = SimplePool.Spawn(tilePrefab, position, tilePrefab.transform.rotation);
        tileGo.transform.SetParent(transform);
    }
}
