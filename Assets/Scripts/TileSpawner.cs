using System;
using System.Collections.Generic;
using UnityEngine;

public class TileSpawner : MonoBehaviour
{
    [SerializeField] private GameObject tilePrefab;

    [SerializeField] private List<Tile> tiles;
    private void Awake()
    {
        tiles = new();
        for (int i = 0; i < 30; i++)
        {
            Vector3 randomPosition = UnityEngine.Random.insideUnitSphere * 5;
            randomPosition.y = UnityEngine.Random.Range(3, 8);
            SpawnTile(randomPosition);
        }

    }

    private void SpawnTile(Vector3 position)
    {
        GameObject tileGo = SimplePool.Spawn(tilePrefab, position, tilePrefab.transform.rotation);
        tileGo.transform.SetParent(transform);
        Tile tile = tileGo.GetComponent<Tile>();
        tile.OnInit(TileType.TYPE2);
        tiles.Add(tile);
    }
    public List<Tile> FindMatchingTiles(int countToMatch)
    {
        List<TileQuanity> temp = new();

        TileType[] values = (TileType[])Enum.GetValues(typeof(TileType));

        for (int i = 0; i <= (int)values[^1]; i++)
        {
            TileQuanity tileQuanity = new()
            {
                quantity = 0,
                tiles = new()
            };
            temp.Add(tileQuanity);
        }

        for (int i = 0; i < tiles.Count; i++)
        {
            TileQuanity tileQuanity = temp[(int)tiles[i].TileType];
            tileQuanity.tiles.Add(tiles[i]);
            tileQuanity.quantity++;
            if (tileQuanity.quantity >= countToMatch)
            {
                return tileQuanity.tiles;
            }
        }
        return null;
    }
    public void RemoveTileFromTiles(Tile tile) => tiles.Remove(tile);
    public void AddTileFromTiles(Tile tile) => tiles.Add(tile);

}
public class TileQuanity
{
    public int quantity;
    public List<Tile> tiles;
}
