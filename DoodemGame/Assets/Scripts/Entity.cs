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
    private float timeLastHit;
    private bool isOnGround;
    private string layer;
    private bool fly;
    private playerInfo _playerInfo;
    
    public NetworkVariable<int> _idPlayer = new NetworkVariable<int>(writePerm:NetworkVariableWritePermission.Server);

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
        var meshes = transform.GetComponentsInChildren<MeshRenderer>().ToList();
        meshes.ForEach(mesh =>
        {
            var materials = mesh.materials.ToList();
            materials.ForEach(mat => mat.color = id == 0 ? Color.red : Color.blue);
            mesh.SetMaterials(materials);
        });
    }
    

    void Start()
    {
        // PlayerId = Seleccionable.ClientID;
        SetLayer(0, _idPlayer.Value);
        _idPlayer.OnValueChanged += SetLayer; 
        agente = GetComponent<NavMeshAgent>();
        // SetAgent();
        //if (id == 0) 
        //    gameObject.layer = LayerMask.NameToLayer("Rojo");
        //else 
        //    gameObject.layer = LayerMask.NameToLayer("Azul");
        //agente.SetDestination(objetive.position);
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

    public void SetAgent()
    {
        if(!agente) return;
        agente.SetDestination(objetive.position);
        isOnGround = true;
        agente = GetComponent<NavMeshAgent>();
        agente.speed = speed;
        agente.SetDestination(objetive.position);
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
}
