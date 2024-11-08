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
    public Recursos typeResource;

    private static Random random;
    
    
    private void Awake()
    {
            GameManager.Instance.playerObjects.Add(gameObject);
            DisableMeshesRecursively(gameObject);
        
    }
    void DisableMeshesRecursively(GameObject obj)
    {
        // Desactiva el MeshRenderer si el objeto lo tiene
        MeshRenderer meshRenderer = obj.GetComponent<MeshRenderer>();
        if (meshRenderer != null)
        {
            meshRenderer.enabled = false;
        }

        // Recorre todos los hijos del objeto y llama recursivamente a esta función
        foreach (Transform child in obj.transform)
        {
            DisableMeshesRecursively(child.gameObject);
        }
    }
   public void EnableMeshesRecursively(GameObject obj)
    {
        // Desactiva el MeshRenderer si el objeto lo tiene
        MeshRenderer meshRenderer = obj.GetComponent<MeshRenderer>();
        if (meshRenderer != null)
        {
            Debug.LogError("ACTIVO");
            meshRenderer.enabled = true;
        }

        // Recorre todos los hijos del objeto y llama recursivamente a esta función
        foreach (Transform child in obj.transform)
        {
            EnableMeshesRecursively(child.gameObject);
        }
    }
    
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
        //if (IsOwner)
        //{
        //    Debug.LogError("holaaaa");
        //    SetHijos();
        //}

        StartCoroutine(UpdateEntities());
        
        //GameManager.Instance.playerObjects[_idPlayer.Value].Add(gameObject);
        //if (GameManager.Instance.clientId != _idPlayer.Value)
        //{
        //    gameObject.SetActive(false);
        //}
    }

    public void SetColorsGridBiome()
    {
        Collider[] colliders = Physics.OverlapBox(transform.position, transform.localScale/2f, transform.rotation,
            LayerMask.GetMask("casilla"));
        Debug.LogError(colliders.Length);
        foreach (var c in colliders)
        {
            var casillaMesh = c.GetComponent<MeshRenderer>();
            var casilla = c.GetComponent<casilla>();
            var materialBiome = casilla.GetBiome().GetComponent<ABiome>().mat;
            casillaMesh.material = materialBiome;
        }

    }
     
    private IEnumerator UpdateEntities()
     {
         yield return new WaitForSeconds(0.5f);
         GameManager.Instance.updateEntidades();
     }


     private void OnTriggerEnter(Collider other)
    {
        if (other.transform.parent &&
                   other.transform.parent.name == "Tiles" && 
                  (!other.GetComponent<casilla>().GetBiome() || gameObject!=other.GetComponent<casilla>().GetBiome()))
        {
                var mesh = other.transform.GetComponent<MeshRenderer>();
                var casilla = other.GetComponent<casilla>();
                var index = casilla.GetAreaNav();
                casilla.SetPreviousIndexArea(index);
                casilla.SetPreviousMat(mesh.material);
                casilla.SetPreviousBiome(casilla.GetBiome());
                
                if(IsOwner)
                    mesh.material = mat;
                casilla.SetMat(mat);
                casilla.SetBiome(gameObject);
                casilla.SetAreaNav(indexLayerArea);
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

    //nunca se destruye
    void OnDestroy()
    {
        //GameManager.Instance.playerObjects[_idPlayer.Value].Remove(gameObject);
        Collider[] colliders = Physics.OverlapBox(transform.position, transform.localScale/2f, transform.rotation,LayerMask.GetMask("casilla"));
       
       foreach (var c in colliders)
       {
           var casilla = c.GetComponent<casilla>();
           if (casilla.GetBiome() == gameObject)
           {
               casilla.ResetCasilla();
           }
       }
       GameManager.Instance.updateEntidades();
    }

    public abstract void ActionBioma(GameObject o);
    public abstract void LeaveBiome(GameObject o);


}
