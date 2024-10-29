using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Borrar : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClickButton()
    {
        var g = GameManager.Instance.objectSelected;
        if(!g) return;
        if (g.TryGetComponent(out casilla c))
        {
            g = c.GetBiome();
            if(!g) return;
        }
        if (g.TryGetComponent(out NetworkObject n))
        {
            Seleccionable s = null;
            if(g.name=="personaje1(Clone)")
                s =transform.parent.GetChild(0).GetChild(0).GetComponent<Seleccionable>();
            else if(g.name=="bioma(Clone)")
                s =transform.parent.GetChild(0).GetChild(1).GetComponent<Seleccionable>();
            else if(g.name=="bioma1(Clone)")
                s =transform.parent.GetChild(0).GetChild(2).GetComponent<Seleccionable>();
            if(s!=null)
                s.AddNumCarta();
            GameManager.Instance.DespawnServerRpc(n, default);
            GameManager.Instance.objectSelected = null;
        }
        
    }
}
