using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GamePlay
{
    public class GameView : MonoBehaviour
    {
        [Header("POPUP WIN")]
        [SerializeField] private GameObject popUpWin;
        [SerializeField] private TextMeshProUGUI levelTextPopUpWin;
        [SerializeField] private TextMeshProUGUI ScoreTextPopUpWin;

        [Header("POPUP LOSE")]
        [SerializeField] private GameObject popUpLose;
        [SerializeField] private TextMeshProUGUI levelTextPopUpLose;
        [SerializeField] private TextMeshProUGUI ScoreTextPopUpLose;

        [SerializeField] private GameObject addSlot;
        [SerializeField] private TextMeshProUGUI countDownText;
        [SerializeField] private TextMeshProUGUI levelText;
        [SerializeField] private TextMeshProUGUI coinText;

        [Header("COMBO")]
        [SerializeField] private Image fillAmountCombo;
        [SerializeField] private GameObject combo;
        [SerializeField] private TextMeshProUGUI comboText;

        public void SetCountDownText(int minute, int second) => countDownText.text = $"{minute}:{second}";
        public void SetLevelText(string text) => levelText.text = text;
        public void SetStatusAcitvePopUpWin(bool status) => popUpWin.SetActive(status);
        public void SetStatusAcitvePopUpLose(bool status) => popUpLose.SetActive(status);
        public void SetStatusAcitveAddSlot(bool status) => addSlot.SetActive(status);
        public void SetStatusActiveCombo(bool status) => combo.SetActive(status);
        public void SetFillAmountCombo(float amount) => fillAmountCombo.fillAmount = amount;
        public void SetCoinText(int coin) => coinText.text = coin.ToString();
        public void SetComboText(string text)
        {
            comboText.text = $"Combo x{text}";

            //anim zoom in and zoom out
            comboText.rectTransform.DOScale(new Vector3(1.5f, 1.5f, 1.5f), 0.3f).OnComplete(() =>
            {
                comboText.rectTransform.DOScale(Vector3.one, 0.3f);
            });
        }
        public void SetPopUpWin(int level, int score)
        {
            popUpWin.SetActive(true);
            levelTextPopUpWin.text = level.ToString();
            ScoreTextPopUpWin.text = score.ToString();
        }
        public void SetPopUpLose(int level, int score)
        {
            popUpLose.SetActive(true);
            levelTextPopUpLose.text = level.ToString();
            ScoreTextPopUpLose.text = score.ToString();
        }
    }
}
