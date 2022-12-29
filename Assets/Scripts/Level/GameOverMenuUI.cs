using System.Collections;
using TMPro;
using UnityEngine;

public class GameOverMenuUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _namePlayerWinnerTMP;
    [SerializeField] private float _playerWinerCountDownTime = 4f;
    public void SetNamePlayerWinner(string name)
    {
        gameObject.SetActive(true);
        _namePlayerWinnerTMP.text = name;
        StartCoroutine(PlayerWinerCountDown());

    }
    private IEnumerator PlayerWinerCountDown()
    {
        yield return new WaitForSeconds(_playerWinerCountDownTime);
        gameObject.SetActive(false);
    }
}
