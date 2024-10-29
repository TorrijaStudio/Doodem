using System.Collections.Generic;
using System.Linq;
using Animals.Interfaces;
using UnityEngine;

namespace Animals.ClasesDeAnimales
{
    public class BeeBody : AnimalBody
    {
        public override List<float> AssignValuesToResources(IList<Transform> resources)
        {
            var a = new float[resources.Count];
            for (var i = 0; i < resources.Count; i++)
            {
                var dist = transform.position - resources[i].position;
                a[i] = Mathf.Sqrt(dist.magnitude / GameManager.Instance.MaxDistance);
            }
            return a.ToList();
        }

        public override List<float> AssignValuesToEnemies(IList<Transform> enemies)
        {
            var a = new float[enemies.Count];
            for (var i = 0; i < enemies.Count; i++)
            {
                var dist = transform.position - enemies[i].position;
                a[i] = Mathf.Pow(dist.magnitude / GameManager.Instance.MaxDistance, 1.5f);
            }
            return a.ToList();

        }
    }
}