using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using tienda;
using Unity.VisualScripting;
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
    [SerializeField] public GameObject botones;
    public Inventory inventory;
    public bool canOnlyChooseOne;
    private objetoTienda _selectedObject;
    private Vector3 _prevCameraPos;
    public Transform _cameraPos;
    public Quaternion cameraRot;
    public bool isFirstTime = true;


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

    private void MoveCameraToShop()
    {
        var cam = Camera.main.transform;
        _prevCameraPos = cam.position;
        cameraRot = cam.rotation;
        // Debug.LogWarning("camara camara camaramsd asfddjasd " + _prevCameraPos);
        cam.SetPositionAndRotation(_cameraPos.position, _cameraPos.rotation);
    }
    
    private void Start()
    {
        // InitialSelection();
    }

    public void CloseShopAfterTimer()
    {
        Debug.LogWarning("PREVIOUS POSITION: " + _prevCameraPos);
        Camera.main!.transform.SetPositionAndRotation(_prevCameraPos, cameraRot);
        if (isFirstTime && boughtObjects.Count == 0 && totemItems.childCount > 0)
        {
            boughtObjects.Add(totemItems.GetChild(0).gameObject);
            inventory.GetTotemsFromShop();
        }
        botones.SetActive(false);
        DeleteShopItems();
        inventory.DespawnItems();
        
    }

    public void DeleteShopItems()
    {
        for(var i = totemItems.childCount - 1; i >= 0; i--)
        {
            Destroy(totemItems.GetChild(i).gameObject);
        }
        boughtObjects.Clear();
        botones.SetActive(false);
    }
    
    // Start is called before the first frame update
    public void InitialSelection()
    {
        isFirstTime = true;
        MoveCameraToShop();
        canOnlyChooseOne = true;
        var index = 1;
        var prevTotems = new List<ScriptableObjectTienda>(totemsTienda);
        for (int i = 0; i < 2; i++)
        {
            var objT = Instantiate(objTiendaPrefab, positionsToSpawn[index].position, Quaternion.identity, totemItems);
            var totemI = Random.Range(0, prevTotems.Count);
            objT.CreateObject(prevTotems[totemI]);
            prevTotems.RemoveAt(totemI);
            index++;
        }
    }
    public void SetUpShop()
    {
        MoveCameraToShop();
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
