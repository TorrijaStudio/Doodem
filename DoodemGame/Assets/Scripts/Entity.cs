using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

public class Entity : MonoBehaviour,IAtackable
{
    private NavMeshAgent agente;
    private float timeLastHit;
    private bool isOnGround;
    
    public Transform objetive;
    public float speed;
    public float health;
    public float damage;
    public float attackDistance;
    public float attackSpeed;

    void Start()
    {
        agente = GetComponent<NavMeshAgent>();
        //agente.SetDestination(objetive.position);
    }

    void Update()
    {
        
    }

   

    public float Attacked(float enemyDamage)
    {
        Debug.Log(gameObject.name +"-- "+health);
        health -= enemyDamage;
        if (health < 0)
        {
            Destroy(gameObject);
        }

        return health;
    }

    public void SetAgent()
    {
        isOnGround = true;
        agente = GetComponent<NavMeshAgent>();
        agente.speed = speed;
        agente.SetDestination(objetive.position);
    }

    public bool GetIsOnGround()
    {
        return isOnGround;
    }
}
