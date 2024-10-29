using System.Collections.Generic;
using Totems;
using UnityEngine;

namespace tienda
{
    [CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/ScriptableObjectTienda", order = 1)]
    public class ScriptableObjectTienda : ScriptableObject
    {
        public int price;
        public List<TotemPiece> objectsToSell;
        public Sprite image;
    }
}
