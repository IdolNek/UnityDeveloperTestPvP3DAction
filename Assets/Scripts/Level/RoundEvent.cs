using Mirror;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class RoundEvent : NetworkBehaviour
{
    [SerializeField] private GameUI _gameUI;
    [SerializeField] private float _waitToRestartRound = 5f;
    [SerializeField] private PlayerSpawner _playerSpawner;
    private static RoundEvent _instance;
    public readonly SyncList<NetworkGamePlayer> Players = new SyncList<NetworkGamePlayer> ();
    public event UnityAction RoundEnded;

    #region instance
    private void Awake()
    {
        _instance = this;
    }
    public static RoundEvent GetInstance()
    {
        return _instance;
    }
    #endregion

    
    public void AddPlayerToGamePlayerList(NetworkGamePlayer player)
    {
        Players.Add(player);
    }

    private void Start()
    {
        _gameUI.OnStartGame();
    }
    [Server]
    public void OnScoreChanged()
    {
        RpcShowScore(SetLineOfPoints());
        if (IsWinnerFounded(out NetworkGamePlayer winner))
        {
            RpcShowWinner(winner.NickName);
            GameOver();
        }
    }
    [Server]
    public void SpawnPlayer(NetworkGamePlayer player)
    {
        _playerSpawner.SpawnPlayer(player);
    }
    [Server]
    private void GameOver()
    {
        StartCoroutine(GameOverCoroutine());
    }
    [Server]
    private IEnumerator GameOverCoroutine()
    {
        yield return new WaitForSeconds(_waitToRestartRound);
        _playerSpawner.AddAllSpawnPointsFromRemovedList();
        foreach (var player in Players)
        {
            _playerSpawner.SpawnPlayer(player);
        }
        RpcShowScore(SetLineOfPoints());
        RpcShowStartUI();
    }
    [Server]
    private string SetLineOfPoints()
    {
        string score = "";
        foreach (var player in Players)
        {
            score += player.NickName + " Score: " + player.GetComponent<PlayerScore>().Score + " ";
        }
        return score;
    }
    [Server]
    private bool IsWinnerFounded(out NetworkGamePlayer winner)
    {
        winner = Players[0];
        foreach (var player in Players)
        {
            if (player.GetComponent<PlayerScore>().Score >= 3)
            {
                winner = player;
                return true;
            }

        }
        return false;
    }
    [ClientRpc]
    private void RpcShowScore(string score)
    {
        _gameUI.OnShowScore(score);
    }
    [ClientRpc]
    private void RpcShowWinner(string nickName)
    {
        _gameUI.OnShowWiner(nickName);
        RoundEnded?.Invoke();
    }
    [ClientRpc]
    private void RpcShowStartUI()
    {
        _gameUI.OnStartGame();
    }
}
