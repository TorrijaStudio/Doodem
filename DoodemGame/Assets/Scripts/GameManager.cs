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
    private NetworkVariable<int> _id = new(0);
    public int clientId;
    public List<Transform> Bases;
    public GameObject objectSelected;
    public List<GameObject> playerObjects = new List<GameObject>();
    public bool startedGame;
    private int numPlayers;
    public int numRondas;
    public int secondsBiome;
    public List<GameObject> entidatesPrueba = new();
    
    public List<Entity> enemies;
    public List<Entity> allies;
    public List<recurso> recs;
    private bool startMatchAfterTimer;

    [SerializeField] public GameObject[] _heads;
    [SerializeField] public GameObject[] _body;
    [SerializeField] public GameObject[] _feet;

    private List<Vector2> Positions = new ();//casillas disponibles
    private Dictionary<Vector2Int, GameObject> entidades = new();

    public GameObject gameCanvas;
    public Canvas storeCanvas;
    
    private playerInfoStore _store;
    
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

        gameCanvas.gameObject.SetActive(true);
        storeCanvas.gameObject.SetActive(false);
        _store = FindObjectOfType<playerInfoStore>(true);
        _terreno = terrenoGO.GetComponent<terreno>();
        _networkManager = NetworkManager.Singleton;
        _playerPrefab = _networkManager.NetworkConfig.Prefabs.Prefabs[0].Prefab;
        _networkManager.OnServerStarted += OnServerStarted;
        _networkManager.OnClientConnectedCallback += OnClientConnected;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            if(IsHost)
                checkIfRoundEnded("jk");
        }
        
        
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
   public void StartRoundClientRpc()
   {
       // if(IsServer)
       if(startMatchAfterTimer){
           startedGame = true;
           
           for (var index = 0; index < playerObjects.Count; index++)
           {
               var p = playerObjects[index];
               if (!p) continue;
               if (p.TryGetComponent(out ABiome ab))
               {
                  //ab.EnableMeshesRecursively(p);
                  //ab.SetColorsGridBiome();
                  //Debug.LogError(p.name);
                  StartCoroutine(StartBiome(ab, p));
                   //p.SetActive(true);
               }else
                   p.SetActive(true);
           }
           UpdateBiomeThings();
           _terreno.GetComponent<NavMeshSurface>().BuildNavMesh();
       }
       else
       {
           FindObjectOfType<playerInfoStore>().CloseShopAfterTimer();
           //mover camera a el tablero
           startMatchAfterTimer = true;
           
           FindObjectOfType<Inventory>().SpawnSeleccionables();
           FindObjectOfType<Inventory>().SpawnTotemsAsSeleccionables();
           gameCanvas.gameObject.SetActive(true);
           storeCanvas.gameObject.SetActive(false);
           startedGame = false;
           Debug.LogWarning("Empezando timer en ExecuteOnAllClients)");
           StartTime();
       }
    }
   
    private IEnumerator StartBiome(ABiome ab,GameObject p)
    {
        yield return new WaitForSeconds(secondsBiome);
        ab.EnableMeshesRecursively(p);
        ab.SetColorsGridBiome();
        StartCoroutine(ab.SetResourcesDespawn(secondsBiome*15));
        _terreno.GetComponent<NavMeshSurface>().BuildNavMesh();
    }
   
   [ClientRpc]
    public void EndRoundClientRpc(string winner)
    {
        if(winner==" ")
        {
            startedGame = false;
            gameCanvas.gameObject.SetActive(false);
            storeCanvas.gameObject.SetActive(true);
            startMatchAfterTimer = false;
            _store.InitialSelection();
            Debug.LogWarning("Empezando timer en StartRound (if)");
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
        {
            startedGame = false;
            startMatchAfterTimer = false;
            StartCoroutine(DelayToChangeCanvas());
            Debug.LogWarning("Empezando timer en StartRound (else)");
            StartTime();
        }
        
    }

    private IEnumerator DelayToChangeCanvas()
    {
        yield return new WaitForSeconds(2.0f);
        gameCanvas.gameObject.SetActive(false);
        storeCanvas.gameObject.SetActive(true);
        _store.SetUpShop(15);
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
    public void SpawnServerRpc(int playerId, int prefab, Vector3 pos, int head, int body, int feet)
    {
        var player = Instantiate(NetworkManager.Singleton.NetworkConfig.Prefabs.Prefabs[prefab].Prefab, pos, Quaternion.identity);
        // player.name = "AAAAAAAAAAACABO DE SPAWNEAR";
        Debug.LogWarning("ID: " + playerId);
       
        player.GetComponent<NetworkObject>().SpawnWithOwnership(players[playerId].obj);
        var entity = player.GetComponent<Entity>();
        var text = player.name;
        var par = player.transform;
        while (par.parent)
        {
            par = par.parent;
            text = text.Insert(0, par.name + "/");
        }
        Debug.LogWarning(text);
        if(entity)
        {
            entity.SpawnClientRpc(head, body, feet);
        }
        if (player.TryGetComponent(out NavMeshAgent nav))
        {
            
            nav.enabled = true;
            var posInGid = terrenoGO.GetComponent<terreno>().PositionToGrid(pos);
            if (entidades.ContainsKey(posInGid) && entidades[posInGid])
            {
                Debug.Log("Me fui");
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
                StartCoroutine(DespawnAllEntityRoundEnded(RedEnemies));
                EndRoundClientRpc("Rojo");
            }
            else
            {
                StartCoroutine(DespawnAllEntityRoundEnded(BlueEnemies));
                EndRoundClientRpc("Azul");
            }
            
            
        }
    }

    private IEnumerator DespawnAllEntityRoundEnded(List<Entity> lista)
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
    
    
    
    public void UpdateBiomeThings()
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
                Debug.Log("Scooby dooby do, who are you? ");
                Seleccionable.ClientID = _id.Value;
                Inventory.Instance.clientID = _id.Value;
                clientId = _id.Value;
                // Camera.main.enabled = false;
                GameObject.Find(clientId == 0 ? "Main Camera" : "Main Camera1").GetComponent<Camera>().gameObject.SetActive(false);
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
            {
                SetTerreainBiomeDefaultClientRpc(Random.Range(0,2));
                EndRoundClientRpc(" ");
                
            }
        } 
        // var player = Instantiate(_playerPrefab);
        // player.GetComponent<NetworkObject>().SpawnWithOwnership(obj);Debug.Log(_idPlayer);
    }

    [ClientRpc]
    private void SetTerreainBiomeDefaultClientRpc(int index)
    {
        Debug.Log(index+"kdsjfnksdjfs");
        var casillas = _terreno.casillas.Select((c)=> c.GetComponent<NavMeshModifier>());
        var biome = biomasGame[index];
        var material = biome.mat;
        var indexLayerArea = biome.indexLayerArea;
        foreach (var c in casillas)
        {
            c.area = indexLayerArea;
            c.GetComponent<MeshRenderer>().material = material;
        }
        _terreno.GetComponent<NavMeshSurface>().BuildNavMesh();
    }

    public override void OnDestroy()
    {
        _networkManager.OnServerStarted -= OnServerStarted;
        _networkManager.OnClientConnectedCallback -= OnClientConnected;
        base.OnDestroy();
    }
    
}