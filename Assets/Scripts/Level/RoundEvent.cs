using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundEvent : NetworkBehaviour
{
    [SerializeField] private GameUI _gameUI;
    private static RoundEvent instance;
    [HideInInspector]
    public Player LocalPlayer;
    public readonly SyncList<String> PlayerNickNames = new SyncList<string>();

    public bool IsGameOver()
    {
        Debug.Log($"В момент проверки количество игроков = {PlayerNickNames.Count}");
        return PlayerNickNames.Count == 1;
    }

    private void Awake()
    {
        instance = this;
    }
    public static RoundEvent GetInstance()
    {
        return instance;
    }
    //private void Start()
    //{
    //    if (isServer) StartGame();
    //        else CmdStartGame();
        
    //}

    private void Start()
    {
        _gameUI.OnStartGame();
    }
    public void AddPlayerNickName(string nickName)
    {
        PlayerNickNames.Add(nickName);
        Debug.Log($"Мы добавили по счету уже = {PlayerNickNames.Count}");
    }
    public void RemovePlayerNickName(string nickName)
    {
        PlayerNickNames.Remove(nickName);
    }

    public void GameOver()
    {
        Debug.Log("На клиенте сработал метод ГАмеовер");
        CmdGameOver();
    }
    [Command(requiresAuthority = false)]
    private void CmdGameOver()
    {
        Debug.Log("Сработала команда серверу, покажи кто выйграл");
        RpcShowWiner();
    }
    [ClientRpc]
    private void RpcShowWiner()
    {
        Debug.Log("Отправили всем клиентам вывести имя победителя");
        Debug.Log($"Имя победителя = {PlayerNickNames[0]}");
        _gameUI.OnShowWiner(PlayerNickNames[0]);
    }

    //[Command]
    //private void CmdStartGame()
    //{
    //    StartGame();
    //    Debug.Log("Сработал CMD");
    //}

    //[Server]
    //public void StartGame()
    //{
    //    RpcStartGame();
    //    Debug.Log("Сработал он Сурвер");
    //}
    //[ClientRpc]
    //private void RpcStartGame()
    //{
    //    _gameUI.OnStartGame();
    //    Debug.Log("Сработал RCP клиент");
    //}
}
