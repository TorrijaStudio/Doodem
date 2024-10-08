using System.Collections;
using System.Collections.Generic;
using Unity.Services.Relay.Models;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class Aguila : MonoBehaviour
{
    public float height;
    public float flySpeed;
    public bool fly;
    public float xTime = 4f;
    
    private NavMeshAgent agente;
    private Entity _entity;
    private Attack _attack;
    private float landHeight;
   
   
    // Start is called before the first frame update
    void Start()
    {
        _entity = GetComponent<Entity>();
        agente = GetComponent<NavMeshAgent>();
        _attack = GetComponent<Attack>();
        landHeight = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        if (fly)
        {
            agente.enabled = false;
            flyUpdate();
        }
        else
        {
            land();
        }
    }

    private void flyUpdate()
    {
        if (transform.position.y < height)
        {
            gameObject.transform.Translate(Vector3.up* (Time.deltaTime * flySpeed));
            return;
        }

        if ((transform.position - _attack.GetCurrentObjetive().position).magnitude > _entity.attackDistance)
        {
            Vector3 dir = _attack.GetCurrentObjetive().position - transform.position;
            dir.Normalize();
            transform.Translate(dir* (Time.deltaTime * flySpeed),Space.World);
        }
        
    }

    private void land()
    {
        fly = false;
        if (transform.position.y > landHeight)
        {
            Vector3 dir = _attack.GetCurrentObjetive().position -transform.position;
            dir -= new Vector3(dir.x, 0, dir.z)*0.75f;//reducir el tiempo de caida
            dir.Normalize();
            gameObject.transform.Translate(dir* (Time.deltaTime * flySpeed),Space.World);
        }
        else
        {
            fly = false;
            agente.enabled = true;
            agente.SetDestination(_attack.GetCurrentObjetive().position);
        }
    }

    public void AguilaKill()
    {
        Collider[] hitColliders = Physics.OverlapSphere(agente.transform.position, _entity.attackDistance);
        List<Collider> allys = new List<Collider>();
            Debug.LogError("uhdvb");
        foreach (var c in hitColliders)
        {
            if (c.gameObject.layer == gameObject.layer)
            {
                
                c.GetComponent<Aguila>().fly = true;
                allys.Add(c);
            }
        }
        StartCoroutine(LandAllys(allys));
    }

    private IEnumerator LandAllys(List<Collider> c)
    {
        Debug.LogError("uhdvb");
        yield return new WaitForSeconds(0.2f);
        foreach (var VARIABLE in c)
        {
            Debug.LogError("wow");
            VARIABLE.GetComponent<Aguila>().land();
        }
    }
    
}
