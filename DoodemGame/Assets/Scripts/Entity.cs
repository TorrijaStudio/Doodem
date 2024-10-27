using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Animals.Interfaces;
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
    public List<Transform> objetives;
    private Dictionary<Recursos, int> _resources;
    
    private IAnimalHead _head;
    private AnimalBody _body;
    private IAnimalFeet _feet;
        
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
    
    public void SetAnimalParts(GameObject head, GameObject body, GameObject feet)
    {
        _body = Instantiate(body, transform).GetComponent<AnimalBody>();
        _head = Instantiate(head, _body.GetHeadAttachmentPoint().position, _body.GetHeadAttachmentPoint().rotation,
            transform).GetComponent<IAnimalHead>();
        _feet = Instantiate(feet, _body.GetFeetAttachmentPoint().position, _body.GetFeetAttachmentPoint().rotation,
            transform).GetComponent<IAnimalFeet>();
        var a = transform.GetChild(0).GetComponentsInChildren<MeshRenderer>();
        foreach (var meshRenderer in a)
        {
            meshRenderer.enabled = false;
        }
    }

    
    void Start()
    {        
        _resources = new Dictionary<Recursos, int>();
        SetLayer(0, _idPlayer.Value);
        currentDamage = damage;
        _idPlayer.OnValueChanged += SetLayer; 
        SetAgent();
        agente.speed = 1f;
        // StartCoroutine(SearchResources());
        StartCoroutine(Brain());
    }

    private IEnumerator Brain()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            ReevaluateSituation();
        }
    }
    void Update()
    {
        checkAreaAgent();
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
        agente = GetComponent<NavMeshAgent>();
        isOnGround = true;
        agente.speed = speed;

        StartCoroutine(SetDestination(objetive));
    }

    private IEnumerator SetDestination(Transform d)
    {
        yield return new WaitUntil((() => agente.isOnNavMesh));
        if (TryGetComponent(out IAttack a))
        {
            a.SetCurrentObjetive(d);
        }
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
    private void ReevaluateSituation()
    {
        var enemies = FindObjectsOfType<Entity>().Where((entity, i) => entity.layer == layerEnemy).Select(entity => entity.transform).ToList();
        var resources = FindObjectsOfType<recurso>().Where(recurso => !recurso.GetSelected()).Select((recurso =>recurso.transform)).ToList();
        if(resources.Count == 0 && enemies.Count == 0)  return;
        
        var values = new List<KeyValuePair<Transform, float>>();
        values.AddRange(_head.AssignValuesToEnemies(enemies));
        values.AddRange(_head.AssignValuesToResources(resources));
        values.AddRange(_body.AssignValuesToEnemies(enemies));
        values.AddRange(_body.AssignValuesToResources(resources));
        values.AddRange(_feet.AssignValuesToEnemies(enemies));
        values.AddRange(_feet.AssignValuesToResources(resources));
        // var keyValuePairs = values.OrderBy((f => f.Value));
        // var a = from entry in values orderby entry.Value descending select entry;
        values.Sort((kp, kp1) => (int)Mathf.CeilToInt((kp.Value - kp1.Value)*100));
        objetive = values.First().Key;
        agente.SetDestination(objetive.position);
        Debug.Log(objetive.name);
    }

    private void MergeDictionaries(ref List<KeyValuePair<Transform, float>> inDic, List<KeyValuePair<Transform, float>> joinDic)
    {
        inDic.AddRange(joinDic);
    }

    public override void OnDestroy()
    {
        _idPlayer.OnValueChanged -= SetLayer;
        base.OnDestroy();
    }
    public delegate void EmptyEvent();
    public delegate void ResourcesEvent(Recursos res, int n);
     
    public EmptyEvent OnDeath;
    public EmptyEvent OnKilledEnemy;
     
    public ResourcesEvent OnResourcesChanged;
     public int GetResources(Recursos res)
    {
        return _resources.TryGetValue(res, out var value) ? value : 0;
    }
     public void AddOrTakeResources(Recursos res, int n)
         {
             if (_resources.TryGetValue(res, out var value))
             {
                 if (value + n > 0)
                     _resources[res] = value + n;
                 else
                     _resources.Remove(res);
                 OnResourcesChanged.Invoke(res, Math.Max(n+value, 0));
             }
             else if(n > 0)
             {
                 _resources.Add(res, n);
                 OnResourcesChanged(res, n);
             }
         }
    public IEnumerator SearchResources()
    {
        //seleccion de bioma segun el bicho que seas:
        yield return new WaitUntil((() => agente.isOnNavMesh));
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
