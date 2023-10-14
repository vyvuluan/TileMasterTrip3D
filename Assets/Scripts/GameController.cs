using System.Collections.Generic;
using UnityEngine;
public class GameController : MonoBehaviour
{
    [Header("MVC")]
    [SerializeField] private GameModel model;
    [SerializeField] private GameView view;
    [Header("Preferences")]
    [SerializeField] private Transform tf;
    [SerializeField] private TileSpawner tileSpawner;
    [SerializeField] private List<Transform> slots;
    [SerializeField] private LayerMask layerMaskTile;
    private RaycastHit hitInfo;
    private Dictionary<int, Tile> slotCurrentDics;

    public List<Tile> list = new();
    private void Awake()
    {
        slotCurrentDics = new();
        for (int i = 0; i < slots.Count; i++)
        {
            slotCurrentDics.Add(i, null);
        }
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hitInfo, 100f, layerMaskTile))
            {
                Tile tileTemp = hitInfo.collider.GetComponent<Tile>();
                tileTemp.SetRollback(hitInfo.collider.transform.position, hitInfo.collider.transform.rotation);
                HandleCollectTile(tileTemp);
            }
        }
        list = new();
        foreach (var item in slotCurrentDics)
        {
            list.Add(item.Value);
        }
    }
    public void HandleCollectTile(Tile tile)
    {
        if (slotCurrentDics[0] == null)
        {
            slotCurrentDics[0] = tile;
            tile.Collect(slots[0]);
        }
        else
        {
            for (int i = 0; i < slots.Count; i++)
            {
                if (slotCurrentDics[i] != null && slotCurrentDics[i].TileType == tile.TileType)
                {
                    MoveOneSlot(i, tile);
                    return;
                }
            }
            //haven't seen the of same type tile.
            MoveOneSlot(0, tile);
        }


    }
    public void MoveOneSlot(int index, Tile tile)
    {
        Debug.Log(index);
        for (int j = slots.Count - 1; j > index; j--)
        {
            if (slotCurrentDics[j - 1] != null)
            {
                slotCurrentDics[j - 1].Collect(slots[j]);
                slotCurrentDics[j] = slotCurrentDics[j - 1];
            }
        }
        slotCurrentDics[index] = tile;
        slotCurrentDics[index].Collect(slots[index]);

        Match(index);
    }
    public void Match(int index)
    {
        //match
        TileType tileType = slotCurrentDics[index].TileType;
        if (slotCurrentDics[index + 1] == null || slotCurrentDics[index + 2] == null) return;
        if (tileType == slotCurrentDics[index + 1].TileType && tileType == slotCurrentDics[index + 2].TileType)
        {
            slotCurrentDics[index].OnDespawn();
            slotCurrentDics[index + 1].OnDespawn();
            slotCurrentDics[index + 2].OnDespawn();
            slotCurrentDics[index] = slotCurrentDics[index + 1] = slotCurrentDics[index + 2] = null;
            for (int i = index + 3; i < slots.Count; i++)
            {
                if (slotCurrentDics[i] == null) continue;
                slotCurrentDics[i].Collect(slots[i - 3]);
                slotCurrentDics[i - 3] = slotCurrentDics[i];
                slotCurrentDics[i] = null;
            }

        }
    }
    public void Back()
    {
        for (int i = slots.Count - 1; i >= 0; i--)
        {
            if (slotCurrentDics[i] != null)
            {
                slotCurrentDics[i].Back();
                slotCurrentDics[i] = null;
                return;
            }
        }
    }
}
