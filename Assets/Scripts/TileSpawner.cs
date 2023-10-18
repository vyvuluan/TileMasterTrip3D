using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileSpawner : MonoBehaviour
{
    [SerializeField] private GameObject tilePrefab;
    [SerializeField] private Transform hat;
    [SerializeField] private List<Tile> tiles;

    private List<MapDetail> mapDetail;
    public void Initialized(List<MapDetail> mapDetail)
    {
        this.mapDetail = mapDetail;
        hat.gameObject.SetActive(true);
        tiles = new();
        hat.DOLocalRotate(new Vector3(0, 360, 0), 1, RotateMode.FastBeyond360)
            .SetLoops(-1, LoopType.Restart)
            .SetEase(Ease.Linear);
        StartCoroutine(SpawnTile());
    }
    private IEnumerator SpawnTile()
    {
        for (int i = 0; i < mapDetail.Count; i++)
        {
            for (int j = 0; j < mapDetail[i].Chance * 3; j++)
            {
                yield return new WaitForSeconds(0.1f);
                GameObject tileGo = SimplePool.Spawn(tilePrefab, hat.position, hat.rotation);
                tileGo.transform.SetParent(transform);
                Tile tile = tileGo.GetComponent<Tile>();
                tile.OnInit(mapDetail[i].Type, mapDetail[i].Sprite);
                tile.AddForce(Random.Range(1, 3), hat.forward);
                tiles.Add(tile);
            }
        }
        hat.gameObject.SetActive(false);


    }
    public List<Tile> FindMatchingTiles(int countToMatch, int type)
    {
        Dictionary<int, TileQuantity> temp = new();
        //init dictionary 
        for (int i = 0; i < mapDetail.Count; i++)
        {
            TileQuantity tileQuanity = new()
            {
                quantity = 0,
                tiles = new()
            };
            temp.Add(mapDetail[i].Type, tileQuanity);
        }

        //find
        for (int i = 0; i < tiles.Count; i++)
        {
            if (tiles[i].TileType != type && countToMatch != 3) continue;
            //update list temp
            TileQuantity tileQuanity = temp[tiles[i].TileType];
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
