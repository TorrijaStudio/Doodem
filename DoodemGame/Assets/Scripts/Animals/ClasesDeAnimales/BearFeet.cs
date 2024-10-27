using System.Collections.Generic;
using System.Linq;
using Animals.Interfaces;
using UnityEngine;

namespace Animals.ClasesDeAnimales
{
    public class BearFeet :MonoBehaviour,  IAnimalFeet
    {
        public List<KeyValuePair<Transform, float>> AssignValuesToResources(IList<Transform> resources)
        {
            var a = new List<KeyValuePair<Transform, float>>();
            foreach (var enemy in resources)
            {
                var dist = transform.position - enemy.position;
                a.Add(new KeyValuePair<Transform, float>(enemy, Mathf.Pow(dist.magnitude / GameManager.Instance.MaxDistance, 2f)));
            }
            return a;        
        }

        public List<KeyValuePair<Transform, float>> AssignValuesToEnemies(IList<Transform> enemies)
        {
            var a = new List<KeyValuePair<Transform, float>>();
            foreach (var enemy in enemies)
            {
                var dist = transform.position - enemy.position;
                a.Add(new KeyValuePair<Transform, float>(enemy, Mathf.Sqrt(dist.magnitude / GameManager.Instance.MaxDistance)));
            }
            return a;
        }

        public Resources Resources { get; set; }
        public float Life { get; set; }
        public float Health { get; set; }
        public float Damage { get; set; }
    }
}