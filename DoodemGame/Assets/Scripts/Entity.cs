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
        SetLayer(0, _idPlayer.Value);
        _idPlayer.OnValueChanged += SetLayer; 
        SetAgent();
        StartCoroutine(SearchResources());
    }

    void Update()
    {
        
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
        Debug.LogError("objetivo sSET  " +d.name);
        agente.SetDestination(d.position);
    }
    
    
    public bool GetIsOnGround()
    {
        return isOnGround;
    }

    public override void OnDestroy()
    {
        _idPlayer.OnValueChanged -= SetLayer;
        base.OnDestroy();
    }

    public IEnumerator SearchResources()
    {
        //seleccion de bioma segun el bicho que seas:
        yield return new WaitUntil((() => agente.isOnNavMesh));
        var biomas = GameManager.Instance.biomas;
        Debug.LogError(biomas.Count);
        float minDistance = float.MaxValue;
        Transform o = null;
        foreach (bioma b in biomas)
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
}
