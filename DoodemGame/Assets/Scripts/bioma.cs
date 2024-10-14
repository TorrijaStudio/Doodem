using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.AI.Navigation;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UIElements;
using Random = Unity.Mathematics.Random;


public class bioma : NetworkBehaviour
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

    private static Random random;
    // Start is called before the first frame update
    void Start()
    {
        HashSet<Vector2 >positions = new HashSet<Vector2>();
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
        SetHijos();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Entity e))
        {
            if(e._idPlayer.Value == _idPlayer.Value)
                Debug.LogError(other.name);
        }
    }

    public void SetHijos()
    {
        obstaculos = transform.GetChild(0);
        foreach (Transform t in obstaculos)
        {
           int index = UnityEngine.Random.Range(0, pos.Count);
           Vector2 v =pos[index];
           pos.Remove(v);
           Vector3 newPos = new Vector3(v.x*cellSize.x+transform.position.x,transform.position.y,v.y*cellSize.y+transform.position.z);
           t.position = newPos;
        }

        recursos = transform.GetChild(1);
        foreach (Transform r in recursos)
        {
            int index = UnityEngine.Random.Range(0, pos.Count);
            Vector2 v =pos[index];
            pos.Remove(v);
            Vector3 newPos = new Vector3(v.x*cellSize.x+transform.position.x,transform.position.y,v.y*cellSize.y+transform.position.z);
            r.position = newPos;
        }
        terreno.transform.GetComponent<NavMeshSurface>().BuildNavMesh();
    }

    public Transform GetRecursos()
    {
        return recursos;
    }
    
}
