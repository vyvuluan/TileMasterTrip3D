using System;
using System.Collections.Generic;
using UnityEngine;

public class TileSpawner : MonoBehaviour
{
    [SerializeField] private GameObject tilePrefab;
    [SerializeField] private List<Tile> tiles;

    private List<MapDetail> mapDetail;
    private void Awake()
    {
        tiles = new();
        for (int i = 0; i < mapDetail.Count; i++)
        {
            for (int j = 0; j < mapDetail[i].Chance; j++)
            {
                Vector3 randomPosition = UnityEngine.Random.insideUnitSphere * 5;
                randomPosition.y = UnityEngine.Random.Range(3, 8);
                SpawnTile(randomPosition, mapDetail[i].Sprite, mapDetail[i].Type);
            }
        }

    }
    public void Initialized(List<MapDetail> mapDetail)
    {
        this.mapDetail = mapDetail;
    }
    private void SpawnTile(Vector3 position, Sprite sprite, TileType type)
    {
        GameObject tileGo = SimplePool.Spawn(tilePrefab, position, tilePrefab.transform.rotation);
        tileGo.transform.SetParent(transform);
        Tile tile = tileGo.GetComponent<Tile>();
        tile.OnInit(type, sprite);
        tiles.Add(tile);
    }
    public List<Tile> FindMatchingTiles(int countToMatch, TileType type)
    {
        List<TileQuantity> temp = new();

        //convect Enum TileType to Array
        TileType[] values = (TileType[])Enum.GetValues(typeof(TileType));
        //Init list temp
        for (int i = 0; i <= (int)values[^1]; i++)
        {
            TileQuantity tileQuanity = new()
            {
                quantity = 0,
                tiles = new()
            };
            temp.Add(tileQuanity);
        }
        //find
        for (int i = 0; i < tiles.Count; i++)
        {
            if (tiles[i].TileType != type && countToMatch != 3) continue;
            //update list temp
            TileQuantity tileQuanity = temp[(int)tiles[i].TileType];
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
    public bool CheckWin() => tiles.Count == 0;

}
public class TileQuantity
{
    public int quantity;
    public List<Tile> tiles;
}
