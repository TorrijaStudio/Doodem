using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    [SerializeField] private Transform totemItems;
    [SerializeField] private GameObject botones;
    public Inventory inventory;
    public bool canOnlyChooseOne;
    private objetoTienda _selectedObject;

    public objetoTienda SelectedObject
    {
        get => _selectedObject;
        set
        {
            if(_selectedObject)
                _selectedObject.selected = false;
            _selectedObject = value;
        }
    }
    
    private void Start()
    {
        InitialSelection();
    }

    public void DeleteShopItems()
    {
        for(int i = totemItems.childCount - 1; i >= 0; i--)
        {
            Destroy(totemItems.GetChild(i).gameObject);
        }
        boughtObjects.Clear();
        botones.SetActive(false);
    }
    
    // Start is called before the first frame update
    public void InitialSelection()
    {
        canOnlyChooseOne = true;
        var index = 1;
        var prevTotems = new List<int>();
        for (var i = 0; i < totemsTienda.Count; i++)
        {
            prevTotems.Add(i);
        }
        for (int i = 0; i < 2; i++)
        {
            var objT = Instantiate(objTiendaPrefab, positionsToSpawn[index].position, Quaternion.identity, totemItems);
            var totemI = Random.Range(0, prevTotems.Count);
            prevTotems.RemoveAt(totemI);
            objT.CreateObject(totemsTienda[totemI]);
            index++;
        }
    }
    public void SetUpShop()
    {
        DeleteShopItems();
        canOnlyChooseOne = false;
        var index = 0;
        var spawnableObjects = objectsTiendas.Where(aux => !inventory.Contains(aux.objectsToSell[0])).ToList();
        for (int i = 0; i < 4; i++)
        {
            if(spawnableObjects.Count == 0) break;
            
            var objT = Instantiate(objTiendaPrefab, positionsToSpawn[index].position, Quaternion.identity, totemItems);
            var objToSpawn = Random.Range(0, spawnableObjects.Count);
            objT.CreateObject(spawnableObjects[objToSpawn]);
            spawnableObjects.RemoveAt(objToSpawn);
            index++;
        }
        botones.SetActive(true);
    }
}
