using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using tienda;
using TMPro;
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
    [SerializeField] private TextMeshProUGUI playerMoneyText;
    public Inventory inventory;
    public bool canOnlyChooseOne;
    private objetoTienda _selectedObject;
    private Vector3 _prevCameraPos;
    public Transform _cameraPos;
    public Quaternion cameraRot;
    public bool isFirstTime = true;
    public int playerMoney = 8;
    private int _selectedItemsCost;

    public int SelectedItemsCost
    {
        get => _selectedItemsCost;
        set
        {
            _selectedItemsCost = Math.Max(0, value);
            Debug.Log("Objeto tienda (de)seleccionado " + OnItemSelected.Method.Name + " new precio: " + _selectedItemsCost + " player " + playerMoney);
            OnItemSelected.Invoke();
        }
    }
    public delegate void ItemSelected();

    public ItemSelected OnItemSelected;
    
    public bool CanBuyItem(int cost)
    {
        return SelectedItemsCost + cost <= playerMoney;
    }

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
        
        playerMoneyText.gameObject.SetActive(true);
        playerMoneyText.SetText(_selectedItemsCost + "/" + playerMoney);
    }
    
    private void Start()
    {
        // InitialSelection();
    }

    public void CloseShopAfterTimer()
    {
        // Debug.LogWarning("PREVIOUS POSITION: " + _prevCameraPos);
        Camera.main!.transform.SetPositionAndRotation(_prevCameraPos, cameraRot);
        if (isFirstTime && boughtObjects.Count == 0 && totemItems.childCount > 0)
        {
            boughtObjects.Add(totemItems.GetChild(0).gameObject);
            inventory.GetTotemsFromShop();
        }
        botones.SetActive(false);
        DeleteShopItems();
        inventory.DespawnItems();
        
        playerMoneyText.gameObject.SetActive(false);
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
        playerMoney = 10;
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
    public void SetUpShop(int moneyGained)
    {
        playerMoney += moneyGained;
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
