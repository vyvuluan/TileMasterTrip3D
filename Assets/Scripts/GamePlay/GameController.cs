using AudioSystem;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GamePlay
{
    public class GameController : MonoBehaviour
    {
        [Header("MVC")]
        [SerializeField] private GameModel model;
        [SerializeField] private GameView view;
        [Header("Preferences")]
        [SerializeField] private Transform slotAdd;
        [SerializeField] private Transform slot;
        [SerializeField] private TileSpawner tileSpawner;
        [SerializeField] private List<Transform> slots;
        [SerializeField] private LayerMask layerMaskTile;
        private int levelCurrent;
        private int coinCurrent;
        private int coinInLevel;
        private float worldHeight;
        private float worldWidth;
        private bool isMatching = false;
        private bool canTouch = true;
        private bool isCountdownPaused = false;
        private bool isUsingFreezeTime = false;
        private RaycastHit hitInfo;
        private Dictionary<int, Tile> slotCurrentDics;
        private AudioController audioController;
        #region Variable Combo
        private int currentCombo = 0;
        private float comboTime = 10f;
        private float currentTimeRemaining;
        #endregion

        private void Awake()
        {
            if (GameObject.FindGameObjectWithTag(Constanst.SoundTag) == null)
            {
                SceneManager.LoadScene(Constanst.EntryScene);
            }
            else
            {
                audioController = AudioController.Instance;
            }
            Application.targetFrameRate = 60;
            Initialized();
        }
        private void Start()
        {
            //scale with every monitor
            float value = (slot.localScale.x - 10) / 10;
            tileSpawner.transform.localScale = new Vector3(value + 1f, value + 1f, value + 1f);
        }
        private void Update()
        {
            if (Input.GetMouseButtonDown(0) && canTouch && !isMatching)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hitInfo, 100f, layerMaskTile))
                {
                    audioController.AudioService.PlayClicked();
                    canTouch = false;
                    Tile tileTemp = hitInfo.collider.GetComponent<Tile>();
                    tileTemp.SetRollback(hitInfo.collider.transform.position, hitInfo.collider.transform.rotation, SetCanTouch);
                    tileSpawner.RemoveTileFromTiles(tileTemp);
                    HandleCollectTile(tileTemp);
                    if (tileSpawner.CheckWin())
                    {
                        Win();
                    }
                }
            }
            HandleTimeCombo();
        }
        private Vector3 CalculatorScaleX(float scale, Transform tf)
        {
            Vector3 initialScale = tf.localScale;
            float scaleXRatio = scale / initialScale.x;
            float newScaleY = initialScale.y * scaleXRatio;
            float newScaleZ = initialScale.z * scaleXRatio;
            return new Vector3(scale, newScaleY, newScaleZ);
        }
        private void Initialized()
        {
            worldHeight = Camera.main.orthographicSize * 2f;
            worldWidth = worldHeight * Screen.width / Screen.height;
            view.SetPosWallWithScreen(worldHeight, worldWidth);
            //scale with every monitor
            slot.localScale = CalculatorScaleX(worldWidth - 1.5f, slot);
            Application.targetFrameRate = 60;
            currentTimeRemaining = comboTime;
            coinCurrent = PlayerPrefs.GetInt(Constanst.CoinPlayerPrefs, 0);
            levelCurrent = PlayerPrefs.GetInt(Constanst.LevelPlayerPrefs, 1);
            MapConfig mapConfig = model.MapConfig.Maps[levelCurrent - 1];
            view.SetLevelText(mapConfig.DisplayName);
            view.SetCoinText(coinCurrent);
            tileSpawner.Initialized(mapConfig.MapDetails);
            slotCurrentDics = new();
            for (int i = 0; i < slots.Count; i++)
            {
                slotCurrentDics.Add(i, null);
            }
            StartCoroutine(Countdown(mapConfig.PlayTime));
        }
        private void HandleTimeCombo()
        {
            if (currentCombo <= 0) return;
            currentTimeRemaining -= Time.deltaTime;
            view.SetFillAmountCombo(Mathf.Clamp01(currentTimeRemaining / comboTime));
            if (currentTimeRemaining <= 0)
            {
                ResetCombo();
            }
        }
        private void ResetCombo()
        {
            currentCombo = 0;
            comboTime = 10f;
            currentTimeRemaining = comboTime;
            view.SetFillAmountCombo(1);
            view.SetStatusActiveCombo(false);
        }
        private void ComboCompleted()
        {
            view.SetStatusActiveCombo(true);
            view.SetFillAmountCombo(1);
            //Increase Combo
            currentCombo++;
            //SetCoin
            coinCurrent += currentCombo;
            coinInLevel += currentCombo;
            view.SetCoinText(coinCurrent);
            PlayerPrefs.SetInt(Constanst.CoinPlayerPrefs, coinCurrent);
            view.SetComboText(currentCombo.ToString());
            //Update combo time after decrease 5% time 
            comboTime *= 1f - 0.05f * currentCombo;
            currentTimeRemaining = comboTime;
        }
        public void SetCanTouch(bool canTouch) => this.canTouch = canTouch;
        protected IEnumerator Countdown(float time)
        {
            float currentTime = time;
            while (currentTime > 0)
            {
                if (!isCountdownPaused)
                {
                    currentTime -= Time.deltaTime;
                }
                view.SetCountDownText(ConvertTimeToMinutesAndSeconds(currentTime));
                yield return null;
            }
            view.SetStatusAcitvePopUpLose(true);
        }
        private string ConvertTimeToMinutesAndSeconds(float time)
        {
            int minutes = Mathf.FloorToInt(time / 60);
            int seconds = Mathf.FloorToInt(time % 60);

            return $"{minutes}:{seconds}";
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

        public IEnumerator Match(int index)
        {
            //match
            if (slotCurrentDics[index] == null || slotCurrentDics[index + 1] == null || slotCurrentDics[index + 2] == null) yield break;
            int tileType = slotCurrentDics[index].TileType;
            if (tileType == slotCurrentDics[index + 1].TileType && tileType == slotCurrentDics[index + 2].TileType)
            {
                isMatching = true;
                audioController.AudioService.PlayMatch();
                Tile tile = slotCurrentDics[index];
                Tile tile1 = slotCurrentDics[index + 1];
                Tile tile2 = slotCurrentDics[index + 2];

                slotCurrentDics[index] = null;
                slotCurrentDics[index + 1] = null;
                slotCurrentDics[index + 2] = null;
                yield return new WaitForSeconds(0.3f);
                ComboCompleted();
                tile.OnDespawn();
                tile1.OnDespawn();
                tile2.OnDespawn();
                //move the components behind index + 3

                for (int i = index + 3; i < slots.Count; i++)
                {
                    if (slotCurrentDics[i] == null) continue;
                    slotCurrentDics[i].Collect(slots[i - 3]);
                    slotCurrentDics[i - 3] = slotCurrentDics[i];
                    slotCurrentDics[i] = null;
                }

                isMatching = false;
            }
            else
            {
                if (CheckGameOver())
                {
                    GameOver();
                }

            }
        }
        private void GameOver()
        {
            isMatching = true;
            audioController.AudioService.PlayLose();
            view.SetStatusActiveCombo(false);
            view.SetPopUpLose(levelCurrent, coinInLevel);
        }
        public void Win()
        {
            isMatching = true;
            audioController.AudioService.PlayWin();
            //view.SetStatusActiveCombo(false);
            view.SetPopUpWin(levelCurrent, coinInLevel);
            levelCurrent++;
            PlayerPrefs.SetInt(Constanst.LevelPlayerPrefs, levelCurrent);
        }
        public void ReloadScene()
        {
            isMatching = true;
            DOTween.KillAll();
            SceneManager.LoadScene(Constanst.GameplayScene);
        }
        public void ChangeHomeScene()
        {
            isMatching = true;
            DOTween.KillAll();
            SceneManager.LoadScene(Constanst.HomeScene);
        }
        #region BOOSTER
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
        public void Hint()
        {
            List<Tile> temp;
            if (slotCurrentDics[0] == null)
            {
                temp = tileSpawner.FindMatchingTiles(3, 0);
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
                    Win();
                }
            }
        }
        public void AddSlot()
        {
            slots.Add(slotAdd);
            slotCurrentDics.Add(slotCurrentDics.Count, null);
            view.SetStatusAcitveAddSlot(false);
        }
        public void FreezeTime()
        {
            if (isUsingFreezeTime) return;
            isCountdownPaused = true;
            isUsingFreezeTime = true;
            Invoke(nameof(UsingFreezeTimeFinish), model.TimeFreeze);
        }
        private void UsingFreezeTimeFinish()
        {
            isCountdownPaused = false;
            isUsingFreezeTime = false;
        }
        #endregion
    }
}
