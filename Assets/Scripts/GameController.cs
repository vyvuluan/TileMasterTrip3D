using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    private bool canTouch;
    private int levelCurrent;
    private RaycastHit hitInfo;
    private Dictionary<int, Tile> slotCurrentDics;
    public List<Tile> list = new();
    private void Awake()
    {
        canTouch = true;
        levelCurrent = PlayerPrefs.GetInt(Constants.LevelPlayerPrefs, 1);
        Debug.Log(levelCurrent);
        MapConfig mapConfig = model.MapConfig.Maps[levelCurrent - 1];
        view.SetLevelText(mapConfig.DisplayName);
        tileSpawner.Initialized(mapConfig.MapDetails);
        slotCurrentDics = new();
        for (int i = 0; i < slots.Count; i++)
        {
            slotCurrentDics.Add(i, null);
        }
        StartCoroutine(Countdown(mapConfig.PlayTime));

    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && canTouch)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hitInfo, 100f, layerMaskTile))
            {
                canTouch = false;
                Tile tileTemp = hitInfo.collider.GetComponent<Tile>();
                tileTemp.SetRollback(hitInfo.collider.transform.position, hitInfo.collider.transform.rotation, SetCanTouch);
                tileSpawner.RemoveTileFromTiles(tileTemp);
                HandleCollectTile(tileTemp);
                if (tileSpawner.CheckWin())
                {
                    Debug.Log("win");
                    Win();
                }
            }
        }
        list = new();
        foreach (var item in slotCurrentDics)
        {
            list.Add(item.Value);
        }
    }
    public void SetCanTouch(bool canTouch) => this.canTouch = canTouch;
    protected IEnumerator Countdown(float time)
    {
        float currentTime = time;
        while (currentTime > 0)
        {
            currentTime -= Time.deltaTime;
            TimeSpan timeSpan = TimeSpan.FromSeconds(currentTime);

            int minutes = timeSpan.Minutes;
            int remainingSeconds = timeSpan.Seconds;
            view.SetCountDownText(minutes, remainingSeconds);
            yield return null;
        }
        view.SetStatusAcitvePopUpLose(true);
    }
    private void HandleCollectTile(Tile tile)
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
    private bool CheckGameOver() => slotCurrentDics[slots.Count - 1] != null;

    private void MoveOneSlot(int index, Tile tile)
    {
        //Move from index position up 1 position
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
        StartCoroutine(Match(index));
    }
    public void Win()
    {
        levelCurrent++;
        PlayerPrefs.SetInt(Constants.LevelPlayerPrefs, levelCurrent);
        view.SetStatusAcitvePopUpWin(true);
    }
    public IEnumerator Match(int index)
    {
        //match
        TileType tileType = slotCurrentDics[index].TileType;
        if (slotCurrentDics[index + 1] == null || slotCurrentDics[index + 2] == null) yield break;
        if (tileType == slotCurrentDics[index + 1].TileType && tileType == slotCurrentDics[index + 2].TileType)
        {
            yield return new WaitForSeconds(0.4f);
            slotCurrentDics[index].OnDespawn();
            slotCurrentDics[index + 1].OnDespawn();
            slotCurrentDics[index + 2].OnDespawn();
            slotCurrentDics[index] = slotCurrentDics[index + 1] = slotCurrentDics[index + 2] = null;
            //move the components behind index + 3
            for (int i = index + 3; i < slots.Count; i++)
            {
                if (slotCurrentDics[i] == null) continue;
                slotCurrentDics[i].Collect(slots[i - 3]);
                slotCurrentDics[i - 3] = slotCurrentDics[i];
                slotCurrentDics[i] = null;
            }
        }
        else
        {
            if (CheckGameOver())
            {
                view.SetStatusAcitvePopUpLose(true);
            }

        }
    }
    public void ReloadScene()
    {
        SceneManager.LoadScene(Constants.GameplayScene);
    }
    public void Back()
    {
        for (int i = slots.Count - 1; i >= 0; i--)
        {
            if (slotCurrentDics[i] != null)
            {
                slotCurrentDics[i].Back();
                tileSpawner.AddTileFromTiles(slotCurrentDics[i]);
                slotCurrentDics[i] = null;
                return;
            }
        }
    }
    public void Suggest()
    {
        List<Tile> temp;
        if (slotCurrentDics[0] == null)
        {
            temp = tileSpawner.FindMatchingTiles(3, TileType.TYPE0);
        }
        else
        {
            temp = tileSpawner.FindMatchingTiles(2, slotCurrentDics[0].TileType);
            for (int i = 0; i < slots.Count - 1; i++)
            {
                //2 tile same
                if (slotCurrentDics[i + 1] != null && slotCurrentDics[i].TileType == slotCurrentDics[i + 1].TileType)
                {
                    temp = tileSpawner.FindMatchingTiles(1, slotCurrentDics[i].TileType);
                    break;
                }
            }
        }
        //handle after find
        for (int i = 0; i < temp.Count; i++)
        {
            tileSpawner.RemoveTileFromTiles(temp[i]);
            HandleCollectTile(temp[i]);
            if (tileSpawner.CheckWin())
            {
                Debug.Log("win");
                Win();
            }
        }
    }
}
