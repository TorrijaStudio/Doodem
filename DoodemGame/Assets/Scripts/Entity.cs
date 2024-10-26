using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

public class Entity : NetworkBehaviour ,IAtackable
{
    private NavMeshAgent agente;
    private bool isOnGround;
    public string layer;
    private playerInfo _playerInfo;
    
    public NetworkVariable<int> _idPlayer = new NetworkVariable<int>(writePerm:NetworkVariableWritePermission.Server);
    public string layerEnemy;
    private int currentAreaIndex;
    [SerializeField] private float currentDamage;

    // public int PlayerId
    // {
    //     get => _idPlayer.Value;
    //     set => _idPlayer.Value = value;
    // }
    
    public Transform objetive;
    public float speed;
    public float health;
    public float damage;
    public float attackDistance;
    public float attackSpeed;

    private void SetLayer(int oldId, int id)
    {
        gameObject.layer = LayerMask.NameToLayer(id == 0 ? "Rojo" : "Azul");
        layerEnemy = id == 0 ? "Azul" : "Rojo";
        layer = id == 0 ? "Rojo" : "Azul";
        var meshes = transform.GetComponentsInChildren<MeshRenderer>().ToList();
        meshes.ForEach(mesh =>
        {
            var materials = mesh.materials.ToList();
            materials.ForEach(mat => mat.color = id == 0 ? Color.red : Color.blue);
            mesh.SetMaterials(materials);
        });
        Debug.Log(id);
        objetive = GameManager.Instance.Bases[id];
    }
    
    void Start()
    {
        //GameManager.Instance.playerObjects[_idPlayer.Value].Add(gameObject);
        StartCoroutine(AddPosition());
        SetLayer(0, _idPlayer.Value);
        currentDamage = damage;
        _idPlayer.OnValueChanged += SetLayer; 
        SetAgent();
        //StartCoroutine(SearchResources());
        //if (GameManager.Instance.clientId != _idPlayer.Value)
        //{
        //    gameObject.SetActive(false);
        //}
    }

    void Update()
    {
        checkAreaAgent();
    }
    
    private IEnumerator AddPosition()
    {
        yield return new WaitForSeconds(1.0f);
        Debug.LogError("aladi");
        GameManager.Instance.AddPositionSomething(transform.position,gameObject);
    }
    
    public float Attacked(float enemyDamage)
    {
        Debug.Log(gameObject.name + " -- "+health);
        health -= enemyDamage;
        if (health < 0)
        {
            Destroy(gameObject);
        }

        return health;
    }

    private void SetAgent()
    {
        Debug.LogError("me llamo");
        agente = GetComponent<NavMeshAgent>();
        //agente.enabled = true;
        isOnGround = true;
        //agente.speed = speed;
        
        StartCoroutine(SetDestination(objetive));
    }

    public IEnumerator SetDestination(Transform d)
    {
        yield return new WaitUntil((() => agente.isOnNavMesh && gameObject.activeSelf));
        Debug.LogError("HOLAAA");
        if (TryGetComponent(out IAttack a))
        {
            a.SetCurrentObjetive(d);
        }
        Debug.LogError(d.position+" me muevo aqui");
        agente.SetDestination(d.position);
    }

    #region getters and setters

    public bool GetIsOnGround()
    {
        return isOnGround;
    }

    public float GetCurrentDamage()
    {
        return currentDamage;
    }

    public void SetCurrentDamage(float v)
    {
        currentDamage = v;
    }

    public void SetSpeed(float s)
    {
        agente.speed = s < 1 ? 1 : s;
    }
    

    public float GetSpeed()
    {
        return agente.speed;
    }
    #endregion
    

    public override void OnDestroy()
    {
        //GameManager.Instance.playerObjects[_idPlayer.Value].Remove(gameObject);
        _idPlayer.OnValueChanged -= SetLayer;
        base.OnDestroy();
    }

    public IEnumerator SearchResources()
    {
        //seleccion de bioma segun el bicho que seas:
        yield return new WaitUntil((() => agente.isOnNavMesh && gameObject.activeSelf));
        var biomas = GameManager.Instance.biomasInMatch;
        float minDistance = float.MaxValue;
        Transform o = null;
        foreach (ABiome b in biomas)
        {
            foreach (Transform r in b.GetRecursos())
            {
                float d = Vector3.Distance(transform.position, r.position);
                if (minDistance > d && !r.GetComponent<recurso>().GetSelected())
                {
                    minDistance = d;
                    o = r;
                }
            }
        }

        if (o)
        {
            o.GetComponent<recurso>().SetSelected(true);
            StartCoroutine(SetDestination(o));
        }
    }

    #region biomas

    private void checkAreaAgent()
    {
        NavMeshHit hit;
        
        if (NavMesh.SamplePosition(agente.transform.position, out hit, 1.0f, NavMesh.AllAreas))
        {
            for (int areaIndex = 0; areaIndex < 5; areaIndex++)
            {
                if ((hit.mask & (1 << areaIndex)) != 0)
                {
                    if (currentAreaIndex != areaIndex)
                    {
                        OnEnterArea(areaIndex,currentAreaIndex);
                        currentAreaIndex = areaIndex;
                    }
                    break; 
                }
            }
        }
    }

    private void OnEnterArea(int index,int prevBiome)
    {
        if (prevBiome != 0)//bioma 0 es el walkable, no hay que deshacer efectos
        {
            var prevBioma = GameManager.Instance.biomasGame[prevBiome];
            prevBioma.LeaveBiome(gameObject);
        }
        
        var bioma = GameManager.Instance.biomasGame[index];
        if(bioma)
            bioma.ActionBioma(gameObject);
    }

    #endregion
}
