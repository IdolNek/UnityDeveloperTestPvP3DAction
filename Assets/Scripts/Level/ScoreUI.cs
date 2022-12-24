using TMPro;
using UnityEngine;

public class ScoreUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _scoreText;
    public void SetPlayerScore(string score)
    {
        gameObject.SetActive(true);
        _scoreText.text = score;
    }
}
