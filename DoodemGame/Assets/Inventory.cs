using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Totems;
using Unity.Mathematics;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    private List<List<TotemPiece>> _totemPieces;

    [SerializeField] private playerInfoStore boton;

    [SerializeField] private Transform posToSpawn;

    [SerializeField] private float distance;

    [SerializeField] private Totem totemToInstantiate;
    // Start is called before the first frame update
    void Start()
    {
        _totemPieces = new List<List<TotemPiece>>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GetTotemsFromShop()
    {
        foreach (var obj in boton.boughtObjects.Select(objetoTienda => objetoTienda.GetComponent<objetoTienda>()))
        {
            Debug.Log(obj.name);
            _totemPieces.Add(obj.info.objectsToSell);
        }
    }
    
    public void SpawnTotems()
    {
        var objectsToSpawn = _totemPieces.Count;
        var separationDistance = distance / objectsToSpawn;
        var pos = posToSpawn.position;
        foreach (var totemPiece in _totemPieces)
        { 
            var totem = Instantiate(totemToInstantiate, pos, quaternion.identity, transform);
            var aux = new GameObject[] { null, null, null };
            foreach (var piece in totemPiece)
            {
                switch (piece.tag)
                {
                    case "Head":
                        aux[0] = piece.gameObject;
                        break;
                    case "Body":
                        aux[1] = piece.gameObject;
                        break;
                    case "Feet":
                        aux[2] = piece.gameObject;
                        break;
                }
            }
            totem.CreateTotem(aux[0], aux[1], aux[2]);
            pos += Vector3.right * separationDistance;
        }
    }
}
