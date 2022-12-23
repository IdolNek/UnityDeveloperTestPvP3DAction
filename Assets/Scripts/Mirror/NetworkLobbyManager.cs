using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;

public class NetworkLobbyManager : NetworkRoomManager
{
    private static NetworkLobbyManager _instance;
    public override void Start()
    {
        base.Start();
        _instance = this;
    }
    public static NetworkLobbyManager GetInstance()
    {
        return _instance;
    }
    public List<NetworkGamePlayer> Players = new List<NetworkGamePlayer>();
    public override bool OnRoomServerSceneLoadedForPlayer(NetworkConnectionToClient conn, GameObject roomPlayer, GameObject gamePlayer)
    {
        NetworkGamePlayer player = gamePlayer.GetComponent<NetworkGamePlayer>();
        player.Index = roomPlayer.GetComponent<NetworkRoomPlayer>().index;
        player.NickName = "Player " + (player.Index +1).ToString();
        Players.Add(player);
        RoundEvent.GetInstance().AddPlayerNickName(player.NickName);
        return true;
    }
    bool showStartButton;
    public override void OnRoomServerPlayersReady()
    {

#if UNITY_SERVER
            base.OnRoomServerPlayersReady();
#else
        showStartButton = true;
#endif
    }
    public override void OnGUI()
    {
        base.OnGUI();
        if (allPlayersReady && showStartButton && GUI.Button(new Rect(150, 300, 120, 20), "START GAME"))
        {
            showStartButton = false;
            ServerChangeScene(GameplayScene);
        }
    }
}
