using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class RollbackBooster
{
    private int index;
    private Vector3 position;

    public Vector3 Position { get => position; set => position = value; }
    public int Index { get => index; set => index = value; }
    public RollbackBooster(int index, Vector3 position)
    {
        this.index = index;
        this.position = position;
    }
}
public class GameController : MonoBehaviour
{
    [Header("MVC")]
    [SerializeField] private GameModel model;
    [SerializeField] private GameView view;
    [Header("Preferences")]
    [SerializeField] private Transform tf;
    [SerializeField] private TileSpawner tileSpawner;
    [SerializeField] private List<Transform> slot;
    [SerializeField] private LayerMask layerMaskTile;
    private RaycastHit hitInfo;
    private Tile temp;
    private RollbackBooster rollback;
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hitInfo, 100f, layerMaskTile))
            {
                rollback = new RollbackBooster(0, hitInfo.collider.transform.position);
                temp = hitInfo.collider.GetComponent<Tile>();
                hitInfo.collider.GetComponent<Tile>().Collect(slot[0]);
            }
        }

    }
    public void Back()
    {
        temp.Back(rollback.Position);
    }
}
