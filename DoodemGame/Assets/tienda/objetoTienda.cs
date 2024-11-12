using System;
using System.Collections;
using System.Collections.Generic;
using tienda;
using TMPro;
using Totems;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class objetoTienda : MonoBehaviour,IPointerClickHandler
{
    [SerializeField] public ScriptableObjectTienda info;
    public bool selected;

    private playerInfoStore _store;

    private TextMeshProUGUI _proUGUI;
    // Start is called before the first frame update
    void Start()
    {
        // _proUGUI = GetComponentInChildren<TextMeshProUGUI>();
        // _store.OnItemSelected += SetTextColour;
    }

    private void OnDestroy()
    {
        _store.OnItemSelected -= SetTextColour;
    }

    private void SetTextColour()
    {
        if (selected)
        {
            _proUGUI.color = new Color(0.55f, 0.2f, 0.58f);
            return;
        }

        _proUGUI.color = _store.CanBuyItem(info.price) ? new Color(0.28f, 0.6f, 0f) : new Color(0.67f, 0.17f, 0.11f);
    }
    
    public void CreateObject(ScriptableObjectTienda scriptableObjectTienda)
    {
        info = scriptableObjectTienda;
        GetComponent<Image>().sprite = info.image;
        _store = FindObjectOfType<playerInfoStore>();
        _proUGUI = GetComponentInChildren<TextMeshProUGUI>();
        _proUGUI.SetText(info.price.ToString());
        _store.OnItemSelected += SetTextColour;
        SetTextColour();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(!selected && eventData.button == PointerEventData.InputButton.Left)
        {
            selected = true;
            if (!_store.CanBuyItem(info.price)) return;
            
            if (_store.canOnlyChooseOne)
                _store.SelectedObject = this;
            
            _store.SelectedItemsCost += info.price;
        }
        else if (selected && eventData.button == PointerEventData.InputButton.Right)
        {
            selected = false;
            _store.SelectedItemsCost -= info.price;
            if (_store.canOnlyChooseOne)
                _store.SelectedObject = null;
        }
    }
}

