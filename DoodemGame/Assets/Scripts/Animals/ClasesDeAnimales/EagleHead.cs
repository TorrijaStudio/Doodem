using System.Collections.Generic;
using System.Linq;
using Animals.Interfaces;
using UnityEngine;

namespace Animals.ClasesDeAnimales
{
    public class EagleHead :MonoBehaviour,  IAnimalHead
    {
        public List<KeyValuePair<Transform, float>> AssignValuesToResources(IList<Transform> resources)
        {
            var a =new List<KeyValuePair<Transform, float>>();
            foreach (var enemy in resources)
            {
                var dist = transform.position - enemy.position;
                a.Add(new KeyValuePair<Transform, float>(enemy, Mathf.Log10(dist.magnitude / GameManager.Instance.MaxDistance)));
            }
            return a;
        }

        public List<KeyValuePair<Transform, float>> AssignValuesToEnemies(IList<Transform> enemies)
        {
            // return enemies.Select(resource => resource.position - transform.position).Select(dist => Mathf.Log(dist.magnitude / GameManager.Instance.MaxDistance, 8)).ToList();
            var a = new List<KeyValuePair<Transform, float>>();
            foreach (var enemy in enemies)
            {
                var dist = transform.position - enemy.position;
                a.Add(new KeyValuePair<Transform, float>(enemy, Mathf.Log(dist.magnitude / GameManager.Instance.MaxDistance, 8)));
            }
            return a;
        }

        public Resources Resources { get; set; }
        public float Life { get; set; }
        public float Health { get; set; }
        public float Damage { get; set; }
    }
}