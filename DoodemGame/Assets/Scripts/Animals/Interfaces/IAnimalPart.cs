using System.Collections.Generic;
using UnityEngine;

namespace Animals.Interfaces
{
    public interface IAnimalPart
    {
        public List<KeyValuePair<Transform, float>> AssignValuesToResources(IList<Transform> resources);
        public List<KeyValuePair<Transform, float>> AssignValuesToEnemies(IList<Transform> enemies);

        Resources Resources { get; set; }
        float Life { get; set; }
        float Health { get; set; }
        float Damage { get; set; }
    }
}