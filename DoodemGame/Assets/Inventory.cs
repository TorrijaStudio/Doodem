using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Totems;
using Unity.Mathematics;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] private List<List<TotemPiece>> _totemPieces;

    [SerializeField] private playerInfoStore boton;

    [SerializeField] private Transform posToSpawn;

    [SerializeField] private GameObject pointer;
    [SerializeField] private GameObject wall;
    [SerializeField] private float distance;

    [SerializeField] private Totem totemToInstantiate;

    [SerializeField]
    private Transform totemParent;
    // Start is called before the first frame update
    void Start()
    {
        _totemPieces = new List<List<TotemPiece>>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool Contains(TotemPiece piece)
    {
        foreach (var totem in _totemPieces)
        {
            foreach (var totemPiece in totem)
            {
                if (piece == totemPiece) return true;
            }
        }

        return false;
    }
    
    public void SetDrag(bool active)
    {
        pointer.SetActive(active);
        wall.SetActive(active);
    }
    
    public void GetTotemsFromShop()
    {
        foreach (var obj in boton.boughtObjects.Select(objetoTienda => objetoTienda.GetComponent<objetoTienda>()))
        {
            Debug.Log(obj.name);
            _totemPieces.Add(obj.info.objectsToSell);
        }
    }

    public void DespawnItems()
    {
        var tempTotemPieces = new List<List<TotemPiece>>();
        for(int i = totemParent.childCount - 1; i >= 0; i--)
        {
            var child = totemParent.GetChild(i);
            var tempInfo = child.GetComponent<Totem>().GetTotem();
            if(tempInfo.Count > 0)
                tempTotemPieces.Add(tempInfo.Select(piece => piece.objectsToSell[0]).ToList());
            // if (tempInfo.Count == 3)
            // {
            //     tempTotemPieces.Add(tempInfo.Select(piece => piece.objectsToSell[0]).ToList());
            // }
            // else  if(tempInfo.Count > 0)
            // {
            //     foreach (var totemPiece in tempInfo)
            //     {
            //         tempTotemPieces.Add(new List<TotemPiece>(){totemPiece.objectsToSell[0]});
            //     }
            // }
            Destroy(child.gameObject);
        }
        _totemPieces.Clear();
        
        _totemPieces = tempTotemPieces;
        foreach (var totemPiece in _totemPieces)
        {
            var txt = "";
            foreach (var piece in totemPiece)
            {
                txt += "_ " + piece;
            }
            Debug.Log(txt);
        }
    }
    
    public void SpawnTotems()
    {
        var objectsToSpawn = _totemPieces.Count;
        var separationDistance = distance / objectsToSpawn;
        var pos = posToSpawn.position;
        foreach (var totemPiece in _totemPieces)
        { 
            var totem = Instantiate(totemToInstantiate, pos, Quaternion.identity, totemParent);
            // totem.transform.localRotation = Quaternion.Euler(0, 0, 0);
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
        SetDrag(true);
    }

    public void SpawnSeleccionables()
    {
        //TODO: esto esta azul
    }
}
