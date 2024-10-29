using System.Collections.Generic;
using System.Linq;
using Animals.Interfaces;
using UnityEngine;

namespace Animals.ClasesDeAnimales
{
    public class BeeHead :MonoBehaviour,  IAnimalHead
    {
        public List<float> AssignValuesToResources(IList<Transform> resources)
        {
            var a = new float[resources.Count];
            for (var i = 0; i < resources.Count; i++)
            {
                var dist = transform.position - resources[i].position;
                a[i] = Mathf.Pow(dist.magnitude / GameManager.Instance.MaxDistance, 2.5f);
            }
            return a.ToList();
        }

        public List<float> AssignValuesToEnemies(IList<Transform> enemies)
        {
            var a = new float[enemies.Count];
            for (var i = 0; i < enemies.Count; i++)
            {
                var dist = transform.position - enemies[i].position;
                a[i] = Mathf.Log(dist.magnitude / GameManager.Instance.MaxDistance, 5);
            }
            return a.ToList();
        }

        public Resources Resources { get; set; }
        public float Life { get; set; }
        public float Health { get; set; }
        public float Damage { get; set; }
    }
}