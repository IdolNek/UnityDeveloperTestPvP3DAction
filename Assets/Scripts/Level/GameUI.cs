using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUI : MonoBehaviour
{
    [SerializeField] private StartGameCountDownUI _countDownUI;
    [SerializeField] private GameOverMenuUI _whoWinUI;


    IEnumerator Start()
    {
        while (RoundEvent.GetInstance() == null || RoundEvent.GetInstance().LocalPlayer == null)
            yield return null;
    }
    public void OnStartGame()
    {
        _countDownUI.StartCountDown();
    }

    public void OnShowWiner(string nickNameWiner)
    {
        _whoWinUI.SetNamePlayerWinner(nickNameWiner);
    }
}
