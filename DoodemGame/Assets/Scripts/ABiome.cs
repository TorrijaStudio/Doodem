using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Unity.AI.Navigation;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using Random = Unity.Mathematics.Random;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;


public abstract class ABiome : NetworkBehaviour
{
    private Transform obstaculos;
    private Transform recursos;
    public terreno terreno;
    private Vector2 cellSize;
    private List<Vector2> pos;//posiciones disponibles del bioma
    public NetworkVariable<int> _idPlayer = new NetworkVariable<int>(writePerm:NetworkVariableWritePermission.Server);
    //tamaño del bioma. Numero de celdas a cada lado
    public int xSize;
    public int zSize;
    public Material mat;
    public int indexLayerArea;

    private static Random random;
  
    
    // Start is called before the first frame update
     void Start()
    {
        HashSet<Vector2> positions = new HashSet<Vector2>();
        pos = new List<Vector2>();
        terreno = GameObject.Find("terreno").GetComponent<terreno>();
        cellSize = new Vector2(terreno.gameObject.transform.lossyScale.x, terreno.gameObject.transform.lossyScale.z) /
                   terreno.GetGrid();
        for (int i = 0; i <= xSize; i++)
        {
            for (int j = 0; j <= zSize; j++)
            {
                positions.Add(new Vector2(i, j));
                positions.Add(new Vector2(-i, -j));
                positions.Add(new Vector2(i, -j));
                positions.Add(new Vector2(-i, j));
            }
        }
        
        pos = positions.ToList();
        transform.localScale = new Vector3(2*xSize*cellSize.x+cellSize.x,transform.localScale.y,2*zSize*cellSize.y+cellSize.y);
        SetHijos();
        
    }

   

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.parent &&
                   other.transform.parent.name == "Tiles" && 
                  (!other.GetComponent<casilla>().GetBiome() || gameObject!=other.GetComponent<casilla>().GetBiome()))
        {
            //if(_idPlayer.Value == other.GetComponent<casilla>().GetSide())
            //{
                var mesh = other.transform.GetComponent<MeshRenderer>();
                var casilla = other.GetComponent<casilla>();
                var index = casilla.GetAreaNav();
                casilla.SetPreviousIndexArea(index);
                casilla.SetPreviousMat(mesh.material);
                casilla.SetPreviousBiome(casilla.GetBiome());
                
                mesh.material = mat;
                casilla.SetBiome(gameObject);
                casilla.SetAreaNav(indexLayerArea);
                casilla.SetSide(_idPlayer.Value);
                //terreno.transform.GetComponent<NavMeshSurface>().BuildNavMesh();
            
            //}
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.parent && other.transform.parent.name == "Tiles"&& other.GetComponent<casilla>().GetBiome() == gameObject)
        {
            var casilla = other.GetComponent<casilla>();
            other.transform.GetComponent<MeshRenderer>().material = casilla.GetPrevousMat();
            casilla.SetBiome(casilla.GetPreviousBiome());
            casilla.SetAreaNav(casilla.GetPreviousIndexArea());
        }
    }
    

    public void SetHijos()
    {
        obstaculos = transform.GetChild(0);
        foreach (Transform t in obstaculos)
        {
            obstaculos.localScale = Vector3.one;
           int index = UnityEngine.Random.Range(0, pos.Count);
           Vector2 v =pos[index];
           pos.Remove(v);
           Vector3 newPos = new Vector3(v.x*cellSize.x+transform.position.x,transform.position.y,v.y*cellSize.y+transform.position.z);
           if(terreno.IsInside(newPos))
           {
                t.position = newPos;
           }else
               t.gameObject.SetActive(false);
        }

        recursos = transform.GetChild(1);
        foreach (Transform r in recursos)
        {
            recursos.localScale = Vector3.one;
            int index = UnityEngine.Random.Range(0, pos.Count);
            Vector2 v =pos[index];
            pos.Remove(v);
            Vector3 newPos = new Vector3(v.x*cellSize.x+transform.position.x,transform.position.y,v.y*cellSize.y+transform.position.z);
            if(terreno.IsInside(newPos))
            {
                r.position = newPos;
            }else
                r.gameObject.SetActive(false);
           
        }
        //terreno.transform.GetComponent<NavMeshSurface>().BuildNavMesh();
    }

    public Transform GetRecursos()
    {
        return recursos;
    }

    public abstract void ActionBioma(GameObject o);
    public abstract void LeaveBiome(GameObject o);


}
