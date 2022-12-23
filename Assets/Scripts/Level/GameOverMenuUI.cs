using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameOverMenuUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _namePlayerWinnerTMP;
    public void SetNamePlayerWinner(string name)
    {
        gameObject.SetActive(true);
        _namePlayerWinnerTMP.text = name;
    }
}
