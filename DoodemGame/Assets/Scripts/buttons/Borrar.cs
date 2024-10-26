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
            GameManager.Instance.DespawnServerRpc(n, default);
            GameManager.Instance.objectSelected = null;
        }
        
    }
}
