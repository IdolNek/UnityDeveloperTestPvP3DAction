using UnityEngine;

public class GameUI:MonoBehaviour
{
    [SerializeField] private StartGameCountDownUI _countDownUI;
    [SerializeField] private GameOverMenuUI _whoWinUI;
    [SerializeField] private ScoreUI _scoreUI;

    public void OnStartGame()
    {
        _countDownUI.StartCountDown();
    }
    public void OnShowWiner(string nickNameWiner)
    {
        _whoWinUI.SetNamePlayerWinner(nickNameWiner);
    }   
    public void OnShowScore(string score)
    {
        _scoreUI.SetPlayerScore(score);
    }
}
