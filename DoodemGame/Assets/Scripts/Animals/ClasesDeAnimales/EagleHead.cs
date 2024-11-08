using System.Collections.Generic;
using System.Linq;
using Animals.Interfaces;
using Totems;
using UnityEngine;

namespace Animals.ClasesDeAnimales
{
    public class EagleHead :MonoBehaviour,  IAnimalHead
    {
        private Entity _entity;
        private void Start()
        {
            _entity = transform.GetComponentInParent<Entity>();
            _entity.SubscribeAttack(TotemPiece.Type.Head, new Entity.AttackStruct(AttackDistance, _entity.Attack));
            // _entity.att
        }
        
        public List<float> AssignValuesToResources(IList<Transform> resources)
        {
            var a = new float[resources.Count];
            for (var i = 0; i < resources.Count; i++)
            {
                var dist = transform.position - resources[i].position;
                a[i] = Mathf.Log10(dist.magnitude / GameManager.Instance.MaxDistance);
            }
            return a.ToList();
        }

        public List<float> AssignValuesToEnemies(IList<Transform> enemies)
        {
            var a = new float[enemies.Count];
            for (var i = 0; i < enemies.Count; i++)
            {
                var dist = transform.position - enemies[i].position;
                a[i] = Mathf.Log(dist.magnitude / GameManager.Instance.MaxDistance, 8);
            }
            return a.ToList();
        }

        public Resources Resources { get; set; }
        
        [field: SerializeField] public float Speed { get; set; }
        [field: SerializeField] public float Health { get; set; }
        [field: SerializeField] public float Damage { get; set; }

        [field: SerializeField] public float AttackDistance { get; set; }
    }
}