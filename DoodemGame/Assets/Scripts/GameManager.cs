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
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

public class GameManager : NetworkBehaviour
{
    private NetworkManager _networkManager;
    private GameObject _playerPrefab;
    private NetworkVariable<int> _id = new();
    private Dictionary<Vector2Int, GameObject> entidades = new();
    
    //public List<GameObject>[] playerObjects = new List<GameObject>[2];
    public static GameManager Instance;
    public GameObject terreno;
    public playerInfo[] players = new playerInfo[2];
    public ABiome[] biomasGame = new ABiome[5]; 
    public List<ABiome> biomasInMatch = new ();
    public int clientId;
    public List<Transform> Bases;
    public GameObject objectSelected;
   

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
        for(int i=0;i<2;i++)
        {
            //playerObjects[i] = new();
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            StartGame();
        }
        
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
       
            if (Physics.Raycast(ray, out hit, 100,LayerMask.GetMask("Rojo","Azul","casilla")))
            {
                objectSelected = hit.transform.gameObject;
                //DespawnServerRpc(hit.transform.gameObject.GetComponent<NetworkObject>(),default);
            }
            else
            {
                if (!EventSystem.current.IsPointerOverGameObject())
                {
                    objectSelected = null;
                }
            }
        }
    }

    public void StartGame()
    {
        //var idOtherPlayer = clientId == 0 ? 1 : 0;
        //Debug.LogError(clientId+" : "+idOtherPlayer);
        //for (int i = 0; i <playerObjects[idOtherPlayer].Count; i++)
        //{
        //    if(playerObjects[idOtherPlayer][i])
        //        playerObjects[idOtherPlayer][i].SetActive(true);
        //        
        //}

        //foreach (var p in playerObjects[idOtherPlayer])
        //{
        //    if(p)//cada entity y abiome se borre en ondestroy?
        //        p.SetActive(true);
        //}
        //Debug.LogError(entidades.Count);
        foreach (GameObject g in entidades.Values)
        {
            if(!g) continue;
            Debug.LogError(g.name);
            if (g.TryGetComponent(out Entity e))
            {
                //e.GetComponent<NavMeshAgent>().enabled = true;
                e.SetSpeed(e.speed);
            }
            
        }
        terreno.GetComponent<NavMeshSurface>().BuildNavMesh();
    }

    public void updateEntidades()
    {
        Debug.LogError(entidades.Count);
        foreach (GameObject g in entidades.Values)
        {
            if(!g) continue;
            Debug.LogError(g.name);
            if (g.TryGetComponent(out recurso r))
            {
                r.CheckIfItsInMyBiome();
            }else if (g.TryGetComponent(out obstaculo o))
            {
                o.CheckIfItsInMyBiome();
            }
        }
    }

    private void OnServerStarted()
    {
        print("Server ready");
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
        player.GetComponent<NetworkObject>().SpawnWithOwnership(players[playerId].obj);
        if (player.TryGetComponent(out NavMeshAgent nav))
        {
            nav.enabled = true;
            var posInGid = terreno.GetComponent<terreno>().PositionToGrid(pos);
            if (entidades.ContainsKey(posInGid) && entidades[posInGid])
            {
                Destroy(player);
            }
            //else
            //{
            //    AddPositionSomething(pos,player);
            //}
            
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

    [ServerRpc(RequireOwnership = false)]
    public void DespawnServerRpc(NetworkObjectReference target, ServerRpcParams serverRpcParams)
    {
        if (target.TryGet(out NetworkObject targetObject))
        {
            Destroy(targetObject.gameObject);
        }
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
    
    public void StartTime()
    {
        GameObject.Find("wall").GetComponent<wall>().enabled = true;
    }

    public override void OnDestroy()
    {
        _networkManager.OnServerStarted -= OnServerStarted;
        _networkManager.OnClientConnectedCallback -= OnClientConnected;
        base.OnDestroy();
    }
    
}