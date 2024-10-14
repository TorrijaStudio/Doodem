using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class recurso : MonoBehaviour
{
    private bool isSelected;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool GetSelected()
    {
        return isSelected;
    }

    public void SetSelected(bool b)
    {
        isSelected = b;
    }

    public void PickRecurso()
    {
        gameObject.SetActive(false);
    }
}
