using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class readyBoton : MonoBehaviour
{
    // Start is called before the first frame update
    public objetoTienda[] storeObjects;
    public playerInfoStore tienda;
    public Transform storeItems;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

   public void OnClickPlayButton()
    {
        foreach (Transform obTr in storeItems)
        {
            var ob = obTr.GetComponent<objetoTienda>();
            if (ob.selected)
            {
                tienda.boughtObjects.Add(ob.gameObject);
            }
        }
        FindObjectOfType<Inventory>().GetTotemsFromShop();
        FindObjectOfType<Inventory>().SpawnTotems();
        // FindObjectOfType<Canvas>().gameObject.SetActive(false);
    }
}
