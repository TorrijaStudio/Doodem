using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.WebSockets;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.AI;

public class GameManager : NetworkBehaviour
{
    private NetworkManager _networkManager;
    private GameObject _playerPrefab;
    public static GameManager Instance;

    // Start is called before the first frame update
    void Start()
    {
        if(Instance)
            Destroy(this);
        else
        {
            Instance = this;
        }
        _networkManager = NetworkManager.Singleton;
        _playerPrefab = _networkManager.NetworkConfig.Prefabs.Prefabs[0].Prefab;
        _networkManager.OnServerStarted += OnServerStarted;
        _networkManager.OnClientConnectedCallback += OnClientConnected;
    }

    private void OnServerStarted()
    {
        print("Server ready");
    }
    
    [ServerRpc(RequireOwnership = false)]
    public void SpawnServerRpc(ulong obj, int prefab, Vector3 pos)
    {
        var player = Instantiate(NetworkManager.Singleton.NetworkConfig.Prefabs.Prefabs[prefab].Prefab, pos, Quaternion.identity);
        player.GetComponent<NetworkObject>().SpawnWithOwnership(obj);
        player.GetComponent<NavMeshAgent>().enabled = true;
        //player.GetComponent<Entity>().enabled = true;
       // player.GetComponent<Attack>().enabled = true;
        player.GetComponent<Entity>().SetAgent();
    }
    
    private void OnClientConnected(ulong obj)
    {
        Seleccionable.clientID = obj;
        // var player = Instantiate(_playerPrefab);
        // player.GetComponent<NetworkObject>().SpawnWithOwnership(obj);
    }

    public void OnDestroy()
    {
        _networkManager.OnServerStarted -= OnServerStarted;
        _networkManager.OnClientConnectedCallback -= OnClientConnected;
    }
}