using UnityEngine;
using Mirror;

public class NetworkLobbyManager : NetworkRoomManager
{
    public override bool OnRoomServerSceneLoadedForPlayer(NetworkConnectionToClient conn, GameObject roomPlayer, GameObject gamePlayer)
    {
        NetworkGamePlayer player = gamePlayer.GetComponent<NetworkGamePlayer>();
        player.Index = roomPlayer.GetComponent<NetworkRoomPlayer>().index;
        player.NickName = "Player" + (player.Index +1);
        RoundEvent.GetInstance().AddPlayerToGamePlayerList(player);
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
