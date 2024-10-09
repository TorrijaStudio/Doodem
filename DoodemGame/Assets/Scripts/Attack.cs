using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Attack : MonoBehaviour
{
    public float angleAttack;
    
    private NavMeshAgent agente;

    private Entity entity;

    private Transform objetive;
    private Transform currentObjective;
    private float damage;
    private float attackDistance;
    private float attackSpeed;
    private float timeLastHit;
    
    // Start is called before the first frame update
    void Start()
    {
        agente = GetComponent<NavMeshAgent>();
        entity = GetComponent<Entity>();
        objetive = entity.objetive;
        currentObjective = objetive;
        damage = entity.damage;
        attackDistance = entity.attackDistance;
        attackSpeed = entity.attackSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        if(entity.GetIsOnGround())
        {
            AttackArea();   
        }
    }
    
    
    public void Attackupdate()
    {
        if (currentObjective == objetive)
        { 
            Collider[] hitColliders = Physics.OverlapSphere(agente.transform.position, attackDistance, LayerMask.GetMask("Enemy"));
            if (hitColliders.Length==0) return;
            foreach (var c in hitColliders)
            {
                if (c.gameObject != gameObject && gameObject.layer!=c.gameObject.layer)
                {
                    currentObjective = c.transform;
                    if(agente.enabled)
                        agente.SetDestination(currentObjective.position);
                    break;
                }
            }
        }
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
                    if(agente.enabled)
                        agente.SetDestination(objetive.position);
                    if (gameObject.TryGetComponent(out Aguila a))
                    {
                        a.AguilaKill();
                    }
                }
                timeLastHit = Time.time;
            }   
        }
        else
        {
            currentObjective = objetive;
            if(agente.enabled)
                agente.SetDestination(objetive.position);
        }
    }

    private void AttackArea()
    {
        Collider[] hitColliders = Physics.OverlapSphere(agente.transform.position, attackDistance, LayerMask.GetMask("Enemy"));
        Debug.Log("Overlaped with " + hitColliders.Length + "colliders");
        if (hitColliders.Length==0) return;
        foreach (var c in hitColliders)
        {
            if (c.gameObject != gameObject && gameObject.layer!=c.gameObject.layer)
            {
                currentObjective = c.transform;
                if(agente.enabled)  
                    agente.SetDestination(currentObjective.position);
                break;
            }
        }
        
        if (Time.time - timeLastHit >= 1f / attackSpeed)
        {
            foreach (var c in hitColliders)
            {
                float angle = Vector3.Angle(transform.forward, c.transform.position - transform.position);
                if(angle>angleAttack/2) continue;
                float aux = 0;
                if (c.TryGetComponent(out IAtackable m))
                {
                    aux = m.Attacked(damage);
                }
                if (aux < 0)
                {
                    Debug.Log(gameObject.name+"  "+objetive.position);
                    currentObjective = objetive;
                    if(agente.enabled)
                        agente.SetDestination(objetive.position);
                }
            }
                
            timeLastHit = Time.time;
        }
        
        if (!currentObjective)
        {
            currentObjective = objetive;
            if(agente.enabled)
                agente.SetDestination(objetive.position);
        }
    }

    public Transform GetCurrentObjetive()
    {
        return currentObjective;
    }
}
