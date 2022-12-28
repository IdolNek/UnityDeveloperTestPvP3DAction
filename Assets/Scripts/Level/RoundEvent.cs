using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class RoundEvent : NetworkBehaviour
{
    [SerializeField] private GameUI _gameUI;
    [SerializeField] private float _waitToRestartRound = 5f;
    [SerializeField] private List<Transform> _spawnPoints;
    [SerializeField] private PlayerSpawner _playerSpawner;
    private static RoundEvent _instance;
    public readonly SyncList<NetworkGamePlayer> Players = new SyncList<NetworkGamePlayer> ();
    public event UnityAction RoundEnded;
   // public event UnityAction<Transform> RoundStarted;

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
    private void GameOver()
    {
        StartCoroutine(GameOverCoroutine());
    }
    [Server]
    private IEnumerator GameOverCoroutine()
    {
        yield return new WaitForSeconds(_waitToRestartRound);
        foreach (var player in Players)
        {
            _playerSpawner.SpawnPlayer(player);
        }
        _playerSpawner.AddAllSpawnPointsFromRemovedList();
        RpcShowStartUI();
        RpcShowScore(SetLineOfPoints());
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
    [ClientRpc]
    private void RpcShowScore(string score)
    {
        _gameUI.OnShowScore(score);
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









    //[Command(requiresAuthority = false)]
    //private void CmdSetLineOfPoints()
    //{
    //    SetLineOfPoints();
    //}
    //[Server]
    //private void ShowWiner()
    //{
    //    RpcShowWiner(PlayerNickNames[0]);
    //}
    //[Command(requiresAuthority = false)]
    //private void CmdShowWiner()
    //{
    //    ShowWiner();
    //}
    //[ClientRpc]
    //private void RpcShowWiner(string nickName)
    //{
    //    _gameUI.OnShowWiner(nickName);
    //}
 





    //public void AddPlayerNickName(string nickName)
    //{
    //    PlayerNickNames.Add(nickName);
    //}
    //private void AddAllPlayerNickName()
    //{
    //    foreach (var player in Players)
    //    {
    //        AddPlayerNickName(player.NickName);
    //    }
    //}
    //public void RemovePlayerNickName(string nickName)
    //{
    //    PlayerNickNames.Remove(nickName);
    //}



    //public bool IsGameOver()
    //{
    //    return PlayerNickNames.Count == 1;
    //}
    //public void GameOver()
    //{
    //    //if (isServer) SetScoreForWiner();
    //    //else CmdSetScoreForWiner();
    //    //if (isServer) ShowWiner();
    //    //else CmdShowWiner();
    //    //StartCoroutine(GameOverCoroutine());
    //}
    //[Command(requiresAuthority = false)]
    //private void CmdSetScoreForWiner()
    //{
    //    SetScoreForWiner();
    //}
    //[Server]
    //private void SetScoreForWiner()
    //{
    //    foreach (var player in Players)
    //    {
    //        if (player.NickName == PlayerNickNames[0]) player.Score += 1;
    //    }
    //}

    //private IEnumerator GameOverCoroutine()
    //{
    //    yield return new WaitForSeconds(_waitToRestartRound);
    //    PlayerNickNames.Clear();
    //    AddAllPlayerNickName();
    //    if (isServer) StartNewGame();
    //    else CmdStartNewGame();
    //    if (isServer) SetLineOfPoints();
    //    else CmdSetLineOfPoints();
    //}



    //[Server]
    //private void StartNewGame()
    //{
    //    RpcStartNewGame();
    //}
    //[Command(requiresAuthority = false)]
    //private void CmdStartNewGame()
    //{
    //    StartNewGame();
    //}
    //[ClientRpc]
    //private void RpcStartNewGame()
    //{       
    //    OnStartGame?.Invoke(_spownPoints);
    //    _gameUI.OnStartGame();
    //}

    //internal void OnScoreChanged()
    //{
    //    throw new NotImplementedException();
    //}


}
