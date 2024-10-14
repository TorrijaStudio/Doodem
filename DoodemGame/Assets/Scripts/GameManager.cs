using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.WebSockets;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class GameManager : NetworkBehaviour
{
    private NetworkManager _networkManager;
    private GameObject _playerPrefab;
    public static GameManager Instance;
    public playerInfo[] players = new playerInfo[2];
    public List<bioma> biomas = new ();
    private NetworkVariable<int> _id = new();
    public int clientId;
    public List<Transform> Bases;

   

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
    public void SpawnServerRpc(int playerId, int prefab, Vector3 pos)
    {
        var player = Instantiate(NetworkManager.Singleton.NetworkConfig.Prefabs.Prefabs[prefab].Prefab, pos, Quaternion.identity);
        player.GetComponent<NetworkObject>().SpawnWithOwnership(players[playerId].obj);
        if (player.TryGetComponent(out NavMeshAgent nav))
        {
            nav.enabled = true;
            //player.GetComponent<NavMeshAgent>().enabled = true;
        }

        if (player.TryGetComponent(out Entity e))
        {
            e._idPlayer.Value = playerId;
           //var entity = player.GetComponent<Entity>();
           //entity._idPlayer.Value = playerId;
        }

        if (player.TryGetComponent(out bioma b))
        {
            b._idPlayer.Value = playerId;
            biomas.Add(b);
        }
        
        //player.GetComponent<Entity>().enabled = true;
       // player.GetComponent<Attack>().enabled = true;
       // entity./
       // entity.SetAgent();
       Debug.Log("Spawning entity with id " + playerId);
    }
    
    private void OnClientConnected(ulong obj)
    {
        
        if(IsClient)
        {
            if(Seleccionable.ClientID == -1)
            {
                Seleccionable.ClientID = _id.Value;
                clientId = _id.Value;
            }
        }
        if (IsServer)
        {
            var player = Instantiate(NetworkManager.Singleton.NetworkConfig.Prefabs.Prefabs[1].Prefab);
            player.GetComponent<NetworkObject>().SpawnWithOwnership(obj);
            var playerInfo = player.GetComponent<playerInfo>();
            playerInfo.PlayerId = _id.Value;
            playerInfo.obj = obj;
            // players[id] = playerInfo;
            _id.Value++;
        } 
        // var player = Instantiate(_playerPrefab);
        // player.GetComponent<NetworkObject>().SpawnWithOwnership(obj);Debug.Log(_idPlayer);
    }

    public override void OnDestroy()
    {
        _networkManager.OnServerStarted -= OnServerStarted;
        _networkManager.OnClientConnectedCallback -= OnClientConnected;
        base.OnDestroy();
    }
    
}