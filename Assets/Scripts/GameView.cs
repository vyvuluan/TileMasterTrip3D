using TMPro;
using UnityEngine;

public class GameView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI countDown;

    public void SetCountDownText(int minute, int second)
    {
        countDown.text = $"{minute}:{second}";
    }
}
