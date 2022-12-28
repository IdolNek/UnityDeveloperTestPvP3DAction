using Mirror;
using System.Collections;
using UnityEngine;


public class PlayerScore : NetworkBehaviour
{
    [SyncVar]
    private int _score;
    private int _startScore = 0;
    public int Score => _score;
    private void OnEnable()
    {
        RoundEvent.GetInstance().RoundEnded += OnRoundEnded;
    }
    private void OnDisable()
    {
        RoundEvent.GetInstance().RoundEnded -= OnRoundEnded;
    }
    [Server]
    public void AddScoreForPlayer()
    {
        _score += 1;
        RoundEvent.GetInstance().OnScoreChanged();
    }
    private void OnRoundEnded()
    {
        if (isServer) ResetScore();
        else CmdResetScore();
    }
    [Server]
    private void ResetScore()
    {
        _score = _startScore;
    }
    [Command(requiresAuthority = false)]
    private void CmdResetScore()
    {
        ResetScore();
    }

}
