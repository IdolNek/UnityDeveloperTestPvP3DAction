using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;
using UnityEngine.SceneManagement;

public class NetworkLobbyManager : NetworkRoomManager
{
    [SerializeField] private GameObject _roundSystem;
    //[SerializeField] private GameObject _roundEvent;
    public List<Player> Players = new List<Player>();
    public static event Action OnServerStopped;
    public static event Action<NetworkConnection> OnServerReadied;

    //public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    //{
    //    base.OnServerAddPlayer(conn);
    //    Player player = conn.identity.GetComponent<Player>();
    //    Players.Add(player);
    //}










    /// <summary>
    /// This is called on the server when a networked scene finishes loading.
    /// </summary>
    /// <param name="sceneName">Name of the new scene.</param>
    public override void OnRoomServerSceneChanged(string sceneName)
    {
        if (sceneName == GameplayScene)
        {
            GameObject roundSystemInstance = Instantiate(_roundSystem);
            NetworkServer.Spawn(roundSystemInstance);
            //GameObject roundEventInstance = Instantiate(_roundEvent);
            //NetworkServer.Spawn(roundEventInstance);
            //NetworkServer.
            //RoundEvent.GetInstance().StartGame();
            //Debug.Log("��������� �����")�
        }       
    }

    /// <summary>
    /// Called just after GamePlayer object is instantiated and just before it replaces RoomPlayer object.
    /// This is the ideal point to pass any data like player name, credentials, tokens, colors, etc.
    /// into the GamePlayer object as it is about to enter the Online scene.
    /// </summary>
    /// <param name="roomPlayer"></param>
    /// <param name="gamePlayer"></param>
    /// <returns>true unless some code in here decides it needs to abort the replacement</returns>
    public override bool OnRoomServerSceneLoadedForPlayer(NetworkConnectionToClient conn, GameObject roomPlayer, GameObject gamePlayer)
    {
        Player player = gamePlayer.GetComponent<Player>();
        player.Index = roomPlayer.GetComponent<NetworkRoomPlayer>().index;
        player.NickName = "Player " + (player.Index +1).ToString();
        Players.Add(player);
        RoundEvent.GetInstance().AddPlayerNickName(player.NickName);
        Debug.Log($"�� ��� ������ ���� ������ ��� ����� � ������� ������� {Players.Count} ");
        return true;
    }

    public override void OnRoomStopClient()
    {
        base.OnRoomStopClient();
    }

    public override void OnRoomStopServer()
    {
        base.OnRoomStopServer();
        OnServerStopped?.Invoke();
    }

    /*
        This code below is to demonstrate how to do a Start button that only appears for the Host player
        showStartButton is a local bool that's needed because OnRoomServerPlayersReady is only fired when
        all players are ready, but if a player cancels their ready state there's no callback to set it back to false
        Therefore, allPlayersReady is used in combination with showStartButton to show/hide the Start button correctly.
        Setting showStartButton false when the button is pressed hides it in the game scene since NetworkRoomManager
        is set as DontDestroyOnLoad = true.
    */

    bool showStartButton;

    public override void OnRoomServerPlayersReady()
    {
        // calling the base method calls ServerChangeScene as soon as all players are in Ready state.
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
            // set to false to hide it in the game scene
            showStartButton = false;

            ServerChangeScene(GameplayScene);
        }
    }
    public override void OnServerReady(NetworkConnectionToClient conn)
    {
        Debug.Log("������ �����");
        base.OnServerReady(conn);
        OnServerReadied?.Invoke(conn);

    }
}
