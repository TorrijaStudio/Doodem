using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

public class Entity : MonoBehaviour,IAtackable
{
    private NavMeshAgent agente;
    private float timeLastHit;
    private Transform currentObjective;

    public Transform objetive;
    public float speed;
    public float health;
    public float damage;
    public float attackDistance;
    public float attackSpeed;

    void Start()
    {
        currentObjective = objetive;
        agente = GetComponent<NavMeshAgent>();
        //agente.SetDestination(objetive.position);
        
    }

    void Update()
    {
        
        if (currentObjective)
        {
            if (Vector3.Distance(agente.transform.position, currentObjective.position) > attackDistance)
            {
                if (agente.destination != currentObjective.position)
                {
                    //agente.SetDestination(currentObjective.position);
                }
            }
            Attack();
        }
    }

   public void Attack()
    {
        Collider[] hitColliders = Physics.OverlapSphere(agente.transform.position, attackDistance, LayerMask.GetMask("Enemy"));
        if (hitColliders.Length==0) return;
        foreach (var c in hitColliders)
        {
            if (c.gameObject != gameObject)
            {
                currentObjective = c.transform;
                agente.SetDestination(currentObjective.position);
                break;
            }
        }
        
        //Debug.Log(hitColliders[0].name);
        if (currentObjective)
        {
            if (Time.time - timeLastHit >= 1f / attackSpeed)
            {
                float aux = 0;
                if (currentObjective.TryGetComponent(out IAtackable m))
                {
                    aux = m.Attacked(damage);
                }
                
                if (aux < 0)
                {
                    Debug.Log(gameObject.name+"  "+objetive.position);
                    currentObjective = objetive;
                    agente.SetDestination(objetive.position);
                }
                timeLastHit = Time.time;
            }
            
        }
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
        agente = GetComponent<NavMeshAgent>();
        agente.speed = speed;
        agente.SetDestination(objetive.position);
    }
}
