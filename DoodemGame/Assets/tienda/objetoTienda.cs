using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class objetoTienda : MonoBehaviour,IPointerClickHandler
{
    public GameObject objectToSell;
    public bool selected;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if(!selected && eventData.button == PointerEventData.InputButton.Left)
        {
            selected = true;
        }
        else if (selected && eventData.button == PointerEventData.InputButton.Right)
        {
            selected = false;
        }
    }
}

