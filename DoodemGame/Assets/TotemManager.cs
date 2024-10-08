using System;
using System.Collections;
using System.Collections.Generic;
using Totems;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class TotemManager : MonoBehaviour, IPointerDownHandler, IPointerMoveHandler
{

    [SerializeField] private TotemPiece selectedPiece;
    [SerializeField] private Totem selectedTotem;

    [SerializeField] private TotemPiece grabbedPiece;
    private Vector3 _grabPosition;
    private const float GrabOffset = 0.5f;
    private bool _isDragging;
    
    [SerializeField] private Transform pointer;
    private Camera _camera;
    
    void Start()
    {
        // RegisterCallback<PointerDownEvent>(PressController());
        _camera = Camera.main;
        pointer = transform;
        Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Wall"), LayerMask.NameToLayer("Pointer"));
    }
    
    void Update()
    {
        //If computer (??)
        if (Input.touchSupported)
        {
            
        }
        else
        {
            PointerController(MouseToRay());
            if (Input.GetMouseButton(0))
            {
                PressController(Input.mousePosition);
                _isDragging = true;
            }
            else
            {
                _isDragging = false;
                if (grabbedPiece)
                {
                    if(!selectedPiece || !selectedTotem.CanTakePart(grabbedPiece.gameObject))
                    {
                        grabbedPiece.MoveTo(_grabPosition - grabbedPiece.transform.forward * GrabOffset, 0.5f, true);
                    }
                    else
                    {
                        //If the totem doesnt have designated parts then dont admit that kind of part
                        var tempTotem = grabbedPiece.totem;
                        var tempPosition = selectedPiece.transform.position;
                        selectedPiece.totem = tempTotem;
                        selectedPiece.transform.SetParent(tempTotem.transform);
                        grabbedPiece.transform.SetParent(selectedTotem.transform);
                        grabbedPiece.totem = selectedTotem;
                        //if totem is null send other piece to established overflow positions
                        
                        selectedPiece.MoveTo(_grabPosition, 0.5f, selectedPiece.totem);
                        grabbedPiece.MoveTo(tempPosition, 0.5f, grabbedPiece.totem);
                        grabbedPiece.transform.SetSiblingIndex(TagToNumber(grabbedPiece.tag));
                        selectedPiece.transform.SetSiblingIndex(TagToNumber(selectedPiece.tag));
                        selectedPiece = grabbedPiece;
                    }
                    grabbedPiece = null;
                    
                if(selectedPiece && selectedPiece.totem)
                    selectedPiece.totem.Lock(false);
                }
            }
        }
    }

    private int TagToNumber(string objTag)
    {
        return objTag switch
        {
            "Head" => 0,
            "Body" => 1,
            "Feet" => 2,
            _ => 3
        };
    }
    
    private void PointerController(Ray ray)
    {
        if (Physics.Raycast(ray, out var hit, 10f, LayerMask.GetMask("Wall", "Totem")))
        {
            pointer.position = hit.point;
        }
    }

    void PressController(Vector3 pos)
    {
        if(!_isDragging && selectedPiece)
        {
            var selectedPieceTransform = selectedPiece.transform;
            _grabPosition = selectedPieceTransform.position + selectedPieceTransform.forward * GrabOffset;
            grabbedPiece = selectedPiece;
            // if(selectedPiece.totem)
            selectedTotem.Lock(true);
            selectedPiece = null;
        }
        if(!grabbedPiece)   return;
        
        Debug.Log("Im pressing " + selectedPiece);
        grabbedPiece.transform.position = CalculatePosition();

    }
    
    private Vector3 CalculatePosition()
    {
        var ray = _camera.ScreenPointToRay(Input.mousePosition);
        var z = _grabPosition.z;
        var y = _grabPosition.y;
        var t = (z - ray.origin.z) / ray.direction.z;
        var pos = new Vector3(t*ray.direction.x + ray.origin.x, y,z);
        return pos;
    }
    
    #region Trigger Events

    private void OnTriggerEnter(Collider other)
        {
            // Debug.Log("Entering: " + other.name + " with tag " + other.tag);
            switch (other.tag)
            {
                case "Totem":
                    if(selectedTotem)
                    {
                        Debug.Log("Deactivating...");
                        selectedTotem.Deactivate();
                    }
                    selectedTotem = other.GetComponent<Totem>();
                    if (_isDragging)
                    {
                        if(selectedTotem.CanTakePart(grabbedPiece.gameObject))
                        {
                            selectedTotem.Separate(TagToNumber(grabbedPiece.tag) + 1);
                            selectedPiece = selectedTotem.transform.GetChild(Mathf.Min(TagToNumber(grabbedPiece.tag), selectedTotem.transform.childCount - 1))
                                .GetComponent<TotemPiece>();
                        }
                    }
                    break;
                case "Head":
                case "Body":
                case "Feet":
                    // if(_isDragging)
                    // {
                    //     other.GetComponent<TotemPiece>().totem.Separate(other.transform.GetSiblingIndex() + 1);
                    //     break;
                    // }
                    if (_isDragging)
                    {
                    }
                    else
                    {
                        var auxSelection = other.GetComponent<TotemPiece>();
                        if (auxSelection == grabbedPiece) break;
                        selectedPiece = auxSelection;
                        if(selectedPiece.totem)
                            selectedPiece.totem.Separate(other.transform.GetSiblingIndex() + 1);
                    }
                    // selectedTotem.Separate(other.transform.GetSiblingIndex() + 1);
                    break;
            }
        }
    
        private void OnTriggerExit(Collider other)
        {
            Debug.Log("Exiting: " + other.name + " with tag " + other.tag);
            switch (other.tag)
            {
                case "Totem":
                    selectedTotem.Deactivate();
                    selectedTotem = null;
                    break;
                case "Head":
                case "Body":
                case "Feet":
                    // Debug.Log(other.name + " - " + selectedPiece.transform);
                    if(!grabbedPiece)
                        selectedPiece = null;
                    else
                    {
                        selectedPiece = grabbedPiece;
                    }
                    break;
            }
        }

    #endregion
    
    private Ray MouseToRay()
    {
        return _camera.ScreenPointToRay(Input.mousePosition);
    }
    
    #region InputRegion

        public void OnPointerDown(PointerEventData eventData)
        {
            // eventData.dragging;
            // PointerController(_camera.ScreenPointToRay(eventData.position));
            PressController(eventData.position);
        }
        
        
        public void OnPointerMove(PointerEventData eventData)
        {
            // Debug.Log(eventData.pointerDrag);
        }
    #endregion

    
}
