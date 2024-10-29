using System.Collections.Generic;
using Animals.Interfaces;
using Totems;
using UnityEngine;

namespace tienda
{
    [CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/ScriptableObjectTienda", order = 1)]
    public class ScriptableObjectTienda : ScriptableObject
    {
        public int price;
        public List<TotemPiece> objectsToSell;
        public List<GameObject> animalParts;
        public Sprite image;
    }
}
