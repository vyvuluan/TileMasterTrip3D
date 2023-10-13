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
                GetSlotEmpty(tileTemp);
            }
        }

    }
    public void GetSlotEmpty(Tile tile)
    {
        for (int i = 0; i < slots.Count; i++)
        {
            if (slotCurrentDics[i] == null)
            {
                slotCurrentDics[i] = tile;
                tile.Collect(slots[i]);
                return;
            }
            else if (slotCurrentDics[i].TileType == tile.TileType)
            {
                MoveOneSlot(i, tile);
                return;
            }
        }
        MoveOneSlot(0, tile);

    }
    public void MoveOneSlot(int index, Tile tile)
    {
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
