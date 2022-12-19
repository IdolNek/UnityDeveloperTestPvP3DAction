using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundSystem : NetworkBehaviour
{
    [SerializeField] private Animator _couldDownAnimator;
    public void CountdownEnded()
    {
        _couldDownAnimator.enabled = false;
    }
    public override void OnStartServer()
    {
        NetworkLobbyManager.OnServerStopped += CleanUpServer;
        NetworkLobbyManager.OnServerReadied += StartCountdown;
    }
    [ServerCallback]
    private void OnDestroy() => CleanUpServer();
    [Server]
    private void CleanUpServer()
    {
        NetworkLobbyManager.OnServerStopped -= CleanUpServer;
        NetworkLobbyManager.OnServerReadied -= StartCountdown;
    }
    [Server]
    private void StartCountdown(NetworkConnection conn)
    {
        _couldDownAnimator.enabled = true;
        RpcStartCountdown();
    }
    [ClientRpc]
    private void RpcStartCountdown()
    {
        _couldDownAnimator.enabled = true;
    }

}
