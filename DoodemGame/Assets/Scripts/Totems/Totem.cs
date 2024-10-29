using System;
using System.Collections;
using System.Collections.Generic;
using tienda;
using Totems;
using UnityEngine;

public class Totem : MonoBehaviour
{
    private Transform _transform;
    [SerializeField] private float TotemOffset = 1.0f;
    [SerializeField] private float TotemPieceHover = 0.30f;
    
    [SerializeField] private Transform head;
    [SerializeField] private Transform body;
    [SerializeField] private Transform feet;
    

    private bool isLocked = false;

    public List<ScriptableObjectTienda> GetTotem()
    {
        var list = new List<ScriptableObjectTienda>();
        if(head)
            list.Add(head.GetComponent<TotemPiece>().scriptableObjectTienda);
        if(body)
            list.Add(body.GetComponent<TotemPiece>().scriptableObjectTienda);
        if(feet)
            list.Add(feet.GetComponent<TotemPiece>().scriptableObjectTienda);
        return list;
    }
    public void CreateTotem(GameObject h, GameObject b, GameObject f)
    {
        _transform = transform;
        // if(!(ValidatePartDebug(h, "Head") && ValidatePartDebug(b, "Body") && ValidatePartDebug(f, "Feet")))
        //     return;

        var position = _transform.position;
        var up = _transform.up * TotemOffset;
        if(ValidatePartDebug(h, "Head"))
            head = Instantiate(h, position + up, Quaternion.Euler(0, 180, 0), _transform).transform;
        // head.transform.SetParent(_transform);
        
        if(ValidatePartDebug(b, "Body"))
            body = Instantiate(b, position, Quaternion.Euler(0, 180, 0), _transform).transform;
        // body.transform.SetParent(_transform);
        
        if(ValidatePartDebug(f, "Feet"))
            feet = Instantiate(f, position - up, Quaternion.Euler(0, 180, 0), _transform).transform;
        // feet.transform.SetParent(_transform);
    }
    public void CreateTotem(ScriptableObjectTienda soh, ScriptableObjectTienda sob, ScriptableObjectTienda sof)
    {
        _transform = transform;
        // if(!(ValidatePartDebug(h, "Head") && ValidatePartDebug(b, "Body") && ValidatePartDebug(f, "Feet")))
        //     return;

        var position = _transform.position;
        var up = _transform.up * TotemOffset;
        var h = soh.objectsToSell[0].gameObject;
        var b = sob.objectsToSell[0].gameObject;
        var f = sof.objectsToSell[0].gameObject;
        if(ValidatePartDebug(h, "Head"))
        {
            var tempHead = Instantiate(soh.objectsToSell[0], position + up, 
                Quaternion.Euler(0, 180, 0), _transform);
            tempHead.scriptableObjectTienda = soh;
            head = tempHead.transform;
        }        // head.transform.SetParent(_transform);
        
        if(ValidatePartDebug(b, "Body"))
        {
            var tempBody = Instantiate(sob.objectsToSell[0], position, 
                Quaternion.Euler(0, 180, 0), _transform);
            tempBody.scriptableObjectTienda = sob;
            body = tempBody.transform;
        }
        // body.transform.SetParent(_transform);
        
        if(ValidatePartDebug(f, "Feet"))
        {
            var tempFeet = Instantiate(sof.objectsToSell[0], position - up, 
                Quaternion.Euler(0, 180, 0), _transform);
            tempFeet.scriptableObjectTienda = sof;
            feet = tempFeet.transform;
        }        // feet.transform.SetParent(_transform);
    }
    private bool ValidatePartDebug(GameObject g, string type)
    {
        if (!g) return false;
        
        if (!g.CompareTag(type))
        {
            Debug.LogError($"Totem {name} was given object {g.name} of type {g.tag} instead of a {type}!");
            return false;
        }

        return true;
    }

    public bool IsPiece()
    {
        return !(head && body && feet);
    }
    
