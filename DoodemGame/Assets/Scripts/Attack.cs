using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Attack : MonoBehaviour
{
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
        Debug.Log("sldjhvbsdjhv s,hj");
        if(entity.GetIsOnGround())
        {
            Debug.LogError(currentObjective);
            Attackupdate();
        }
    }
    public void Attackupdate()
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
        else
        {
            currentObjective = objetive;
            agente.SetDestination(objetive.position);
        }
    }
}
