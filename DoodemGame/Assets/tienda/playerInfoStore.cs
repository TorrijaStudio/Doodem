using System;
using System.Collections;
using System.Collections.Generic;
using tienda;
using UnityEngine;
using Random = UnityEngine.Random;

public class playerInfoStore : MonoBehaviour
{

    public List<GameObject> boughtObjects = new();

    [SerializeField]
    private objetoTienda objTiendaPrefab;
    public List<ScriptableObjectTienda> objectsTiendas;
    public List<ScriptableObjectTienda> totemsTienda;

    [SerializeField] private Transform[] positionsToSpawn;

    private void Start()
    {
        InitialSelection();
    }

    // Start is called before the first frame update
    public void InitialSelection()
    {
        var index = 1;
        for (int i = 0; i < 2; i++)
        {
            var objT = Instantiate(objTiendaPrefab, positionsToSpawn[index].position, Quaternion.identity, transform);
            objT.CreateObject(totemsTienda[Random.Range(0, totemsTienda.Count)]);
            index++;
        }
    }
}