    public bool AddPart(TotemPiece pieceToSet, out TotemPiece outPiece)
    {
        outPiece = null;
        var result = pieceToSet.tag switch
        {
            "Head" => TryAddPiece(pieceToSet, ref head, out outPiece),
            "Body" => TryAddPiece(pieceToSet, ref body, out outPiece),
            "Feet" => TryAddPiece(pieceToSet, ref feet, out outPiece),
            _ => false
        };

        return result;
    }
    public void ForceAddPart(TotemPiece pieceToSet, out TotemPiece outPiece, string tagg)
    {
        outPiece = null;
        
        switch (tagg)
        {
            case "Head":
                ForceAddPiece(pieceToSet, ref head, out outPiece);
                break;
            case "Body":
                ForceAddPiece(pieceToSet, ref body, out outPiece);
                break;
            case "Feet":
                ForceAddPiece(pieceToSet, ref feet, out outPiece);
                break;
            default:
                break;
        }
    }
    public Transform GetPiece(string pieceTag)
    {
        return pieceTag switch
        {
            "Head" => head,
            "Body" => body,
            "Feet" => feet,
            _ => null
        };
    }

    private bool TryAddPiece(TotemPiece pieceToSet, ref Transform other, out TotemPiece outPiece)
    {
        if (!other)
        {
            outPiece = null;
            return false;
        }

        outPiece = other.GetComponent<TotemPiece>();
        other = pieceToSet.transform;
        other.GetComponent<TotemPiece>().totem = this;
        other.SetParent(_transform);
        return true;
    }
    
    private void ForceAddPiece(TotemPiece pieceToSet, ref Transform other, out TotemPiece outPiece)
    {
        // if (!other)
        // {
        //     outPiece = null;
        //     return;
        // }

        outPiece = other ? other.GetComponent<TotemPiece>() : null;
        if (!pieceToSet)
        {
            other = null;
            return;
        }
        other = pieceToSet.transform;
        other.GetComponent<TotemPiece>().totem = this;
        other.SetParent(_transform);
        if (!head && !body && !feet)
        {
            Destroy(gameObject);
        }
    }
    
    public void Separate(int mode)
    {
        if(isLocked)    return;
        // if(!(head && feet && body))    return;
        
        var distanceHead = TotemOffset;
        float distanceBody = 0;
        switch (mode)
        {
            case 1:
                distanceHead += TotemPieceHover;
                break;
            case 2:
                distanceHead += TotemPieceHover * 2;
                distanceBody += TotemPieceHover;
                break;
            case 3:
                distanceHead += TotemPieceHover;
                distanceBody += TotemPieceHover;
                break;
        }

        var position = transform.position;
        var up = _transform.up;
        var speed = 0.1f;
        if(head)
            head.GetComponent<TotemPiece>().MoveTo(position + (up * distanceHead), speed);
        if(body)
            body.GetComponent<TotemPiece>().MoveTo(position + (up * distanceBody), speed);
        if(feet)
            feet.GetComponent<TotemPiece>().MoveTo(position - up * TotemOffset , speed);

    }

    public bool CanTakePart(GameObject part)
    {
        // Debug.LogWarning(feet);
        return part.tag switch
        {
            "Feet" => feet,
            "Body" => body,
            "Head" => head,
            _ => false
        };
    }
    
    // Start is called before the first frame update
    private void Start()
    {
        _transform = transform;
        // if(head)
        //     totemHead = head.GetComponent<TotemPiece>();
        // if(body)
        //     totemBody = body.GetComponent<TotemPiece>();
        // if(feet)
        //     totemFeet = feet.GetComponent<TotemPiece>();
    }

    public void Deactivate()
    {
        if(isLocked)    return;
        
        Separate(-1);
        // transform.GetChild(0).GetComponent<TotemPiece>().MoveTo(_transform.position + TotemOffset * _transform.up, 0.01f);
        // transform.GetChild(1).GetComponent<TotemPiece>().MoveTo(_transform.position, 0.01f);
        // transform.GetChild(2).GetComponent<TotemPiece>().MoveTo(_transform.position - TotemOffset * _transform.up, 0.01f);
    }
    
    
    // private void OnMouseExit()
    // {
    //     Debug.Log("Uwu??");
    //     totemHead.MoveTo(_transform.position + _transform.up * TotemOffset, 0.01f);
    //     totemBody.MoveTo(transform.position,0.01f);
    // }

    // private void OnTriggerExit(Collider other)
    // {
    //     if (other.CompareTag("Pointer"))
    //     {
    //         Deactivate();
    //     }
    // }

    public void Lock(bool locked)
    {
        isLocked = locked;
    }
    // Update is called once per frame
    private void Update()
    {
        
    }
}