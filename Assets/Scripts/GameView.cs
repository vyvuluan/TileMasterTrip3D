using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameView : MonoBehaviour
{
    [SerializeField] private GameObject popUpWin;
    [SerializeField] private GameObject popUpLose;
    [SerializeField] private GameObject addSlot;
    [SerializeField] private TextMeshProUGUI countDownText;
    [SerializeField] private TextMeshProUGUI levelText;

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
    public void SetComboText(string text) => comboText.text = text;
}
