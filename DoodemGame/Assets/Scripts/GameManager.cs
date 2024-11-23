using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using formulas;
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

    public Reward reward = new (20,5,1.5F);
    private int _victoryRojoPoints;
    private int _victoryAzulPoints;
    private int currentRound;
   
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
        
    }

    public Action OnStartMatch;
    
    [ClientRpc]
   public void StartRoundClientRpc()
   {
       if (startedGame)
       {
           var rojos = FindObjectsOfType<Entity>().Where(entity => entity.layer =="Rojo").Sum(entity=> entity.health);
           var azules = FindObjectsOfType<Entity>().Where(entity => entity.layer =="Azul").Sum(entity=> entity.health);
           StartCoroutine(DespawnAllEntityRoundEnded(FindObjectsOfType<Entity>().ToList()));
           if(rojos > azules)
               EndRoundClientRpc("Rojo");
           else if (azules>rojos)
               EndRoundClientRpc("Azul");
           else
               EndRoundClientRpc("empate");
           return;
       }
       // if(IsServer)
       if(startMatchAfterTimer){
           startedGame = true;
           StartTime(25);
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
           OnStartMatch.Invoke();
       }
       else
       {
           FindObjectOfType<playerInfoStore>().CloseShopAfterTimer();
           //mover camera a el tablero
           startMatchAfterTimer = true;
           
           FindObjectOfType<Inventory>().SpawnSeleccionables();
           // FindObjectOfType<Inventory>().SpawnTotemsAsSeleccionables();
           gameCanvas.gameObject.SetActive(true);
           storeCanvas.gameObject.SetActive(false);
           startedGame = false;
           Debug.LogWarning("Empezando timer en ExecuteOnAllClients)");
           StartTime(15);
       }
    }
   
    private IEnumerator StartBiome(ABiome ab,GameObject p)
    {
        yield return new WaitForSeconds(secondsBiome);
        ab.EnableMeshesRecursively(p);
        ab.SetColorsGridBiome();
        StartCoroutine(ab.SetResourcesDespawn(secondsBiome));
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
            StartTime(30);
            return;
        }

        StopTime();
        startedGame = false;
        currentRound++;
       // if (currentRound==numRondas)
       // {
       //     //StartCoroutine(ChangeScene((IsHost == (winner == "Rojo")) ? "victory" : "defeat"));
       //     Debug.LogError("victoria: "+_victoryRojoPoints+" : "+_victoryAzulPoints);
       //     StartCoroutine(ChangeScene(IsHost == (_victoryRojoPoints > _victoryAzulPoints) ? "victory" : "defeat"));
       // }
        //else
        //{
            int gains = reward.GetReward(currentRound);
            if (winner == "Rojo")
            {
                _victoryRojoPoints++;
            }else if (winner == "Azul")
            {
                _victoryAzulPoints++;
            }
            else
            {
                _victoryAzulPoints++;
                _victoryRojoPoints++;
            }

            if (Math.Abs(_victoryAzulPoints - _victoryRojoPoints) == 2 || currentRound == numRondas)
            {
                Debug.LogError("victoria: "+_victoryRojoPoints+" : "+_victoryAzulPoints);
                if (_victoryAzulPoints == _victoryRojoPoints)
                {
                    StartCoroutine(ChangeScene("empate"));
                    return;
                }
                StartCoroutine(ChangeScene(IsHost == (_victoryRojoPoints > _victoryAzulPoints) ? "victory" : "defeat"));
                return;
            }
            startedGame = false;
            startMatchAfterTimer = false;
            StartCoroutine(DelayToChangeCanvas(gains));
            Debug.LogWarning("Empezando timer en StartRound (else)");
            StartTime(10);
       // }
        
    }

    private IEnumerator DelayToChangeCanvas(int moneyGained)
    {
        yield return new WaitForSeconds(2.0f);
        gameCanvas.gameObject.SetActive(false);
        storeCanvas.gameObject.SetActive(true);
        Debug.Log("Ganas: "+moneyGained);
        _store.SetUpShop(moneyGained);
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
    
    public void AddPositionSomething(Vector3 p,GameObject o)
    {
        //var v = terrenoGO.GetComponent<terreno>().PositionToGrid(p);
        //if(entidades.ContainsKey(v) && entidades[v])
        //    entidades[v].SetActive(false);
        //entidades[v] = o;
    }

    
    [ServerRpc(RequireOwnership = false)]
    public void SpawnServerRpc(int playerId, int prefab, Vector3 pos, int head, int body, int feet)
    {
        // Debug.Log($"PREFAB: {prefab} y maximo {NetworkManager.Singleton.NetworkConfig.Prefabs.Prefabs.Count}");
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

    public void StartTime(int time)
    {
        var w = GameObject.Find("Canvas").transform.GetChild(2).GetComponent<wall>();
        w.enabled = true;
        w.StartTimer(time);

    }

    public void StopTime()
    {
        var w = GameObject.Find("Canvas").transform.GetChild(2).GetComponent<wall>();
        w.enabled = true;
        w.StopTimer();
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