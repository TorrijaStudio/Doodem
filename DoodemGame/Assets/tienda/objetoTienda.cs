using System;
using System.Collections;
using System.Collections.Generic;
using tienda;
using Totems;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class objetoTienda : MonoBehaviour,IPointerClickHandler
{
    [SerializeField] public ScriptableObjectTienda info;
    public bool selected;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CreateObject(ScriptableObjectTienda scriptableObjectTienda)
    {
        info = scriptableObjectTienda;
        GetComponent<Image>().sprite = info.image;
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

