using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoundEvent : NetworkBehaviour
{
    [SerializeField] private GameUI _gameUI;
    [SerializeField] private float _waitToRestartRound = 5f;
    [SerializeField] private Transform _spownPoints;
    private static RoundEvent _instance;
    public readonly SyncList<String> PlayerNickNames = new SyncList<string>();
    private List<NetworkGamePlayer> players = new List<NetworkGamePlayer> ();

    private void Awake()
    {
        _instance = this;
    }
    public static RoundEvent GetInstance()
    {
        return _instance;
    }
    public bool IsGameOver()
    {
        return PlayerNickNames.Count == 1;
    }
    private void Start()
    {
        _gameUI.OnStartGame();
        foreach (var player in NetworkLobbyManager.GetInstance().Players)
        {
            players.Add(player);
        }
    }
    public void AddPlayerNickName(string nickName)
    {
        PlayerNickNames.Add(nickName);
    }
    public void RemovePlayerNickName(string nickName)
    {
        PlayerNickNames.Remove(nickName);
    }

    public void GameOver()
    {
        CmdGameOver();
        StartCoroutine(GameOverCoroutine());
        CmdDeleteSpawnPrefab();
    }
    [ClientRpc]
    private void CmdDeleteSpawnPrefab()
    {

        
    }

    private IEnumerator GameOverCoroutine()
    {
        yield return new WaitForSeconds(_waitToRestartRound);

    }

    [Command(requiresAuthority = false)]
    private void CmdGameOver()
    {
        RpcShowWiner();
    }
    [ClientRpc]
    private void RpcShowWiner()
    {
        _gameUI.OnShowWiner(PlayerNickNames[0]);
    }
    

}
