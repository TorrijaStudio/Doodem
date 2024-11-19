using System.Collections.Generic;
using System.Linq;
using Animals.Interfaces;
using Totems;
using UnityEngine;

namespace Animals.ClasesDeAnimales
{
    public class BeeHead :MonoBehaviour,  IAnimalHead
    {
        private Entity _entity;
        
        private void Start()
        {
            _entity = transform.GetComponentInParent<Entity>();
            _entity.SubscribeAttack(TotemPiece.Type.Head, new Entity.AttackStruct(AttackDistance, _entity.Attack));
            // _entity.att
        }
        public List<float> AssignValuesToResources(List<recurso> resources)
        {
            var a = new float[resources.Count];
            return a.ToList();
        }

        public List<float> AssignValuesToEnemies(IList<Transform> enemies)
        {
            var a = new float[enemies.Count];
            return a.ToList();
        }

        [field: SerializeField] public float Speed { get; set; }
        [field: SerializeField] public float Health { get; set; }
        [field: SerializeField] public float Damage { get; set; }


        [field: SerializeField] public float AttackDistance { get; set; }
    }
}