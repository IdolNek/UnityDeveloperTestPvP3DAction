using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
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
