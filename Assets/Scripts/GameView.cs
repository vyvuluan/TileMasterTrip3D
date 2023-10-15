using TMPro;
using UnityEngine;

public class GameView : MonoBehaviour
{
    [SerializeField] private GameObject popUpWin;
    [SerializeField] private GameObject popUpLose;
    [SerializeField] private TextMeshProUGUI countDownText;
    [SerializeField] private TextMeshProUGUI levelText;

    public void SetCountDownText(int minute, int second) => countDownText.text = $"{minute}:{second}";
    public void SetLevelText(string text) => levelText.text = text;
    public void SetStatusAcitvePopUpWin(bool status) => popUpWin.SetActive(status);
    public void SetStatusAcitvePopUpLose(bool status) => popUpLose.SetActive(status);
}
