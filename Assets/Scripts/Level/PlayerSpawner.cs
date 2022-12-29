using Mirror;
using System.Collections.Generic;
using Telepathy;
using UnityEngine;

public class PlayerSpawner : NetworkBehaviour
{
    [SerializeField] private List<Transform> _spawnPoints;
    public readonly SyncList<Transform> _syncSpawnPoints = new SyncList<Transform>();
    public readonly SyncList<Transform> _removedSpawnPoints = new SyncList<Transform>();
    public override void OnStartServer()
    {
        foreach (Transform point in _spawnPoints)
        {
            _syncSpawnPoints.Add(point);
        }
    }
    [Server]
    private void RemoveSpawnPoint(Transform point)
    {
        _syncSpawnPoints.Remove(point);
        _removedSpawnPoints.Add(point);
    }
    [Server]
    private void AddSpawnPoint(Transform point)
    {
        _syncSpawnPoints.Add(point);
        _removedSpawnPoints.Remove(point);
    }
    [Server]
    public void AddAllSpawnPointsFromRemovedList()
    {
        foreach (var point in _removedSpawnPoints)
        {
            AddSpawnPoint(point);
        }
    }
    [Server]
    public void SpawnPlayer(NetworkGamePlayer player)
    {
        Transform point = _syncSpawnPoints[Random.Range(0, _syncSpawnPoints.Count)];
        player.GetComponent<NetworkTransform>().RpcTeleport(point.position);
        RemoveSpawnPoint(point);
    }
}
