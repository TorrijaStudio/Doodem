using System;
using System.Collections.Generic;
using System.Linq;
using Animals.Interfaces;
using Totems;
using UnityEngine;

namespace Animals.ClasesDeAnimales
{
    public class BearBody : AnimalBody
    {
        private Entity _entity;
        private const float AttackDistance = 2.5f;
        private const float AttackAngle = 60f;
        private const float AttackDamage = 10f;

        private void Start()
        {
            _entity = transform.GetComponentInParent<Entity>();
            _entity.SubscribeAttack(TotemPiece.Type.Body, new Entity.AttackStruct(AttackDistance, AreaAttack));
            // _entity.att
        }

        private void AreaAttack()
        {
            Debug.Log("Area attack :) from " + transform.parent.name);
            Collider[] hitColliders = Physics.OverlapSphere(_entity.transform.position, AttackDistance, LayerMask.GetMask(_entity.layerEnemy));
            foreach (var c in hitColliders)
            {
                var transform1 = transform;
                var angle = Vector3.Angle(transform1.forward, c.transform.position - transform1.position);
                if(angle>AttackAngle/2) continue;
                if (c.TryGetComponent(out IAtackable m))
                {
                    m.Attacked(Mathf.Min(_entity.GetCurrentDamageModifier() + AttackDamage, 0));
                }
            }
        }
        
        public override List<float> AssignValuesToResources(IList<Transform> resources)
        {
            var a = new float[resources.Count];
            for (var i = 0; i < resources.Count; i++)
            {
                var dist = transform.position - resources[i].position;
                a[i] = Mathf.Pow(dist.magnitude / GameManager.Instance.MaxDistance, 4f);
            }
            return a.ToList();
        }

        public override List<float> AssignValuesToEnemies(IList<Transform> enemies)
        {
            var a = new float[enemies.Count];
            for (var i = 0; i < enemies.Count; i++)
            {
                var dist = transform.position - enemies[i].position;
                a[i] = Mathf.Pow(dist.magnitude / GameManager.Instance.MaxDistance, 4f);
            }
            return a.ToList();

        }
    }
}