using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class readyBoton : MonoBehaviour
{
    // Start is called before the first frame update
    public objetoTienda[] storeObjects;
    public playerInfoStore tienda;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

   public void OnClickPlayButton()
    {
        foreach (var ob in storeObjects)
        {
            if (ob.selected)
            {
                tienda.boughtObjects.Add(ob.gameObject);
            }
        }
    }
}
