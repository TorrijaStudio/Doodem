using System.Collections.Generic;
using System.Linq;
using Animals.Interfaces;
using Totems;
using UnityEngine;

namespace Animals.ClasesDeAnimales
{
    public class BeeFeet :MonoBehaviour,  IAnimalFeet
    {
        [SerializeField] private Recursos resource;
        private Entity _entity;
        [SerializeField] private int resourceQuantity;
        private bool _isSubscribed;
        private const float AttackDistance = 2.5f;
        private const float AttackAngle = 60f;
        private const float AttackDamage = 10f;

        private void Start()
        {
            _entity = transform.GetComponentInParent<Entity>();
            _entity.OnResourcesChanged += HasResourcesForAttack;
            // _entity.SubscribeAttack(TotemPiece.Type.Body, new Entity.AttackStruct(AttackDistance, AreaAttack));
            // _entity.att
        }

        private void HasResourcesForAttack(Recursos resources, int number)
        {
            if(resources != resource)   return;

            if (number >= resourceQuantity)
            {
                if(!_isSubscribed){
                    Debug.Log("Ataque abeja suscrito");
                    _entity.SubscribeAttack(TotemPiece.Type.Feet, new Entity.AttackStruct(AttackDistance, _entity.PoisonAttack));
                    _isSubscribed = true;
                }
            }
            else
            {
                if (_isSubscribed)
                {
                    Debug.Log("Ataque abeja desuscrito");
                    _entity.UnsubscribeAttack(TotemPiece.Type.Feet);
                    _isSubscribed = false;
                }
            }
        }
        public List<float> AssignValuesToResources(List<recurso> resources)
        {
            var a = new float[resources.Count];
            for (var i = 0; i < resources.Count; i++)
            {
                if(resources[i]._typeRecurso == resource)
                {
                    var dist = transform.position - resources[i].transform.position;
                    a[i] = Mathf.Pow(dist.magnitude / GameManager.Instance.MaxDistance, 4f);
                }
                else
                {
                    a[i] = 0;
                }
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
    }
}