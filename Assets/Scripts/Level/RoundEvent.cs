using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class RoundEvent : NetworkBehaviour
{
    [SerializeField] private GameUI _gameUI;
    [SerializeField] private float _waitToRestartRound = 5f;
    [SerializeField] private Transform[] _spownPoints;
    private static RoundEvent _instance;
    public readonly SyncList<NetworkGamePlayer> Players = new SyncList<NetworkGamePlayer> ();
    public readonly SyncList<String> PlayerNickNames = new SyncList<string>();
    public event UnityAction<Transform[]> OnStartGame;

    private void Awake()
    {
        _instance = this;
    }
    public static RoundEvent GetInstance()
    {
        return _instance;
    }
    private void Start()
    {
        _gameUI.OnStartGame();
    }
    public bool IsGameOver()
    {
        return PlayerNickNames.Count == 1;
    }
    public void AddPlayerNickName(string nickName)
    {
        PlayerNickNames.Add(nickName);
    }
    public void AddPlayerToGamePlayerList(NetworkGamePlayer player)
    {
        Players.Add(player);
    }
    private void AddAllPlayerNickName()
    {
        foreach (var player in Players)
        {
            AddPlayerNickName(player.NickName);
        }
    }
    public void RemovePlayerNickName(string nickName)
    {
        PlayerNickNames.Remove(nickName);
    }
    public void GameOver()
    {
        if (isServer) SetScoreForWiner();
        else CmdSetScoreForWiner();
        if (isServer) ShowWiner();
        else CmdShowWiner();
        StartCoroutine(GameOverCoroutine());
    }
    [Command(requiresAuthority = false)]
    private void CmdSetScoreForWiner()
    {
        SetScoreForWiner();
    }
    [Server]
    private void SetScoreForWiner()
    {
        foreach (var player in Players)
        {
            if (player.NickName == PlayerNickNames[0]) player.Score += 1;
        }
    }
    [Server]
    private void SetLineOfPoints()
    {
        string score = "";
        foreach (var player in Players)
        {
            score += player.NickName + " Score: " + player.Score + " ";
        }
        RpcSetLineOfPoints(score);
    }
    [Command(requiresAuthority = false)]
    private void CmdSetLineOfPoints()
    {
        SetLineOfPoints();
    }
    [ClientRpc]
    private void RpcSetLineOfPoints(string score)
    {
        _gameUI.OnShowScore(score);
    }

    private IEnumerator GameOverCoroutine()
    {
        yield return new WaitForSeconds(_waitToRestartRound);
        PlayerNickNames.Clear();
        AddAllPlayerNickName();
        if (isServer) StartNewGame();
        else CmdStartNewGame();
        if (isServer) SetLineOfPoints();
        else CmdSetLineOfPoints();
    }
    [Server]
    private void StartNewGame()
    {
        RpcStartNewGame();
    }
    [Command(requiresAuthority = false)]
    private void CmdStartNewGame()
    {
        StartNewGame();
    }
    [ClientRpc]
    private void RpcStartNewGame()
    {       
        OnStartGame?.Invoke(_spownPoints);
    }
    [Server]
    private void ShowWiner()
    {
        RpcShowWiner(PlayerNickNames[0]);
    }
    [Command(requiresAuthority = false)]
    private void CmdShowWiner()
    {
        ShowWiner();
    }
    [ClientRpc]
    private void RpcShowWiner(string nickName)
    {
        _gameUI.OnShowWiner(nickName);
    }
}
