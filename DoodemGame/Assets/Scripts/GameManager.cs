using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using Unity.AI.Navigation;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class GameManager : NetworkBehaviour
{
    private NetworkManager _networkManager;
    private GameObject _playerPrefab;
    public static GameManager Instance;
    public GameObject terrenoGO;
    private terreno _terreno;
    public playerInfo[] players = new playerInfo[2];
    public ABiome[] biomasGame = new ABiome[5]; 
    public List<ABiome> biomasInMatch = new ();
    private NetworkVariable<int> _id = new();
    public int clientId;
    public List<Transform> Bases;
    public GameObject objectSelected;
    public List<GameObject> playerObjects = new List<GameObject>();
    public bool startedGame;
    private int numPlayers;
    public int numRondas;
    
    public List<Entity> enemies;
    public List<Entity> allies;
    public List<recurso> recs;

    [SerializeField] public GameObject[] _heads;
    [SerializeField] public GameObject[] _body;
    [SerializeField] public GameObject[] _feet;

    private List<Vector2> Positions = new ();//casillas disponibles
    private Dictionary<Vector2Int, GameObject> entidades = new();
    public List<GameObject> entidatesPrueba = new();
    public float MaxDistance
    {
        get
        {
            var grid = _terreno.GetGrid();
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

        _terreno = terrenoGO.GetComponent<terreno>();
        _networkManager = NetworkManager.Singleton;
        _playerPrefab = _networkManager.NetworkConfig.Prefabs.Prefabs[0].Prefab;
        _networkManager.OnServerStarted += OnServerStarted;
        _networkManager.OnClientConnectedCallback += OnClientConnected;
    }

    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.L) && IsHost)
        //{
        //    ExecuteOnAllClientsClientRpc();
        //}
        //if (Input.GetKeyDown(KeyCode.Y))
        //{
        //    foreach (var VARIABLE in entidades.Keys)
        //    {
        //        if(entidades[VARIABLE])
        //            Debug.LogError(VARIABLE+" : "+entidades[VARIABLE].name);
        //    }
        //}
        //if (!startedGame && Input.GetMouseButtonDown(0))
        //{
        //    Ray rayo = Camera.main.ScreenPointToRay(Input.mousePosition);
        //    RaycastHit hit;
        //    if (Physics.Raycast(rayo, out hit,30,LayerMask.GetMask("Rojo","Azul","casilla")))
        //    {
        //        objectSelected = hit.collider.gameObject;
        //    }
        //    else
        //    {
        //        if (!EventSystem.current.IsPointerOverGameObject())
        //        {
        //            objectSelected = null;
        //        }
        //    }
        //}
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
        terrenoGO.GetComponent<NavMeshSurface>().BuildNavMesh();
    }
    
    [ClientRpc]
   public void ExecuteOnAllClientsClientRpc()
   {
       startedGame = true;
       Debug.LogError(players.Length);
       for (var index = 0; index < playerObjects.Count; index++)
       {
           var p = playerObjects[index];
           if (p.TryGetComponent(out ABiome ab))
           {
               ab.EnableMeshesRecursively(p);
               ab.SetColorsGridBiome();
               Debug.LogError(p.name);
               //p.SetActive(true);
           }else
               p.SetActive(true);
       }

       updateEntidades();
       _terreno.GetComponent<NavMeshSurface>().BuildNavMesh();
       //StartCoroutine(updateEntidades());
   }

   [ClientRpc]
    public void StartRoundClientRpc(string winner)
    {
        if(winner==" ")
        {
            StartTime();
            return;
        }
        startedGame = false;
        numRondas--;
        if (numRondas == 0)
        {
            //if (IsHost)
            //{
            //    if (winner == "Rojo")
            //    {
            //        SceneManager.LoadScene("victory");
            //    }
            //    else
            //    {
            //        SceneManager.LoadScene("defeat");
            //    }
            //}
            //else
            //{
            //    if (winner == "Rojo")
            //    {
            //        SceneManager.LoadScene("defeat");
            //    }
            //    else
            //    {
            //        SceneManager.LoadScene("victory");
            //    }
            //}
            //SceneManager.LoadScene((IsHost == (winner == "Rojo")) ? "victory" : "defeat");
            StartCoroutine(ChangeScene((IsHost == (winner == "Rojo")) ? "victory" : "defeat"));
        }
        else
            StartTime();
        
    }

    private IEnumerator ChangeScene(string s)
    {
        yield return new WaitForSeconds(3.0f);
        SceneManager.LoadScene(s);
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
        //var v = terrenoGO.GetComponent<terreno>().PositionToGrid(p);
        //if(entidades.ContainsKey(v) && entidades[v])
        //    entidades[v].SetActive(false);
        //entidades[v] = o;
    }

    [ServerRpc(RequireOwnership = false)]
    public void SpawnRecursoServerRpc(int resourceType, Vector3 pos)
    {
        var resource = Instantiate(NetworkManager.Singleton.NetworkConfig.Prefabs.Prefabs[4].Prefab, pos, Quaternion.identity);
        resource.GetComponent<NetworkObject>().Spawn();
        transform.SetParent(GameObject.Find("RECURSOS").transform);
        
    }
    
    [ServerRpc(RequireOwnership = false)]
    public void SpawnServerRpc(int playerId, int prefab, Vector3 pos)
    {
        var player = Instantiate(NetworkManager.Singleton.NetworkConfig.Prefabs.Prefabs[prefab].Prefab, pos, Quaternion.identity);
       
        player.GetComponent<NetworkObject>().SpawnWithOwnership(players[playerId].obj);
        var entity = player.GetComponent<Entity>();
        if(entity)
        {
        //     SetAnimalParts(GameManager.Instance._heads[Random.Range(0, GameManager.Instance._heads.Length)], 
        //         GameManager.Instance._body[Random.Range(0, GameManager.Instance._body.Length)], 
        //         GameManager.Instance._feet[Random.Range(0, GameManager.Instance._feet.Length)]);
            entity.SpawnClientRpc(Random.Range(0, GameManager.Instance._heads.Length), Random.Range(0, GameManager.Instance._body.Length), Random.Range(0, GameManager.Instance._feet.Length));
        }
        if (player.TryGetComponent(out NavMeshAgent nav))
        {
            
            nav.enabled = true;
            var posInGid = terrenoGO.GetComponent<terreno>().PositionToGrid(pos);
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
            //var numRecursos = b.transform.GetChild(1).childCount;
            //b.indexTypeRecursos = new int[numRecursos];
            //for (int i = 0; i < numRecursos; i++)
            //{
            //    b.indexTypeRecursos[i] = Random.Range(0, b.typeResource.Length - 1);
            //}
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
    
    [ServerRpc]
    public void GenerateRandomNumberServerRpc(int maxValue,NetworkObjectReference target,int posHijo)
    {
        int generatedNumber = Random.Range(0, maxValue); 
        SendRandomNumberClientRpc(generatedNumber,target,posHijo);
    }

    [ClientRpc]
    public void SendRandomNumberClientRpc(int number,NetworkObjectReference target,int posHijo)
    {
        if (target.TryGet(out NetworkObject targetObject))
        {
            var r = targetObject.transform.GetChild(1).GetChild(posHijo);
            var typeResource = targetObject.GetComponent<ABiome>().typeResource;
            var recurso = r.GetComponent<recurso>();
            recurso._typeRecurso = typeResource[number];
            var mesh = r.GetComponent<MeshRenderer>();
            switch (recurso._typeRecurso)
            {
                case Recursos.Arbol:
                    mesh.material.color = Color.magenta;
                    break;
                case Recursos.Hierba:
                    mesh.material.color = Color.black;
                    break;
                case Recursos.Nido:    
                    mesh.material.color = Color.white;
                    break;
                case Recursos.Piedra:
                    mesh.material.color = Color.gray;
                    break;
                case Recursos.Arena:
                    mesh.material.color = Color.green;
                    break;
                case Recursos.Hielo:
                    mesh.material.color = Color.blue;
                    break;
                case Recursos.Agua:
                    mesh.material.color = Color.yellow;
                    break;
            }
        }
        
    }

    public void StartTime()
    {
        var w = GameObject.Find("Canvas").transform.GetChild(2).GetComponent<wall>();
        w.enabled = true;
        w.StartTimer();

    }
    public void RemoveEntity(Vector3 pos)
    {
        entidades.Remove(_terreno.PositionToGrid(pos));
    }

    public void checkIfRoundEnded(string layer)
    {
        var RedEnemies = FindObjectsOfType<Entity>().Where((entity, i) => entity.layer == "Rojo").ToList();
        var BlueEnemies = FindObjectsOfType<Entity>().Where((entity, i) => entity.layer == "Azul").ToList();
        if (layer == "Rojo")
            RedEnemies.RemoveAt(0);
        else
            BlueEnemies.RemoveAt(0);
        
        if (RedEnemies.Count == 0 || BlueEnemies.Count == 0)
        {
            if (RedEnemies.Count > 0)
            {
                StartCoroutine(RoundEnded(RedEnemies));
                StartRoundClientRpc("Rojo");
            }
            else
            {
                StartCoroutine(RoundEnded(BlueEnemies));
                StartRoundClientRpc("Azul");
            }
            
            
        }
    }

    private IEnumerator RoundEnded(List<Entity> lista)
    {
        yield return new WaitForSeconds(2.0f);
        foreach (var r in lista)
        {
            if (r.TryGetComponent(out NetworkObject n))
            {
                DespawnServerRpc(n, default);
            }
        }
    }
    
    
    
    public void updateEntidades()
    {
        // Debug.LogError(entidades.Count);
        //foreach (GameObject g in entidades.Values)
        //{
        //        if(!g) continue;
        //    // Debug.LogError(g.name);
        //    // Debug.LogError(g.name);
        //    if (g.TryGetComponent(out recurso r))
        //    {
        //        r.CheckIfItsInMyBiome();
        //    }else if (g.TryGetComponent(out obstaculo o))
        //    {
        //        o.CheckIfItsInMyBiome();
        //    }
        //}
        foreach (var e in entidatesPrueba)
        {
           if(!e) continue;
           if (e.TryGetComponent(out recurso r))
           {
               r.CheckIfItsInMyBiome();
           }else if (e.TryGetComponent(out obstaculo o))
           {
               o.CheckIfItsInMyBiome();
           }
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
            if (_id.Value == 2)
                StartRoundClientRpc(" ");
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