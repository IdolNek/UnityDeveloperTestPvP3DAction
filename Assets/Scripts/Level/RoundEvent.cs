using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoundEvent : NetworkBehaviour
{
    [SerializeField] private GameUI _gameUI;
    private static RoundEvent _instance;
    [SerializeField] private float _waitToRestartRound = 5f;
    public readonly SyncList<String> PlayerNickNames = new SyncList<string>();

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
