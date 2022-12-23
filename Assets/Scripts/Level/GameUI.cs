using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUI : MonoBehaviour
{
    [SerializeField] private StartGameCountDownUI _countDownUI;
    [SerializeField] private GameOverMenuUI _whoWinUI;
    public void OnStartGame()
    {
        _countDownUI.StartCountDown();
    }

    public void OnShowWiner(string nickNameWiner)
    {
        _whoWinUI.SetNamePlayerWinner(nickNameWiner);
    }
}
