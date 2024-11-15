using System.Collections.Generic;
using System.Linq;
using Animals.Interfaces;
using Totems;
using UnityEngine;

namespace Animals.ClasesDeAnimales
{
    public class EagleFeet :MonoBehaviour,  IAnimalFeet
    {
        private Entity _entity;
        [SerializeField] private float attackDistance;
        [SerializeField] private Recursos resource;
        [SerializeField] private int resourceQuantity;
        private bool _isSubscribed;
        
        private void Start()
        {
            _entity = transform.GetComponentInParent<Entity>();
            _entity.OnResourcesChanged += HasResourcesForAttack;
        }

        private void HasResourcesForAttack(Recursos resources, int number)
        {
            if(resources != resource)   return;

            if (number >= resourceQuantity)
            {
                if(!_isSubscribed){
                    Debug.Log("Ataque aguila suscrito");
                    _entity.SubscribeAttack(TotemPiece.Type.Feet, new Entity.AttackStruct(attackDistance, AttackEagleFeet));
                    _isSubscribed = true;
                }
            }
            else
            {
                if (_isSubscribed)
                {
                    Debug.Log("Ataque aguila desuscrito");
                    _entity.UnsubscribeAttack(TotemPiece.Type.Feet);
                    _isSubscribed = false;
                }
            }
        }

        private void EagleAttack()
        {
            // if(Physics.)
        }
        
        private void AttackEagleFeet()
        {
            var entityDamage = _entity.damage;
            _entity.damage = Damage;
            _entity.Attack();
            _entity.damage = entityDamage;
            _entity.AddOrTakeResources(resource, resourceQuantity);
        }
        
        public List<float> AssignValuesToResources(IList<Transform> resources)
        {
            var a = new float[resources.Count];
            for (var i = 0; i < resources.Count; i++)
            {
                var dist = transform.position - resources[i].position;
                a[i] = Mathf.Sqrt(dist.magnitude / GameManager.Instance.MaxDistance);
            }
            return a.ToList();
        }

        public List<float> AssignValuesToEnemies(IList<Transform> enemies)
        {
            var a = new float[enemies.Count];
            for (var i = 0; i < enemies.Count; i++)
            {
                var dist = transform.position - enemies[i].position;
                a[i] = Mathf.Sqrt(dist.magnitude / GameManager.Instance.MaxDistance);
            }
            return a.ToList();

        }

        [field: SerializeField] public float Speed { get; set; }
        [field: SerializeField] public float Health { get; set; }
        [field: SerializeField] public float Damage { get; set; }
        [field: SerializeField] public float AttackDistance { get; set; }
    }
}