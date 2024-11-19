using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Animals.Interfaces;
using Totems;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

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
    private bool isEnemy;
    private Coroutine _followCoroutine;
    public bool isFlying;

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
    public float maxAttackDistance;

    private float _speedModifier;
    public float SpeedModifier
    {
        get => _speedModifier;
        set
        {
            _speedModifier = value;
            if(agente)
            {
                agente.speed = Math.Max(speed + _speedModifier, 1) / 5f;
                Debug.Log(agente.speed);
            }
        }
    }

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
        SetHealthAndSpeed(_body.Health,_body.Speed);
        
        _head = Instantiate(head, _body.GetHeadAttachmentPoint().position, _body.GetHeadAttachmentPoint().rotation,
            transform).GetComponent<IAnimalHead>();
        SetHealthAndSpeed(_head.Health,_head.Speed);
        damage = _head.Damage;
        
        _feet = Instantiate(feet, _body.GetFeetAttachmentPoint().position, _body.GetFeetAttachmentPoint().rotation,
            transform).GetComponent<IAnimalFeet>();
        SetHealthAndSpeed(_feet.Health,_feet.Speed);
        var a = transform.GetChild(0).GetComponentsInChildren<MeshRenderer>();
        
        name = $"{body.name}_{head.name}_{feet.name}";
        //Turn off the totem mesh renderers
        foreach (var meshRenderer in a)
        {
            meshRenderer.enabled = false;
        }
    }

    private void SetHealthAndSpeed(float h, float s)
    {
        health += h;
        speed += s;
        if (agente)
            agente.speed = Mathf.Max(speed + _speedModifier, 1) / 5f;
    }

    
    public void SetSpeedModifier(float speedChange)
    {
        _speedModifier = speedChange;
    }
    public void Attack()
    {
        // Debug.Log(name + " attacking!");
        if (Time.time - timeLastHit >= 1f / attackSpeed)
        {
            float aux = 0;
            if (objetive.TryGetComponent(out IAtackable m))
            {
                aux = m.Attacked(GetCurrentDamageModifier() + damage);
            }
            if (aux < 0)
            {
                Debug.Log(gameObject.name+"  " + objetive.position);
                // currentObjective = objetive;
                // if(agente.enabled)
                //     agente.SetDestination(objetive.position);
                // if (gameObject.TryGetComponent(out Aguila a))
                // {
                //     a.AguilaKill();
                // }
            }
            timeLastHit = Time.time;
        }   
    }
    public void PoisonAttack()
    {
        if (objetive && isEnemy && objetive.TryGetComponent<Entity>(out var enemy))
        {
            enemy.ApplyBleeding();
        }
    }
    
    [ClientRpc]
    public void SpawnClientRpc(int h, int b, int f)
    {
         SetAnimalParts(GameManager.Instance._heads[h], 
             GameManager.Instance._body[b], 
             GameManager.Instance._feet[f]);
         GameManager.Instance.playerObjects.Add(gameObject);
         gameObject.SetActive(false);
    }


    private void Awake()
    {
        //GameManager.Instance.playerObjects.Add(gameObject);
        //gameObject.SetActive(false);
        
        _attacksMap = new Dictionary<TotemPiece.Type, AttackStruct>();
        _speedModifier = 0;
    }



    void Start()
    {        
        _resources = new Dictionary<Recursos, int>();
        SetLayer(0, _idPlayer.Value);
        // currentDamage = damage;
        _idPlayer.OnValueChanged += SetLayer; 
        SetAgent();
        agente.speed = speed;
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

    private IEnumerator FollowEnemy()
    {
        if (!agente.isOnNavMesh) yield return null;
        Debug.LogWarning("Following enemy");
        while (!isFlying && !agente.isStopped && isEnemy && objetive)
        {
            var position = objetive.position;
            agente.SetDestination(position);
            var distance = Vector3.Distance(transform.position, position);
            if (distance <= maxAttackDistance)
            {
                agente.isStopped = true;
                break;
            }
            yield return new WaitForNextFrameUnit();
        }
    }
    
    void Update()
    {
        checkAreaAgent();

        // var d = _attacksMap.Select(a => a.Value.AttackDistance).Max();
        if (objetive)
        {
            if (isFlying)
            {
                flyUpdate();
            }
            else if(isEnemy)
            {
                var distance = Vector3.Distance(transform.position, objetive.position);
                // Debug.Log(maxAttackDistance + " ... " + Vector3.Distance(transform.position, objetive.position));
                if (distance <= maxAttackDistance)
                {
                    if (_followCoroutine != null)
                        StopCoroutine(_followCoroutine);
                    agente.isStopped = true;
                    if (Time.time - timeLastHit >= 1f / attackSpeed)
                    {
                        if (TryAttack(distance))
                        {
                            timeLastHit = Time.time;
                            // agente.isStopped = true;
                        }
                    }
                }
                // else if (isFlying)
                // {
                //     flyUpdate();
                // }
                else if (agente.isStopped)
                {
                    agente.isStopped = false;
                    // if(isFlying)
                    //     flyUpdate();
                    // else
                    {
                        _followCoroutine = StartCoroutine(FollowEnemy());
                    }
                }
            }
            else
            {
                
            }
        }
    }
    
    public float Attacked(float enemyDamage)
    {
        // Debug.Log(gameObject.name + " -- "+health);
        if (isFlying) enemyDamage *= 0.95f;
        health -= enemyDamage;
        if (health <= 0)
        {
            Destroy(gameObject);
            if (IsHost)
            {
                Debug.Log("uwulandia");
                GameManager.Instance.checkIfRoundEnded(layer);
            }
        }

        return health;
    }

    public void ApplyBleeding()
    {
        StartCoroutine(ChangeHealthOverTime(-10, 3));
    }

    public void ApplyHealing()
    {
        StartCoroutine(ChangeHealthOverTime(5, 10));
    }
    
    private IEnumerator ChangeHealthOverTime(int healthChange, int seconds)
    {
        for (int i = 0; i < seconds; i++)
        {
            yield return new WaitForSeconds(1.0f);
            health += healthChange;
        }
    }
    
    private void SetAgent()
    {
        agente = GetComponent<NavMeshAgent>();
        isOnGround = true;
        // agente.speed = speed;

        StartCoroutine(SetDestination(objetive));
    }
    private void flyUpdate()
    {
        if (transform.position.y < 2.5F)
        {
            gameObject.transform.Translate(Vector3.up* (Time.deltaTime * speed));
            return;
        }

        if (!objetive) return;
        if ((transform.position - objetive.position).magnitude > maxAttackDistance)
        {
            Vector3 dir = objetive.position - transform.position;
            dir.Normalize();
            transform.Translate(dir* (Time.deltaTime * speed),Space.World);
        }
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
    //private IEnumerator AddPosition()
    //{
    //    yield return new WaitForSeconds(1.0f);
    //    GameManager.Instance.AddPositionSomething(transform.position,gameObject);
    //}

    #region getters and setters

    public bool GetIsOnGround()
    {
        return isOnGround;
    }

    public float GetCurrentDamageModifier()
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
        if(!agente.isOnNavMesh)  return;
        if (objetive)
        {
            // Debug.LogError(Vector3.Distance(objetive.position, transform.position));
            // agente.isStopped = true;
            if (isEnemy)
            {
                if (Vector3.Distance(objetive.position, transform.position) > maxAttackDistance)
                {
                    // Debug.Log($"Atacando a {objetive} en {timeLastHit}");
                    // Attack();
                    agente.isStopped = false;
                    _followCoroutine = StartCoroutine(FollowEnemy());
                }
                return; 
                
                // if(TryAttack(Vector3.Distance(objetive.position, transform.position)))
                // {
                //     agente.isStopped = true;
                //     return;
                // }
                // if(agente.isStopped)    return;
            }
            if (Vector3.Distance(objetive.position, transform.position) <= 2f)
            {
                if(objetive.TryGetComponent<recurso>(out var res))
                    res.PickRecurso();
                return; 
            }
        }

        Debug.LogWarning("Searching for new objective");
        agente.isStopped = false;
        var enemies = FindObjectsOfType<Entity>().Where((entity, i) => entity.layer == layerEnemy).Select(entity => entity.transform).ToList();
        var resources = FindObjectsOfType<recurso>().Where(recurso => !recurso.GetSelected() && Vector3.Distance(transform.position, recurso.transform.position) <= GameManager.Instance.MaxDistance && recurso.GetComponent<MeshRenderer>().enabled).Select((recurso =>recurso)).ToList();
        if(resources.Count == 0 && enemies.Count == 0)  return;

        var values = enemies.Select(transform1 => new KeyValuePair<Transform, float>(transform1, 0f)).ToList();
        values.AddRange(resources.Select(transform1 => new KeyValuePair<Transform, float>(transform1.transform, 0f)));

        
        // var aaaaaaa = values.Aggregate("", (current, pair) => current + (pair.Key + " " + pair.Value + "\n"));
        // Debug.Log(aaaaaaa);
        var partRange = _head.AssignValuesToEnemies(enemies);
        partRange.AddRange(_head.AssignValuesToResources(resources));
        MergeInformation(ref values, partRange);
        
        partRange = _body.AssignValuesToEnemies(enemies);
        partRange.AddRange(_body.AssignValuesToResources(resources));
        MergeInformation(ref values, partRange);
        
        partRange = _feet.AssignValuesToEnemies(enemies);
        partRange.AddRange(_feet.AssignValuesToResources(resources));
        MergeInformation(ref values, partRange);

        // var a = from entry in values orderby entry.Value descending select entry;
        values.Sort((kp, kp1) => (int)Mathf.CeilToInt((kp.Value - kp1.Value)*100));
        // Debug.Log(resources.Count());
        objetive = values.First().Key;
        isEnemy = (bool)objetive.GetComponent<Entity>();
        // Debug.LogWarning($"Next objective is {objetive.name} and is {isEnemy} an enemy??");
        // agente.SetDestination(objetive.position);
        if(isEnemy)
            _followCoroutine = StartCoroutine(FollowEnemy());
        else
        {
            objetive.GetComponent<recurso>().SetSelected(true);
            agente.SetDestination(objetive.position);
            Debug.LogWarning("Going to " + objetive.position);
        }
        // Debug.Log(objetive.name);
    }

    private void MergeInformation(ref List<KeyValuePair<Transform, float>> inDic, List<float> info)
    {
        for (var i = 0; i < inDic.Count; i++)
        {
            inDic[i] = new KeyValuePair<Transform, float>(inDic[i].Key, inDic[i].Value + info[i]);
        }
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
    private float timeLastHit;

    public struct AttackStruct
    {
        public AttackStruct(float d, Action a)
        {
            AttackDistance = d;
            Attack = a;
            // Type = type;
        }

        // public TotemPiece.Type Type;
        public float AttackDistance;
        public Action Attack;
    }
    private Dictionary<TotemPiece.Type, AttackStruct> _attacksMap;
    // private List<AttackStruct> aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa;

    public void SubscribeAttack(TotemPiece.Type type, AttackStruct attackStruct)
    {
        _attacksMap.TryAdd(type, attackStruct);
        maxAttackDistance = _attacksMap.Select(a => a.Value.AttackDistance).Max();
    }
    public void UnsubscribeAttack(TotemPiece.Type type)
    {
        _attacksMap.Remove(type);
        maxAttackDistance = _attacksMap.Select(a => a.Value.AttackDistance).Max();
    }
    private bool TryAttack(float distance)
    {
        string blah = string.Join(", ", _attacksMap.Select(v => v.Value.AttackDistance.ToString(CultureInfo.InvariantCulture)).ToArray());
        // Debug.Log(blah);
        var possibleAttacks = _attacksMap.ToArray().Where(a => a.Value.AttackDistance >= distance).ToArray();
        if (!possibleAttacks.Any()) return false;
        // Debug.Log("Evaluating attacks");
        possibleAttacks[Random.Range(0, possibleAttacks.Count())].Value.Attack.Invoke();
        return true;
        // possibleAttacks.
    }
    
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
