using Mirror;
using TMPro;
using UnityEngine;

public class NetworkGamePlayer : NetworkBehaviour
{
    [SerializeField] private TextMeshProUGUI _nickName;
    [SyncVar] public string NickName;
    public int Index;
    public override void OnStartClient()
    {
        _nickName.text = NickName;
    }
    public override void OnStartLocalPlayer()
    {
        if (isServer) SpawnPlayer();
        else CmdSpawnPlayer();
    }
    [Command]
    private void CmdSpawnPlayer()
    {
        SpawnPlayer();
    }
    [Server]
    private void SpawnPlayer()
    {
        RoundEvent.GetInstance().SpawnPlayer(this);
    }
    public override void OnStopLocalPlayer()
    {
        Cursor.lockState = CursorLockMode.None;
        if (isServer) DisconnectAll();
        else CmdDisconnect();
    }
    [Server]
    private void DisconnectAll()
    {
        NetworkServer.DisconnectAll();
    }
    [Command]
    private void CmdDisconnect()
    {
        DisconnectAll();
    }
}
