using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.WebSockets;
using Unity.AI.Navigation;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class GameManager : NetworkBehaviour
{
    private NetworkManager _networkManager;
    private GameObject _playerPrefab;
    public static GameManager Instance;
    public GameObject terreno;
    public playerInfo[] players = new playerInfo[2];
    public ABiome[] biomasGame = new ABiome[5]; 
    public List<ABiome> biomasInMatch = new ();
    private NetworkVariable<int> _id = new();
    public int clientId;
    public List<Transform> Bases;
    
    public List<Entity> enemies;
    public List<Entity> allies;
    public List<recurso> recs;

    [SerializeField] private GameObject[] _heads;
    [SerializeField] private GameObject[] _body;
    [SerializeField] private GameObject[] _feet;

    private List<Vector2> Positions = new ();//casillas disponibles
    private Dictionary<Vector2Int, GameObject> entidades = new();
    public float MaxDistance
    {
        get
        {
            var grid = terreno.GetComponent<terreno>().GetGrid();
            return grid.magnitude;
        }
        private set => MaxDistance = value;
    }

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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            StartGame();
        }
        if (Input.GetKeyDown(KeyCode.Y))
        {
            foreach (var VARIABLE in entidades.Keys)
            {
                if(entidades[VARIABLE])
                    Debug.LogError(VARIABLE+" : "+entidades[VARIABLE].name);
            }
        }
    }

    public void StartGame()
    {
        foreach (GameObject g in entidades.Values)
        {
            if(!g) continue;
            if (g.TryGetComponent(out recurso r))
            {
                r.CheckIfItsInMyBiome();
            }else if (g.TryGetComponent(out obstaculo o))
            {
                o.CheckIfItsInMyBiome();
            }
        }
        terreno.GetComponent<NavMeshSurface>().BuildNavMesh();
    }

    private void OnServerStarted()
    {
        print("Server ready");
    }

    public void AddPosition(Vector2 v)
    {
        Positions.Add(v);
    }

    public void AddPositionSomething(Vector3 p,GameObject o)
    {
        var v = terreno.GetComponent<terreno>().PositionToGrid(p);
        if(entidades.ContainsKey(v) && entidades[v])
            entidades[v].SetActive(false);
        entidades[v] = o;
    }
    
    
    [ServerRpc(RequireOwnership = false)]
    public void SpawnServerRpc(int playerId, int prefab, Vector3 pos)
    {
        var player = Instantiate(NetworkManager.Singleton.NetworkConfig.Prefabs.Prefabs[prefab].Prefab, pos, Quaternion.identity);
        var entity = player.GetComponent<Entity>();
        if(entity)
            entity.SetAnimalParts(_heads[Random.Range(0, _heads.Length)], _body[Random.Range(0, _body.Length)], _feet[Random.Range(0, _feet.Length)]);
        player.GetComponent<NetworkObject>().SpawnWithOwnership(players[playerId].obj);

        if (player.TryGetComponent(out NavMeshAgent nav))
        {
            
            nav.enabled = true;
            var posInGid = terreno.GetComponent<terreno>().PositionToGrid(pos);
            if (entidades.ContainsKey(posInGid) && entidades[posInGid])
            {
                Destroy(player);
            }
            else
            {
                AddPositionSomething(pos,player);
            }
            
            //player.GetComponent<NavMeshAgent>().enabled = true;
        }

        if (player.TryGetComponent(out Entity e))
        {
            e._idPlayer.Value = playerId;
           //var entity = player.GetComponent<Entity>();
           //entity._idPlayer.Value = playerId;
        }

        if (player.TryGetComponent(out ABiome b))
        {
            b._idPlayer.Value = playerId;
            biomasInMatch.Add(b);
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
                // Camera.main.enabled = false;
                GameObject.Find(clientId == 0 ? "Main Camera" : "Main Camera1").GetComponent<Camera>().enabled =
                    false;
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